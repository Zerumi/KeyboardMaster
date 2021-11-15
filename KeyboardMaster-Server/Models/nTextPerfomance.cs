namespace KeyboardMaster_Server.Models
{
    public class nTextPerfomance
    {
        public int CorrectChars { get; set; }
        public int IncorrectChars { get; set; }
        public int IdealWords { get; set; }
        public int ErrorWords { get; set; }
        public double Accuracy { get; set; }
        public int WordsPerMinute { get; set; }
        public int WrongWords { get; set; }
    }
}