using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    interface IDictonary
    {
        public m3md2.LanguageDictonary LanguageDictonary { get; }
        public string[] Words { get; set; }
        public int AverageLettersInWords { get; }
    }
}
