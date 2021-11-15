using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace KeyboardMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static Cookie AuthCookie;
        public static int best_latency = int.MaxValue;
        public static int sum = 0;
        public static int counter = 1;

        highlightingKeys highlightingKeys = new highlightingKeys();

        public static Stopwatch watch = new Stopwatch();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (m_globalHook == null)//Оформляем событие по вытягиванию нажатых клавиш
            {
                m_globalHook = Hook.GlobalEvents();
                m_globalHook.KeyDown += GlobalHookKeyDown;
                m_globalHook.KeyUp += GlobalHookKeyUp;
            }
        }
        private IKeyboardMouseEvents m_globalHook;
        private void GlobalHookKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)//Обработчик собыьтия по вытягиванию нажатых клавиш
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (watch.IsRunning)//Запущен ли секундомер для проверки задержки
            {
                watch.Stop();
                string output = $"Задержка печати: {watch.ElapsedMilliseconds}ms";
                if (best_latency>watch.ElapsedMilliseconds && watch.ElapsedMilliseconds!=0) //Лучшая задержка
                {
                    best_latency = (int)watch.ElapsedMilliseconds;
                    string latency_record = $"Лучшая задержка: {best_latency}ms";
                    main.best_latency.Content = latency_record;
                }
                main.print_delay.Content = output;
                sum += (int)watch.ElapsedMilliseconds;
                string avr_delay_output = $"Средняя задержка: {sum/counter}ms";
                main.avr_print_delay.Content = avr_delay_output;
                counter++;
                watch.Reset();
                watch.Start();
            }
            else
            {
                watch.Start();
            }
            charsPerMinute.chars.Add( e.KeyData.ToString());
            highlightingKeys.keyPressed(e.KeyData.ToString());
        }
        private void GlobalHookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)//Обработчик собыьтия по вытягиванию нажатых клавиш
        {
            highlightingKeys.keyUpped(e.KeyData.ToString());
        }


        private void GlobalHookKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //Действие при вызове события
            // если зажали клавишу
        }

    }
}
