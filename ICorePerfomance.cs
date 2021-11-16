using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    interface ICorePerfomance
    {
        void bestLatency(int ElapsedMilliseconds);//Вычисление наименьшей задержки между нажатиями

        void printingUniformity(int ElapsedMilliseconds);//Вычисление равномерности ввода символов в процентах

        void print_delay(int ElapsedMilliseconds);//Ткущая задержка между символами

        void avrPrintDelay();//Средняя задержка между символами

        void CPM(int count);//Текущее количество символов в минуту

        void ACPM(int count);//Среднее количество символов в минуту

        void bestCPM(int count);//Наибольшее количество символов в минуту
    }
}
