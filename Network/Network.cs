using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace KeyboardMaster
{
    public class Network
    {
        public HubConnection ScoreConnection = new HubConnectionBuilder().WithUrl($"{m3md2.StaticVariables.BaseServerAddress}score", options =>
        {
            options.UseDefaultCredentials = true;
            options.Headers.Add("User-Agent", "Mozilla/5.0");
            options.Cookies.Add(m3md2.StaticVariables.AuthCookie);
        }).Build();

        internal async void SubmitScore()
        {
            try
            {
                Score score = new Score()
                {
                    Name = Environment.UserName,
                    Timestamp = DateTime.Now,
                    textPerfomance = new nTextPerfomance()
                    {
                        Accuracy = TextPerfomance.Accuracy,
                        CorrectChars = TextPerfomance.CorrectChars,
                        ErrorWords = TextPerfomance.ErrorWords,
                        IdealWords = TextPerfomance.IdealWords,
                        IncorrectChars = TextPerfomance.IncorrectChars,
                        WordsPerMinute = TextPerfomance.WordsPerMinute,
                        WrongWords = TextPerfomance.WrongWords
                    },
                    corePerfomance = new nCorePerfomance()
                    {
                        CharsPerMinute = CorePerfomance.CharsPerMinute,
                        AverageCPM = CorePerfomance.AverageCPM,
                        AverageDelay = CorePerfomance.AverageDelay,
                        BestCPM = CorePerfomance.BestCPM,
                        BestLatency = CorePerfomance.BestLatency,
                        Latency = CorePerfomance.Latency
                    }
                };
                await ScoreConnection.StartAsync();
                await ScoreConnection.InvokeAsync("SubmitScore", score);
                await ScoreConnection.StopAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }
    }
}