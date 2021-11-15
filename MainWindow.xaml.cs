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
        public MainWindow()
        {
            InitializeComponent();
            SetupDictonaty(ConfigurationRequest.GetDictonary());
        }

        private void SetupDictonaty(IDictonary dictonary)
        {
            Random random = new Random(unchecked((Int32)DateTime.Now.Ticks));

            List<int> indexes = new List<int>();

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
                indexes.Remove(index);
                tbWords.Text += $"{dictonary.Words[index]}, ";
            }
        }
    }
}
