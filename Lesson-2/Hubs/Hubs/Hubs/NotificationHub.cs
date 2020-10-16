using Common;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hubs.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        public Task SendMessage(Message message)
        {
            Debug.WriteLine(Context.ConnectionId);

            if (Context.Items.ContainsKey("user_name"))
                message.Title = $"Title {Context.Items["user_name"]}";

            return Clients.Others.Send(message);
        }

        public Task SetName(string name)
        {
            Context.Items.TryAdd("user_name", name);

            return Task.CompletedTask;
        }

        public override Task OnConnectedAsync()
        {
            var name = Context.Items.ContainsKey("user_name") ? Context.Items["user_name"] : string.Empty;
            var message = new Message
            {
                Title = $"new connection: { Context.ConnectionId } - { name }",
                Body = string.Empty
            };

            Clients.Others.Send(message);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var name = Context.Items.ContainsKey("user_name") ? Context.Items["user_name"] : string.Empty;
            var message = new Message
            {
                Title = $"disconnected: { Context.ConnectionId } - { name }",
                Body = string.Empty
            };

            Clients.Others.Send(message);

            return base.OnDisconnectedAsync(exception);
        }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine("Hub dispose");
            base.Dispose(disposing); 
        }
    }
}
