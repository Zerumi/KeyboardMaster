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
        TrainingMode trainingmode;

        public TrainingMode()
        {
            trainingmode = this;
            InitializeComponent();
            media.Source = new Uri(Environment.CurrentDirectory + "\\Attachments\\d3.gif");
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Position = new TimeSpan(0, 0, 1);
            media.Play();
        }

        private void startBut_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)comboBox.SelectedItem;
            if (item.Content.ToString() != "Выберите уровень сложности")
            {
                ITrainingModeLogic trainingMode = new TrainingModeLogic(this);
                trainingMode.Start(int.Parse(item.Content.ToString()));
            }
            else
            {
                MessageBox.Show("Выберите уровень сложности");
            }
            startBut.IsEnabled = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TrainingModeLogic.timer.Stop();
            App.isTraining = false;
        }
    }
}
