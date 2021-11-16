using KeyboardMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace KeyboardMaster
{
    class charsPerMinute
    {
        public static DispatcherTimer timer = new DispatcherTimer();
        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
        public static List<string> chars = new List<string>();
        static ICorePerfomance corePerfomance = new CorePerfomanceLogic();

        public void Activate()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            CorePerfomance.CharsPerMinute = chars.Count * 60;
            string output = $"Символов в минуту: {chars.Count * 60}";//Строка выходных данных о количестве символов в минуту
            main.CPM.Content = output;
            if (chars.Count!=0)
            {
                sum += chars.Count() * 60;
                CorePerfomance.AverageCPM = sum / counter;
                string ACMP_output = $"Среднее число символов в минуту: {sum / counter}";
                main.ACPM.Content = ACMP_output;
                counter++;
            }
            if (best<chars.Count)//Проверка максимального числа символов в минуту
            {
                best = chars.Count;
                CorePerfomance.BestCPM = chars.Count * 60;
                string bestCPM_output = $"Лучшее число символов в минуту: {chars.Count * 60}";
                main.best_CPM.Content = bestCPM_output;
            }

            corePerfomance.CPM(chars.Count);
            corePerfomance.ACPM(chars.Count);
            corePerfomance.bestCPM(chars.Count);

            chars = new List<string>();
            timer.Start();
        }
    }
}
