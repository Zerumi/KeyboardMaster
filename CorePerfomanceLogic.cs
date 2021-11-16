using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace KeyboardMaster
{
    class CorePerfomanceLogic:ICorePerfomance
    {
        #region CPMLogic
        public static int best = 0;
        public static int sumCh = 0;//Сумма символов за текущий временной промежуток
        public static int counterCh = 1;//Счетчик итераций сбора данных о нажатых символах


        public void CPM(int count)
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
            CorePerfomance.CharsPerMinute = count * 60;
            string output = $"Символов в минуту: {count * 60}";//Строка выходных данных о количестве символов в минуту
            main.CPM.Content = output;
        }
        public void ACPM(int count)
        {
            if (count != 0)
            {
                MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
                sumCh += count * 60;
                string ACMP_output = $"Среднее число символов в минуту: {sumCh / counterCh}";
                main.ACPM.Content = ACMP_output;
                counterCh++;
            }
        }

        public void bestCPM(int count)
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (best < count)
            {
                best = count;
                string bestCPM_output = $"Лучшее число символов в минуту: {count * 60}";
                main.best_CPM.Content = bestCPM_output;
            }
        }
        #endregion
        #region LatenciesLogic
        public static int best_latency = int.MaxValue;
        public static int sum = 0;
        public static int lastLatency = 1;
        public static int counter = 1;
        public static int uniformitySum = 0;

        public void bestLatency(int ElapsedMilliseconds)//Лучшая задержка
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (best_latency > ElapsedMilliseconds && ElapsedMilliseconds != 0) 
            {
                best_latency = ElapsedMilliseconds;
                string latency_record = $"Лучшая задержка: {best_latency}ms";
                main.best_latency.Content = latency_record;
            }
        }

        public void printingUniformity(int currentlatency)
        {

            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
            int result = 0;
            currentlatency = currentlatency == 0 ? 1 : currentlatency;

            if (lastLatency < currentlatency)
            {
                result = (int)(((float)lastLatency / (float)currentlatency) * 100);
            }
            else
            {
                result = (int)(((float)currentlatency / (float)lastLatency) * 100);
            }
            uniformitySum += result;
            string output = $"Равномерность печати {uniformitySum/counter}%";
            main.printing_uniformity.Content = output;
        }

        public void print_delay(int ElapsedMilliseconds)
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
            string output = $"Задержка печати: {ElapsedMilliseconds}ms";
            main.print_delay.Content = output;
        }

        public void avrPrintDelay()
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
            string avr_delay_output = $"Средняя задержка: {sum / counter}ms";
            main.avr_print_delay.Content = avr_delay_output;
        }
        #endregion
    }
}
