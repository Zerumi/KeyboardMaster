using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using m3md2;

namespace KeyboardMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly FlowDocument Words = new();
        public readonly Paragraph WordsParagraph = new();
        public readonly FlowDocument WrittenWords = new();
        public readonly Paragraph WrittenWordsParagraph = new();
        public string sWords = string.Empty;

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            SetupDictonaty(ConfigurationRequest.GetDictonary());
            SetupTextBoxes();
            SetupTimer();
            _ = tInput.Focus();
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

        private void SetupTimer()
        {
            lTimer.Content = ConfigurationRequest.GetTime();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            string time = Convert.ToString(lTimer.Content);
            int mins = int.Parse(time.Substring(0, time.IndexOf(':')));
            int secs = int.Parse(time.Substring(time.IndexOf(':') + 1));
            if (secs == 0)
            {
                if (mins-- == 0)
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        timer.Stop();
                        tInput.IsEnabled = false;
                        tbWrittenWords.Document.Blocks.Clear();
                        SetupDictonaty(ConfigurationRequest.GetDictonary());
                        Network.SubmitScore();
                        lTimer.Content = ConfigurationRequest.GetTime();
                        tInput.IsEnabled = true;
                    }));
                    isTimerStarted = false;
                    return;
                }
                secs = 60;
            }
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                lTimer.Content = $"{mins}:{--secs}";
            }));
        }

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
                if (!isTimerStarted)
                {
                    // tbWrittenWords.Document.Blocks.Clear(); // fix
                    timer.Start();
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
    }
}
