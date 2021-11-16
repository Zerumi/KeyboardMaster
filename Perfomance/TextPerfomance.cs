using System;

namespace KeyboardMaster
{
    public class TextPerfomance
    {
        public static int CorrectChars { get; set; }
        public static int IncorrectChars { get; set; }
        public static double Accuracy => Math.Round(CorrectChars / (double)(CorrectChars + IncorrectChars) * 100, 2);
        public static int IdealWords { get; set; }
        public static int ErrorWords { get; set; }
        public static int WrongWords { get; set; }
        public static int WordsPerMinute { get; set; }
        public static int AverageWPM { get; set; }
        public static int StreakIdealWords { get; set; }
        public static double WordAccuracy => Math.Round(IdealWords / (double)WordsPerMinute * 100, 2);
        public static double TextPerfomancePoints => Accuracy * WordAccuracy * IdealWords / 10000;
    }
}
