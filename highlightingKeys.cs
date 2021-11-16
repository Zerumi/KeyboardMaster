using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using m3md2;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;

namespace KeyboardMaster
{
    class highlightingKeys
    {
        Rectangle rect = new Rectangle();
        public void keyPressed(string key)
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;

            rect = m3md2.WinHelper.FindChild<Rectangle>(main.gKeyboard, key);

            if (rect == null)
            {
                key = key.Split(", ").First();
                if (key == "Alt")
                {
                    key = "LMenu";
                }
                rect = m3md2.WinHelper.FindChild<Rectangle>(main.gKeyboard, key);
                rect.Fill = Brushes.LightGreen;
            }
            else
            {
                rect.Fill = Brushes.LightGreen;
            }
        }

        public void keyUpped(string key)
        {
            MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;

            rect = m3md2.WinHelper.FindChild<Rectangle>(main.gKeyboard, key);

            if (rect == null)
            {
                key = key.Split(", ").First();
                if (key == "Alt")
                {
                    key = "LMenu";
                }
                rect = m3md2.WinHelper.FindChild<Rectangle>(main.gKeyboard, key);
                rect.Fill = null;
            }
            else
            {
                rect.Fill = null;
            }
        }
    }
}
