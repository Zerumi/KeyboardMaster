using System;

namespace KeyboardMaster
{
    internal class Score
    {
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public nTextPerfomance textPerfomance { get; set; }
        public nCorePerfomance corePerfomance { get; set; }
    }
}