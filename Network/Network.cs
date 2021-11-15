using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace KeyboardMaster
{
    public static class Network
    {
        public static HubConnection ScoreConnection = new HubConnectionBuilder().WithUrl($"{m3md2.StaticVariables.BaseServerAddress}score", options =>
        {
            options.UseDefaultCredentials = true;
            options.Headers.Add("User-Agent", "Mozilla/5.0");
            options.Cookies.Add(App.AuthCookie);
        }).Build();

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
                        WordsPerMinute = TextPerfomance.WordsPerMinute
                    },
                    corePerfomance = new nCorePerfomance()
                    {
                        CharsPerMinute = CorePerfomance.CharsPerMinute
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

        public static Client AuthUser(string username, out Cookie cookie) // метод для авторизации на сервере
        {
            Client returnproduct = default;
            try
            {
                CookieContainer cookies = new CookieContainer(); // авторизуем, отправляя Post запрос с нужными параметрами
                HttpClientHandler handler = new HttpClientHandler
                {
                    CookieContainer = cookies
                };
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(m3md2.StaticVariables.BaseServerAddress)
                };
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
                string json = JsonConvert.SerializeObject(username);
                HttpResponseMessage response = client.PostAsync($"auth", new StringContent(json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                returnproduct = response.Content.ReadAsAsync<Client>().GetAwaiter().GetResult();
                try // Получаем куки
                {
                    Uri uri = new Uri($"{m3md2.StaticVariables.BaseServerAddress}auth");
                    var collection = cookies.GetCookies(uri);
                    cookie = collection[".AspNetCore.Cookies"];
                }
                catch (Exception ex)
                {
                    ExceptionHandler.RegisterNew(ex);
                    cookie = default;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
                cookie = default;
            }
            return returnproduct;
        }
    }
}