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
    /// Логика взаимодействия для DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window
    {
        public DialogBox()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter or Key.Space)
            {
                if (tbTime.Text.Split(':').Length == 2)
                {
                    foreach (var x in tbTime.Text.Split(':'))
                    {
                        if (!int.TryParse(x, out _))
                        {
                            _ = MessageBox.Show("Преобразование завершено с ошибкой.");
                            Close();
                            return;
                        }
                    }
                    if (int.Parse(tbTime.Text.Split(':')[0]) == 0 && int.Parse(tbTime.Text.Split(':')[1]) < 30)
                    {
                        _ = MessageBox.Show("Введите значение, большее 30и секунд!");
                        return;
                    }
                    Close();
                    (App.Current.MainWindow as MainWindow).lTimer.Content = tbTime.Text;
                }
                else
                {
                    _ = MessageBox.Show("Преобразование завершено с ошибкой.");
                }
            }
        }
    }
}
