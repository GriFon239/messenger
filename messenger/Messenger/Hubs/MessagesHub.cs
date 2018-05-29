using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Hubs
{
    [Authorize]
    public class MessagesHub : Hub
    {
        private static ConcurrentDictionary<string, List<string>> connections = 
            new ConcurrentDictionary<string, List<string>>();

        public override Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(userName))
            {
                if (connections.ContainsKey(userName))
                {
                    connections[userName].Add(Context.ConnectionId);
                }
                else
                {
                    connections.TryAdd(userName, new List<string> { Context.ConnectionId });
                }
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(userName) && connections.ContainsKey(userName))
                connections[userName].Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public Task SendMessage(string message, string user)
        {
            if (!connections.ContainsKey(user))
                return Task.FromResult<object>(null);

            return Clients.Clients(connections[user])
                .SendAsync(
                    "NotifyNewMessage", 
                    new
                    {
                        from = Context.User.Identity.Name,
                        message = message
                    });
        }
    }
}