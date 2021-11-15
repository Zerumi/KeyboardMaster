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
        public static List<string> chars = new List<string>();
        public static DispatcherTimer timer = new DispatcherTimer();
        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
    public void Activate()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Start();
        }
        private void timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            CorePerfomance.CharsPerMinute = chars.Count * 20;
            string output = $"Символов в минуту: {chars.Count * 20}";
            main.CPM.Content = output;
            chars = new List<string>();
            timer.Start();
        }
    }
}
