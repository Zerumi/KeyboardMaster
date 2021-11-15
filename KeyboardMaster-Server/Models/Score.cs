﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyboardMaster_Server.Models
{
    public class Score
    {
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public nTextPerfomance textPerfomance { get; set; }
        public nCorePerfomance corePerfomance { get; set; }
    }
}
