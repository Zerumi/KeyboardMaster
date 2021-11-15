using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using m3md2;
using Newtonsoft.Json;

namespace KeyboardMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FlowDocument Words = new();
        public Paragraph WordsParagraph = new();
        public FlowDocument WrittenWords = new();
        public Paragraph WrittenWordsParagraph = new();
        public string sWords = string.Empty;

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer TimeTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            lWelcome.Content = $"Загрузка...";
            SetupDictonaty(ConfigurationRequest.GetDictonary());
            SetupTextBoxes();
            SetupTimer();
            //Путь к гиф
            media.Source = new Uri(Environment.CurrentDirectory + "\\Attachments\\d1.gif");
            media2.Source = new Uri(Environment.CurrentDirectory + "\\Attachments\\d2.gif");
            //Метод инициализации таймера
            Loading();
            charsPerMinute charsPerMinute = new charsPerMinute();
            charsPerMinute.Activate();
            _ = tInput.Focus();
        }

        #region Dictonary & TextBoxes
        DispatcherTimer timer1 = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        private void timer_tick(object sender, EventArgs e)
        {
            //Отображаем клавиатуру
            gKeyboard.Visibility = Visibility.Visible;
            gCorePerfomance.Visibility = Visibility.Visible;
            gTextInput.Visibility = Visibility.Visible;
            gTextPerfomance.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Visible;
            lWelcome.Visibility = Visibility.Visible;
            lTime.Visibility = Visibility.Visible;

            //Останавливаем таймер
            timer1.Stop();
            //Скрываем гиф
            media.Visibility = Visibility.Hidden;
            media2.Visibility = Visibility.Hidden;
            //Скрываем название проекта
            loadLabel.Visibility = Visibility.Hidden;
        }
        private void timer_tick2(object sender, EventArgs e)
        {

            media2.Visibility = Visibility.Visible;
            loadLabel.Visibility = Visibility.Visible;
            timer2.Stop();
        }
        void Loading()
        {
            timer1.Tick += timer_tick;

            timer1.Interval = new TimeSpan(0, 0, 0, 4, 800);

            timer1.Start();

            timer2.Tick += timer_tick2;

            timer2.Interval = new TimeSpan(0, 0, 0, 2, 0);

            timer2.Start();
        }
        private void SetupDictonaty(IDictonary dictonary)
        {
            Random random = new(unchecked((int)DateTime.Now.Ticks));

            List<int> indexes = new();

            for (int i = 0; i < dictonary.Words.Length; i++)
            {
                indexes.Add(i);
            }

            for (int i = 0; i < dictonary.Words.Length; i++)
            {
                int index = random.Next(dictonary.Words.Length);
                while (!indexes.Contains(index))
                {
                    index = random.Next(dictonary.Words.Length);
                }
                _ = indexes.Remove(index);

                Run myRun = new($"{dictonary.Words[index]} ");
                WordsParagraph.Inlines.Add(myRun);
            }
            Words.Blocks.Add(WordsParagraph);
            tbWords.Document = Words;
        }

        private void SetupTextBoxes()
        {
            TextRange textRange = new(tbWords.Document.ContentStart, tbWords.Document.ContentEnd);
            sWords = textRange.Text;
            currentword = sWords.Substring(0, sWords.IndexOf(' ') + 1);
            TextRange textRange2 = new(tbWords.Document.ContentStart, tbWords.CaretPosition.GetPositionAtOffset(currentword.Length));
            textRange2.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
        }
        #endregion

        #region TimerLogic
        private void SetupTimer()
        {
            string time = ConfigurationRequest.GetTime();
            lTimer.Content = time;
            int mins = int.Parse(time.Substring(0, time.IndexOf(':')));
            int secs = int.Parse(time.Substring(time.IndexOf(':') + 1));
            totalmins = mins + (double)((double)secs / 60);
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += Timer_Tick;


            TimeTimer.Tick += TimeTimerTick;
            TimeTimer.Interval = new TimeSpan(0, 0, 1);
            TimeTimer.Start();
        }

        private void TimeTimerTick(object sender, EventArgs e)
        {
            lTime.Content = DateTime.Now.ToString();
        }

        double totalmins;

        private async void Timer_Tick(object sender, EventArgs e)
        {
            string time = Convert.ToString(lTimer.Content);
            int mins = int.Parse(time.Substring(0, time.IndexOf(':')));
            int secs = int.Parse(time.Substring(time.IndexOf(':') + 1));
            if (secs == 0)
            {
                if (mins-- == 0) // block UI, refreshing
                {
                    await Dispatcher.BeginInvoke(new Action(async() =>
                    {
                        tInput.TextChanged -= tInput_TextChanged;
                        timer.Stop();
                        tInput.IsEnabled = false;
                        tInput.Text = "";
                        TextPerfomance.WordsPerMinute = Convert.ToInt32(Math.Floor((TextPerfomance.IdealWords + TextPerfomance.ErrorWords - TextPerfomance.WrongWords) / totalmins));
                        lWPM.Content = $"Слов в минуту: {TextPerfomance.WordsPerMinute}";
                        tbWords.Document.Blocks.Clear();
                        Words = new FlowDocument();
                        WordsParagraph = new Paragraph();
                        WrittenWords = new FlowDocument();
                        WrittenWordsParagraph = new Paragraph();
                        sWords = string.Empty;
                        SetupDictonaty(ConfigurationRequest.GetDictonary());
                        new Network().SubmitScore();
                        lTimer.Content = ConfigurationRequest.GetTime();
                        isTimerStarted = false;
                        SetupTextBoxes();
                        await Task.Delay(5500);
                        tInput.TextChanged += tInput_TextChanged;
                        tInput.IsEnabled = true;
                        tInput.Focus();
                    }));
                    return;
                }
                secs = 60;
            }
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                lTimer.Content = $"{mins}:{--secs}";
            }));
        }
#endregion

        #region TextInputLogic
        private int rightindex = 0;
        private string OldText = string.Empty;
        private void tInput_GotFocus(object sender, RoutedEventArgs e)
        {
            OldText = tInput.Text;
        }

        private bool isFromNewWord = false;
        private bool isIdealWord = true;
        private string currentword = string.Empty;
        private bool isTimerStarted = false;

        private void tInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!isTimerStarted) // starts new test
                {
                    tbWrittenWords.Document.Blocks.Clear();
                    timer.Start();

                    MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
                    charsPerMinute.best = 0;
                    charsPerMinute.sum = 0;
                    charsPerMinute.counter = 1;//Обнуление значений перед началом нового сбора данных
                    App.watch.Stop();
                    App.watch.Reset();
                    App.best_latency = int.MaxValue;
                    App.counter = 1;
                    App.sum = 0;
                    main.best_latency.Content = $"Лучшая задержка: {0}ms";
                    main.print_delay.Content = $"Задержка печати: {0}ms";
                    main.avr_print_delay.Content = $"Средняя задержка: {0}ms";
                    main.CPM.Content = $"Символов в минуту: {0}";
                    main.ACPM.Content = $"Среднее число символов в минуту: {0}";
                    main.best_CPM.Content = $"Лучшее число символов в минуту: {0}";

                    isTimerStarted = true;
                }
                if (isFromNewWord)
                {
                    isFromNewWord = false;
                    isIdealWord = true;
                    currentword = sWords.Substring(0, sWords.IndexOf(' ') + 1);
                    TextRange textRange = new(tbWords.Document.ContentStart, tbWords.CaretPosition.GetPositionAtOffset(currentword.Length));
                    textRange.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
                }
                else if (OldText == string.Empty)
                {
                    char rightchar = sWords[rightindex++];
                    TextRange textRange = new(tbWords.Document.ContentStart, tbWords.CaretPosition.GetPositionAtOffset(currentword.Length));

                    if (tInput.Text == " ") // Word ended
                    {
                        textRange.Text = "";
                        sWords = sWords[(sWords.IndexOf(' ') + 1)..];
                        rightindex = 0;
                        isFromNewWord = true;
                        if (isIdealWord)
                        {
                            lIdealWords.Content = $"Идеально написанные слова: {++TextPerfomance.IdealWords}";
                        } 
                        else if (tInput.Text != currentword)
                        {
                            lWrongWords.Content = $"Завершенные слова с опечатками: {++TextPerfomance.WrongWords}";
                        }
                        WrittenWordsParagraph.Inlines.Add(new Run(currentword) { Foreground = tInput.Text == currentword ? isIdealWord ? new SolidColorBrush(Color.FromRgb(0, 255, 0)) : new SolidColorBrush(Color.FromRgb(255, 255, 0)) : new SolidColorBrush(Color.FromRgb(255, 0, 0)) });
                        WrittenWords.Blocks.Clear();
                        WrittenWords.Blocks.Add(WrittenWordsParagraph);
                        tbWrittenWords.Document = WrittenWords;
                        tInput.Text = string.Empty;
                    }
                    else if (tInput.Text[^1] == rightchar) // right symbol
                    {
                        if (tInput.Text == currentword.Substring(0, rightindex))
                        {
                            textRange.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
                        }
                        lCorrectChars.Content = $"Правильно напечатанных символов: {++TextPerfomance.CorrectChars}";
                        lAccuracy.Content = $"Аккуратность: {TextPerfomance.Accuracy}%";
                    }
                    else if (tInput.Text[^1] != rightchar) // wrong symbol
                    {
                        textRange.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
                        lIncorrectChars.Content = $"Неправильно напечатанных символов: {++TextPerfomance.IncorrectChars}";
                        lAccuracy.Content = $"Аккуратность: {TextPerfomance.Accuracy}%";
                        if (isIdealWord)
                        {
                            isIdealWord = false;
                            lErrorWords.Content = $"Слова с ошибками: {++TextPerfomance.ErrorWords}";
                        }
                    }
                }
                else if (tInput.Text == string.Empty && !isFromNewWord) // Ctrl + A -> Delete
                {
                    rightindex -= OldText.Length;
                }
                else if (tInput.Text.Replace(OldText, "").Length > 1 && tInput.Text.Replace(OldText, "") != tInput.Text) // Ctrl + V something
                {
                    tInput.Text = OldText;
                }
                else if (OldText.Replace(tInput.Text, "").Length > 1 && OldText.Replace(tInput.Text, "") != OldText) // Manual removing (not Backspace)
                {
                    tInput.Text = OldText;
                }
                else if (tInput.Text.Replace(OldText, "").Length == 1) // Added 1 symbol
                {
                    char rightchar = sWords[rightindex++]; 
                    TextRange textRange = new(tbWords.Document.ContentStart, tbWords.CaretPosition.GetPositionAtOffset(currentword.Length));
                    
                    if (tInput.Text.Replace(OldText, "") == " ") // Word ended
                    {
                        textRange.Text = "";
                        sWords = sWords[(sWords.IndexOf(' ') + 1)..];
                        rightindex = 0;
                        isFromNewWord = true;
                        if (isIdealWord)
                        {
                            lIdealWords.Content = $"Идеально написанные слова: {++TextPerfomance.IdealWords}";
                        }
                        else if (tInput.Text != currentword)
                        {
                            lWrongWords.Content = $"Завершенные слова с опечатками: {++TextPerfomance.WrongWords}";
                        }
                        WrittenWordsParagraph.Inlines.Add(new Run(currentword) { Foreground = tInput.Text == currentword ? isIdealWord ? new SolidColorBrush(Color.FromRgb(0, 255, 0)) : new SolidColorBrush(Color.FromRgb(255, 255, 0)) : new SolidColorBrush(Color.FromRgb(255, 0, 0)) });
                        WrittenWords.Blocks.Clear();
                        WrittenWords.Blocks.Add(WrittenWordsParagraph);
                        tbWrittenWords.Document = WrittenWords;
                        tInput.Text = string.Empty;
                    }
                    else if (tInput.Text[^1] == rightchar) // right symbol
                    {
                        if (tInput.Text == currentword.Substring(0, rightindex))
                        {
                            textRange.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
                        }
                        lCorrectChars.Content = $"Правильно напечатанных символов: {++TextPerfomance.CorrectChars}";
                        lAccuracy.Content = $"Аккуратность: {TextPerfomance.Accuracy}%";
                    }
                    else if (tInput.Text[^1] != rightchar) // wrong symbol
                    {
                        textRange.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
                        lIncorrectChars.Content = $"Неправильно напечатанных символов: {++TextPerfomance.IncorrectChars}";
                        lAccuracy.Content = $"Аккуратность: {TextPerfomance.Accuracy}%";
                        if (isIdealWord)
                        {
                            isIdealWord = false;
                            lErrorWords.Content = $"Слова с ошибками: {++TextPerfomance.ErrorWords}";
                        }
                    }
                }
                else if (OldText.Replace(tInput.Text, "").Length == 1) // Removed 1 symbol
                {
                    --rightindex;
                }
                OldText = tInput.Text;
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lWelcome.Content = $"Подключаемся к серверу...";
            _ = AuthUser(Environment.UserName, out m3md2.StaticVariables.AuthCookie);
            lWelcome.Content = $"{Parser.GetWelcomeLabel(Parser.GetTimeDescription(DateTime.Now))}, {Environment.UserName}!";
        }

        public static Client AuthUser(string username, out Cookie cookie) // метод для авторизации на сервере
        {
            Client returnproduct = default;
            try
            {
                CookieContainer cookies = new CookieContainer(); // авторизуем, отправляя Post запрос с нужными параметрами
                HttpClientHandler handler = new HttpClientHandler
                {
                    CookieContainer = cookies
                };
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(m3md2.StaticVariables.BaseServerAddress)
                };
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
                string json = JsonConvert.SerializeObject(username);
                HttpResponseMessage response = client.PostAsync($"auth", new StringContent(json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                returnproduct = response.Content.ReadAsAsync<Client>().GetAwaiter().GetResult();
                try // Получаем куки
                {
                    Uri uri = new Uri($"{m3md2.StaticVariables.BaseServerAddress}auth");
                    var collection = cookies.GetCookies(uri);
                    cookie = collection[".AspNetCore.Cookies"];
                }
                catch (Exception ex)
                {
                    ExceptionHandler.RegisterNew(ex);
                    cookie = default;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
                cookie = default;
            }
            return returnproduct;
        }
    }
}
