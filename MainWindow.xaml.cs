using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string sWords = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            SetupDictonaty(ConfigurationRequest.GetDictonary());
            TextRange textRange = new(tbWords.Document.ContentStart, tbWords.Document.ContentEnd);
            sWords = textRange.Text;
        }

        private void SetupDictonaty(IDictonary dictonary)
        {
            Random random = new(unchecked((Int32)DateTime.Now.Ticks));

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

        int rightindex = 0;

        string OldText = string.Empty;
        private void tInput_GotFocus(object sender, RoutedEventArgs e)
        {
            OldText = tInput.Text;
        }

        bool isFromNewWord = false;

        private void tInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (isFromNewWord)
                {
                    isFromNewWord = false;
                }
                else if (OldText == string.Empty)
                {
                    char rightchar = sWords[rightindex++];
                    if (tInput.Text == " ") // Word ended
                    {
                        // edit flowdocument // word skipped
                        sWords = sWords[(sWords.IndexOf(' ') + 1)..];
                        rightindex = 0;
                        isFromNewWord = true;
                        tInput.Text = string.Empty;
                    }
                    else if (tInput.Text[^1] == rightchar)
                    {
                        lCorrectChars.Content = $"Правильно напечатанных символов: {++TextPerfomance.CorrectChars}";
                    }
                    else if (tInput.Text[^1] != rightchar)
                    {
                        lIncorrectChars.Content = $"Неправильно напечатанных символов: {++TextPerfomance.IncorrectChars}";
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
                    if (tInput.Text.Replace(OldText, "") == " ") // Word ended
                    {
                        // edit flowdocument
                        sWords = sWords[(sWords.IndexOf(' ') + 1)..];
                        rightindex = 0;
                        isFromNewWord = true;
                        tInput.Text = string.Empty;
                    }
                    else if (tInput.Text[^1] == rightchar)
                    {
                        lCorrectChars.Content = $"Правильно напечатанных символов: {++TextPerfomance.CorrectChars}";
                    }
                    else if (tInput.Text[^1] != rightchar)
                    {
                        lIncorrectChars.Content = $"Неправильно напечатанных символов: {++TextPerfomance.IncorrectChars}";
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
