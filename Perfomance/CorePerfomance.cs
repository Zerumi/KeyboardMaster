using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    public class CorePerfomance
    {
        public static int CharsPerMinute { get; set; }
        public static int BestCPM { get; set; }
        public static int AverageCPM { get; set; }
        public static long BestLatency { get; set; }
        public static long Latency { get; set; }
        public static int AverageDelay { get; set; }
    }
}
