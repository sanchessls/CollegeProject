using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ScrumPokerPlanning.SignalRServerSide
{
    public class FeatureHub : Hub<IFeature>
    {
        private static readonly Dictionary<string, string> connectionsNgroup = new Dictionary<string, string>();

         public async Task JoinGroup(string group)
        {
            if (connectionsNgroup.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connectionsNgroup[Context.ConnectionId]);
                connectionsNgroup.Remove(Context.ConnectionId);
            }
            connectionsNgroup.Add(Context.ConnectionId, group);
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
    }
}