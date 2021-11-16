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
using System.Windows.Shapes;

namespace KeyboardMaster
{
    /// <summary>
    /// Логика взаимодействия для TrainingMode.xaml
    /// </summary>
    public partial class TrainingMode : Window
    {
        public TrainingMode()
        {
            InitializeComponent();
            media.Source = new Uri(Environment.CurrentDirectory + "\\Attachments\\d3.gif");
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Position = new TimeSpan(0,0,1);
            media.Play();
        }
    }
}
