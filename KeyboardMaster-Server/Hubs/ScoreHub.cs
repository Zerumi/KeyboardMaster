using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using KeyboardMaster_Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace KeyboardMaster_Server.Hubs
{
    [Authorize]
    public class ScoreHub : Hub
    {
        static List<Score> scores = new List<Score>();

        public async Task SubmitScore(Score score)
        {
            scores.Add(score);
            await Clients.All.SendAsync("NewScore", score);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("GetScoreTable", scores);
            await base.OnConnectedAsync();
        }
    }
}
