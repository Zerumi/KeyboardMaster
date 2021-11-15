using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    class nTextPerfomance
    {
        public int CorrectChars { get; set; }
        public int IncorrectChars { get; set; }
        public int IdealWords { get; set; }
        public int ErrorWords { get; set; }
        public double Accuracy { get; set; }
        public int WordsPerMinute { get; set; }
    }
}
