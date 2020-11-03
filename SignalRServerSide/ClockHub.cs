using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.SignalRServerSide
{
    public class ClockHub : Hub<IClock>
    {
        public async Task SendTimeToClients(DateTime dateTime)
        {
            await Clients.All.ShowTime(dateTime);
        }
    }
}