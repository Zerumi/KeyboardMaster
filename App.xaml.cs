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
using System.Windows.Media;
using Gma.System.MouseKeyHook;

namespace KeyboardMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        static ICorePerfomance corePerfomance = new CorePerfomanceLogic();
        public static Cookie AuthCookie;
        public static bool isTraining = false;

        highlightingKeys highlightingKeys = new highlightingKeys();

        public static Stopwatch watch = new Stopwatch();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
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
            if (!isTraining)//Проверка, идет ли сейчас тренировка
            {

                if (watch.IsRunning)//Запущен ли секундомер для проверки задержки
                {
                    watch.Stop();

                    corePerfomance.print_delay((int)watch.ElapsedMilliseconds);

                    corePerfomance.bestLatency((int)watch.ElapsedMilliseconds);

                    CorePerfomanceLogic.sum += (int)watch.ElapsedMilliseconds;

                    corePerfomance.avrPrintDelay();

                    corePerfomance.printingUniformity((int)watch.ElapsedMilliseconds);


                    CorePerfomanceLogic.counter++;
                    CorePerfomanceLogic.lastLatency = (int)watch.ElapsedMilliseconds;
                    CorePerfomance.Latency = watch.ElapsedMilliseconds;
                    watch.Reset();
                    watch.Start();
                }
                else
                {
                    watch.Start();
                }
                charsPerMinute.chars.Add(e.KeyData.ToString());
                highlightingKeys.keyPressed(e.KeyData.ToString());
            }
            else
            {
                if (e.KeyCode.ToString() == TrainingModeLogic.currentChar.ToString())
                {
                    TrainingModeLogic.verification = true;
                    TrainingModeLogic.counter++;
                    TrainingModeLogic trainingModeLogic = new TrainingModeLogic(main.trainingMode);
                    trainingModeLogic.StartGenerating();
                }
                else
                {
                    TrainingModeLogic.verification = false;
                    TrainingModeLogic trainingModeLogic = new TrainingModeLogic(main.trainingMode);
                    TrainingModeLogic.timer.Stop();
                    trainingModeLogic.timer_tick(new object(), new EventArgs());
                }

            }
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
