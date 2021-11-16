using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    interface IDictonary
    {
        public string[] Words { get; set; }
        public int AverageLettersInWords { get; }
    }
}
