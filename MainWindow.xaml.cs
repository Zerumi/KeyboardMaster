using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //Путь к гиф
            media.Source = new Uri(Environment.CurrentDirectory + "\\Attachments\\d1.gif");
            media2.Source = new Uri(Environment.CurrentDirectory + "\\Attachments\\d2.gif");
            media3.Source = new Uri(Environment.CurrentDirectory + "\\Attachments\\d2.gif");
            //Метод инициализации таймера
            LoadingScreenSetup();
            SetupDictonaries();
            SetupDictonaty(ConfigurationRequest.GetDictonary());
            SetupTextBoxes();
            SetupTimer();
            charsPerMinute charsPerMinute = new charsPerMinute();
            charsPerMinute.Activate();
            _ = tInput.Focus();
        }

        #region AnimationScreen
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
            media3.Visibility = Visibility.Visible;

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
        void LoadingScreenSetup()
        {
            timer1.Tick += timer_tick;

            timer1.Interval = new TimeSpan(0, 0, 0, 4, 800);

            timer1.Start();

            timer2.Tick += timer_tick2;

            timer2.Interval = new TimeSpan(0, 0, 0, 2, 0);

            timer2.Start();
        }
        #endregion

        #region Dictonary & TextBoxes
        private void SetupDictonaries()
        {
            for (int i = 0; i < Enum.GetNames(typeof(LanguageDictonary)).Length; i++)
            {
                string tempname = Enum.GetNames(typeof(LanguageDictonary))[i];
                MenuItem temp = new MenuItem()
                {
                    Name = tempname,
                    Header = tempname
                };
                temp.Click += Temp_Click;
                mLangs.Items.Add(temp);
            }
        }

        private void Temp_Click(object sender, RoutedEventArgs e)
        {
            switch ((LanguageDictonary)Enum.Parse(typeof(LanguageDictonary), (sender as FrameworkElement).Name))
            {
                case LanguageDictonary.RU:
                    SetupDictonaty(new RuDictonary());
                    break;
                case LanguageDictonary.EN:
                    SetupDictonaty(new EnDictonary());
                    break;
                default:
                    break;
            }

            SetupTextBoxes();
        }

        private void SetupDictonaty(IDictonary dictonary)
        {
            TextRange textRange = new(tbWords.Document.ContentStart, tbWords.Document.ContentEnd);
            textRange.Text = "";

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
                        swordtime.Stop();
                        tInput.IsEnabled = false;
                        tInput.Text = "";
                        ICorePerfomance corePerfomance = new CorePerfomanceLogic();
                        corePerfomance.CorePerfomancePoints(CorePerfomance.AverageCPM,  CorePerfomance.printingUniformity);
                        TextPerfomance.StreakIdealWords = maxstreak;
                        TextPerfomance.WordsPerMinute = Convert.ToInt32(Math.Floor((TextPerfomance.IdealWords + TextPerfomance.ErrorWords - TextPerfomance.WrongWords) / totalmins));
                        lWPM.Content = $"Слов в минуту: {TextPerfomance.WordsPerMinute}";
                        lIdealWordStreak.Content = $"Серия идеально написанных слов: {maxstreak}";
                        tbWords.Document.Blocks.Clear();
                        Words = new FlowDocument();
                        WordsParagraph = new Paragraph();
                        WrittenWords = new FlowDocument();
                        WrittenWordsParagraph = new Paragraph();
                        sWords = string.Empty;
                        SetupDictonaty(ConfigurationRequest.GetDictonary());
                        Network.SubmitScore();
                        lTimer.Content = ConfigurationRequest.GetTime();
                        isTimerStarted = false;
                        SetupTextBoxes();
                        await Task.Delay(5500);
                        tInput.TextChanged += tInput_TextChanged;
                        tInput.IsEnabled = true;
                        _ = tInput.Focus();
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
        private bool isStreak = true;
        private int currentstreak = 0;
        private int maxstreak = 0;
        private double lasttimervalue = 0;

        Stopwatch swordtime = new Stopwatch();

        private void tInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!isTimerStarted) // starts new test
                {
                    CorePerfomanceLogic.best = 0;
                    CorePerfomanceLogic.sumCh = 0;
                    CorePerfomanceLogic.counter = 1;//Обнуление значений перед началом нового сбора данных
                    App.watch.Stop();
                    App.watch.Reset();
                    CorePerfomanceLogic.best_latency = int.MaxValue;
                    CorePerfomanceLogic.counter = 1;
                    CorePerfomanceLogic.sum = 0;
                    CorePerfomanceLogic.uniformitySum = 0;
                    best_latency.Content = $"Лучшая задержка: {0}ms";
                    print_delay.Content = $"Задержка печати: {0}ms";
                    avr_print_delay.Content = $"Средняя задержка: {0}ms";
                    CPM.Content = $"Символов в минуту: {0}";
                    ACPM.Content = $"Среднее число символов в минуту: {0}";
                    best_CPM.Content = $"Лучшее число символов в минуту: {0}";
                    printing_uniformity.Content = $"Равномерность печати: {0}";

                    TextPerfomance.IdealWords = 0;
                    TextPerfomance.AverageWPM = 0;
                    TextPerfomance.WordsPerMinute = 0;
                    TextPerfomance.CorrectChars = 0;
                    TextPerfomance.ErrorWords = 0;
                    TextPerfomance.WrongWords = 0;
                    TextPerfomance.IncorrectChars = 0;
                    TextPerfomance.StreakIdealWords = 0;
                    lCorrectChars.Content = "Правильно напечатанных символов: 0";
                    lIncorrectChars.Content = "Неправильно напечатанных символов: 0";
                    lAccuracy.Content = "Аккуратность: 0.00%";
                    lIdealWords.Content = "Идеально написанные слова: 0";
                    lErrorWords.Content = "Слова с опечатками: 0";
                    lWrongWords.Content = "Завершенные слова с опечатками: 0";
                    lWPM.Content = "Фактически слов в минуту: Только в конце теста";
                    lAverageWPM.Content = "В среднем слов в минуту: 0";
                    lIdealWordStreak.Content = "Серия идеально написанных слов: 0";
                    lWordAccuracy.Content = "Аккуратность написания слов: 0.00%";
                    lTextPP.Content = "Показатель производительности: 0";

                    tbWrittenWords.Document.Blocks.Clear();
                    timer.Start();
                    swordtime.Start();
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
                        double totalsecs = swordtime.Elapsed.TotalMilliseconds / 1000;
                        textRange.Text = "";
                        sWords = sWords[(sWords.IndexOf(' ') + 1)..];
                        rightindex = 0;
                        isFromNewWord = true;
                        if (isIdealWord && tInput.Text == currentword)
                        {
                            lIdealWords.Content = $"Идеально написанные слова: {++TextPerfomance.IdealWords}";
                            if (isStreak)
                            {
                                lIdealWordStreak.Content = $"Серия идеально написанных слов: {++currentstreak}";
                            }
                            else
                            {
                                isStreak = true;
                            }
                        }
                        else if (tInput.Text != currentword)
                        {
                            lWrongWords.Content = $"Завершенные слова с опечатками: {++TextPerfomance.WrongWords}";
                            isStreak = false;
                            currentstreak = 0;
                            if (currentstreak > maxstreak)
                            {
                                maxstreak = currentstreak;
                            }
                        }
                        if (tInput.Text == currentword)
                        {
                            double wordtime = totalsecs - lasttimervalue;
                            int wordlength = currentword.Length;
                            TextPerfomance.AverageWPM = (int)Math.Round((double)((double)wordlength / ConfigurationRequest.GetDictonary().AverageLettersInWords) * (1 / (double)wordtime) * 60);
                            lAverageWPM.Content = $"В среднем слов в минуту: {TextPerfomance.AverageWPM}";
                        }
                        lasttimervalue = totalsecs;
                        ++TextPerfomance.currentwrittenwords;
                        lWordAccuracy.Content = $"Аккуратность написания слов: {TextPerfomance.WordAccuracy}%";
                        lTextPP.Content = $"Показатель производительности: {TextPerfomance.TextPerfomancePoints}";
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
                    TextRange textRange = new(tbWords.Document.ContentStart, tbWords.CaretPosition.GetPositionAtOffset(currentword.Length));
                    textRange.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
                }
                else if (tInput.Text.Replace(OldText, "").Length > 1 && tInput.Text.Replace(OldText, "") != tInput.Text) // Ctrl + V something
                {
                    tInput.Text = OldText;
                }
                else if (OldText.Replace(tInput.Text, "").Length > 1 && OldText.Replace(tInput.Text, "") != OldText) // Manual removing (not Backspace)
                {
                    tInput.Text = OldText;
                }
                else if (tInput.Text.Replace(OldText, "").Length == 1 && tInput.Text.Replace(OldText, "") != tInput.Text) // Added 1 symbol
                {
                    char rightchar = sWords[rightindex++]; 
                    TextRange textRange = new(tbWords.Document.ContentStart, tbWords.CaretPosition.GetPositionAtOffset(currentword.Length));
                    
                    if (tInput.Text.Replace(OldText, "") == " ") // Word ended
                    {
                        double totalsecs = swordtime.Elapsed.TotalMilliseconds / 1000;
                        textRange.Text = "";
                        sWords = sWords[(sWords.IndexOf(' ') + 1)..];
                        rightindex = 0;
                        isFromNewWord = true;
                        if (isIdealWord)
                        {
                            lIdealWords.Content = $"Идеально написанные слова: {++TextPerfomance.IdealWords}";
                            if (isStreak)
                            {
                                lIdealWordStreak.Content = $"Серия идеально написанных слов: {++currentstreak}";
                            }
                            else
                            {
                                isStreak = true;
                            }
                        }
                        else if (tInput.Text != currentword)
                        {
                            lWrongWords.Content = $"Завершенные слова с опечатками: {++TextPerfomance.WrongWords}";
                            isStreak = false;
                            currentstreak = 0;
                            if (currentstreak > maxstreak)
                            {
                                maxstreak = currentstreak;
                            }
                        }
                        if (tInput.Text == currentword)
                        {
                            double wordtime = totalsecs - lasttimervalue;
                            int wordlength = currentword.Length;
                            TextPerfomance.AverageWPM = (int)Math.Round((double)((double)wordlength / ConfigurationRequest.GetDictonary().AverageLettersInWords) * (1 / (double)wordtime) * 60);
                            lAverageWPM.Content = $"В среднем слов в минуту: {TextPerfomance.AverageWPM}";
                        }
                        lasttimervalue = totalsecs;
                        ++TextPerfomance.currentwrittenwords;
                        lWordAccuracy.Content = $"Аккуратность написания слов: {TextPerfomance.WordAccuracy}%";
                        lTextPP.Content = $"Показатель производительности: {TextPerfomance.TextPerfomancePoints}";
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
                    TextRange textRange = new(tbWords.Document.ContentStart, tbWords.CaretPosition.GetPositionAtOffset(currentword.Length));

                    if (tInput.Text == currentword.Substring(0, rightindex))
                    {
                        textRange.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
                    }
                }
                OldText = tInput.Text;
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }
        #endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lWelcome.Content = $"Подключаемся к серверу...";
            _ = AuthUser(Environment.UserName, out m3md2.StaticVariables.AuthCookie);
            await Network.ConfigureConnection();
            lWelcome.Content = $"{Parser.GetWelcomeLabel(Parser.GetTimeDescription(DateTime.Now))}, {Environment.UserName}!";
        }

        private void mAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Keyboard Master by CrutchGroup\nMade for Samara IT Hackaton by IT Cube 15-16/11/2021\nVersion 1.0 stable\nKeyboardMaster-server v.1.0 stable\nMade by Zerumi & PizhikCoder (Discord: Zerumi#4666 / PizhikCoder#4565)\nGitHub: https://github.com/Zerumi \nGitHub: https://github.com/PizhikCoder");
        }
        public TrainingMode trainingMode = default;

        private void mTrainingMode_Click(object sender, RoutedEventArgs e)
        {
            trainingMode = new TrainingMode();
            trainingMode.Show();
        }

        private void mPerfomanceRanking_Click(object sender, RoutedEventArgs e)
        {
            PerfomanceRanking perfomanceRanking = new PerfomanceRanking();
            perfomanceRanking.Show();
        }

        private void media3_MediaEnded(object sender, RoutedEventArgs e)
        {
            media3.Position = new TimeSpan(0, 0, 1);
            media3.Play();
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
