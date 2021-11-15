using System;

namespace KeyboardMaster
{
    class TextPerfomance
    {
        public static int CorrectChars { get; set; }
        public static int IncorrectChars { get; set; }
        public static int IdealWords { get; set; }
        public static int ErrorWords { get; set; }
        public static double Accuracy => Math.Round(CorrectChars / (double)(CorrectChars + IncorrectChars) * 100, 2);
        public static int WordsPerMinute { get; set; }
    }
}
