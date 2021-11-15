using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyboardMaster_Server.Models
{
    public class Score
    {
        public string Name { get; set; }
        public TextPerfomance textPerfomance { get; set; }
        public CorePerfomance corePerfomance { get; set; }
    }
}
