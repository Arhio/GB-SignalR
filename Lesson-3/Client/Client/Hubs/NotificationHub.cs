using Common;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        public Task SendMessage(Message message)
        {
            Debug.WriteLine(Context.ConnectionId);
            var name = Context.Items["user_name"] as string;
            var messageClient = new ClientMessage { NameClient = string.IsNullOrWhiteSpace(name) ? "Anonim" : name,  Title = message.Title, Body = message.Body  };
            return Clients.Others.Send(messageClient);
        }

        public Task SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Task.CompletedTask;

            if (Context.Items.ContainsKey("user_name"))
                Context.Items["user_name"] = name;
            else
                Context.Items.TryAdd("user_name", name);

            return Task.CompletedTask;
        }

        public Task<string> GetName()
        {
            if (Context.Items.ContainsKey("user_name"))
                return Task.FromResult((string)Context.Items["user_name"]);
            return Task.FromResult("Anonim");
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
