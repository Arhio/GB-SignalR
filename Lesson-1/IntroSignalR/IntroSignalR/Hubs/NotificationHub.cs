using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroSignalR.Hubs
{
    public class NotificationHub : Hub
    {
        public Task SendMessage(string message) => Clients.Others.SendAsync("Send", message);
    }
}
