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
            corePerfomance.CPM(chars.Count);
            corePerfomance.ACPM(chars.Count);
            corePerfomance.bestCPM(chars.Count);

            chars = new List<string>();
            timer.Start();
        }
    }
}
