namespace KeyboardMaster_Server.Models
{
    public class nCorePerfomance
    {
        public int CharsPerMinute { get; set; }
        public int BestCPM { get; set; }
        public int AverageCPM { get; set; }
        public long BestLatency { get; set; }
        public long Latency { get; set; }
        public int AverageDelay { get; set; }
    }
}