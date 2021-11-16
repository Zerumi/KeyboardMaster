using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using m3md2;
using System.Threading.Tasks;

namespace KeyboardMaster
{
    public class Network
    {
        public static HubConnection ScoreConnection = new HubConnectionBuilder().WithUrl($"{m3md2.StaticVariables.BaseServerAddress}score", options =>
        {
            options.UseDefaultCredentials = true;
            options.Headers.Add("User-Agent", "Mozilla/5.0");
            options.Cookies.Add(m3md2.StaticVariables.AuthCookie);
        }).WithAutomaticReconnect()
            .Build();

        public static List<Score> scores = new List<Score>();

        internal static async Task ConfigureConnection()
        {
            _ = ScoreConnection.On("GetScoreTable", new Action<List<Score>>(x => 
            {
                scores = x;
                OnScoresUpdate?.Invoke();
            }));

            _ = ScoreConnection.On("NewScore", new Action<Score>(x =>
            {
                scores.Add(x);
                OnScoreAdd?.Invoke(x);
            }));

            await ScoreConnection.StartAsync();
        }

        public static event Action OnScoresUpdate;
        public static event Action<Score> OnScoreAdd;

        internal static async void SubmitScore()
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
                        WrongWords = TextPerfomance.WrongWords,
                        AverageWPM = TextPerfomance.AverageWPM,
                        StreakIdealWords = TextPerfomance.StreakIdealWords,
                        WordAccuracy = TextPerfomance.WordAccuracy,
                        TextPerfomancePoints = TextPerfomance.TextPerfomancePoints
                    },
                    corePerfomance = new nCorePerfomance()
                    {
                        CharsPerMinute = CorePerfomance.CharsPerMinute,
                        AverageCPM = CorePerfomance.AverageCPM,
                        AverageDelay = CorePerfomance.AverageDelay,
                        BestCPM = CorePerfomance.BestCPM,
                        BestLatency = CorePerfomance.BestLatency,
                        Latency = CorePerfomance.Latency,
                        printingUniformity = CorePerfomance.printingUniformity,
                        CorePerfomancePoints = CorePerfomance.CorePerfomancePoints
                    }
                };
                await ScoreConnection.InvokeAsync("SubmitScore", score);
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }
    }
}