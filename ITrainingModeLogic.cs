using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    interface ITrainingModeLogic
    {
        public void Start(int levelspeed);
        void timer_tick(object sender, EventArgs e);

    }
}
