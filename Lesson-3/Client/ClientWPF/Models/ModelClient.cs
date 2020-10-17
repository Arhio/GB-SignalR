using Common;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ClientWPF.Models
{
    class ClientEventArgs : EventArgs
    {
    }

    public class ModelClient
    {
        private readonly HubConnection hubConnection;

        public string _messageBox = "";
        public string MessagesBox { get; set; } = "";
        public event EventHandler eventMessageBox;


        public string StateConnection { get; set; } = "";
        public SolidColorBrush StateConnectionColor { get; set; } = Brushes.Red;
        public event EventHandler eventStateConnection;

        public string NameMethodConnect { get; set; } = "Connect";
        public event EventHandler eventNameMethodConnect;

        public string BodyMessage { get; set; } = "";
        public event EventHandler eventBodyMessage;

        public string NameClient { get; set; } = "";
        public event EventHandler eventNameClient;

        public string Title { get; set; } = "SimpleText";
        public event EventHandler eventTitle;

        public string NameSetClient { get; set; } = "";
        public event EventHandler eventNameSetClient;


        public ModelClient()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:52610/messages")
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<ClientMessage>("Send", message =>
            {
                AppendMessage(message).Wait();
                return Task.CompletedTask;
            });
            hubConnection.Closed += error => 
            { 
                HubClosed(error);
                return Task.CompletedTask;
            };
            hubConnection.Reconnected += id => 
            { 
                HubReconnected(id);
                return Task.CompletedTask;
            };
            hubConnection.Reconnecting += error => 
            { 
                HubReconnecting(error);
                return Task.CompletedTask;
            };
        }

        public async Task AppendMessage(ClientMessage message)
        {
            MessagesBox += $"{message.NameClient}: {message.Title} - {message.Body} {Environment.NewLine}";
            eventMessageBox(this, new ClientEventArgs());
        }

        public async Task HubClosed(Exception error)
        {
            MessagesBox += $"Connection closed. {error}{Environment.NewLine}";
            eventMessageBox(this, new ClientEventArgs());

            StateConnectionColor = Brushes.Red;
            StateConnection = hubConnection.State.ToString();
            eventStateConnection(this, new ClientEventArgs());
        }

        public async Task HubReconnected(string id)
        {
            MessagesBox += $"Reconnected whith id: {id}{Environment.NewLine}";
            eventMessageBox(this, new ClientEventArgs());

            StateConnectionColor = Brushes.Green;
            StateConnection = hubConnection.State.ToString();
            eventStateConnection(this, new ClientEventArgs());
        }

        public async Task HubReconnecting(Exception error)
        {
            MessagesBox += $"Reconecting: {error}{Environment.NewLine}";
            eventMessageBox(this, new ClientEventArgs());

            StateConnectionColor = Brushes.Yellow;
            StateConnection = hubConnection.State.ToString();
            eventStateConnection(this, new ClientEventArgs());
        }

        public async Task Connecting()
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await hubConnection.StartAsync();
                }
                catch (Exception e)
                {
                    MessagesBox += e;
                    eventMessageBox(this, new ClientEventArgs());
                }

                if (hubConnection.State == HubConnectionState.Connected)
                {
                    StateConnectionColor = Brushes.Green;
                    StateConnection = hubConnection.State.ToString();
                    eventStateConnection(this, new ClientEventArgs());

                    NameMethodConnect = $"Disconnect{Environment.NewLine}";
                    eventNameMethodConnect(this, new ClientEventArgs());

                    MessagesBox += $"Conected{Environment.NewLine}";
                    eventMessageBox(this, new ClientEventArgs());
                }
            }
            else if (hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.StopAsync();

                StateConnectionColor = Brushes.Red;
                StateConnection = hubConnection.State.ToString();
                eventStateConnection(this, new ClientEventArgs());

                NameMethodConnect = $"Connect{Environment.NewLine}";
                eventNameMethodConnect(this, new ClientEventArgs());
            }
        }

        public async Task SendMessage()
        {
            if(hubConnection.State == HubConnectionState.Connected && BodyMessage != "")
            {
                var message = new ClientMessage
                {
                    NameClient = string.IsNullOrWhiteSpace(NameClient) ? $"Ananim.{hubConnection.ConnectionId}" : NameClient,
                    Title  = Title,
                    Body = BodyMessage
                };

                try
                {
                    hubConnection.SendAsync("SendMessage", message);
                    MessagesBox += $"You: {message.Title} - {message.Body} {Environment.NewLine}";
                    eventMessageBox(this, new ClientEventArgs());
                    BodyMessage = "";
                    //eventBodyMessage(this, new ClientEventArgs());
                }
                catch (Exception e)
                {
                    MessagesBox += e;
                    eventMessageBox(this, new ClientEventArgs());
                }
            }
        }

        public async Task SetName()
        {
            if (hubConnection.State == HubConnectionState.Connected && NameSetClient != "")
            {
                try
                {
                    hubConnection.SendAsync("SetName", NameSetClient);
                    MessagesBox += $"Yuor name change: {NameSetClient} {Environment.NewLine}";
                    eventMessageBox(this, new ClientEventArgs());
                    NameClient = NameSetClient;
                    eventNameClient(this, new ClientEventArgs());
                }
                catch (Exception e)
                {
                    MessagesBox += e;
                    eventMessageBox(this, new ClientEventArgs());
                }
            }
        }

        public async Task GetName()
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                try
                {
                    var name = await hubConnection.InvokeAsync<string>("GetName");

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        NameClient = NameSetClient = "Amonim";                        
                    }
                    else
                    {
                        NameClient = NameSetClient = name;
                    }

                    eventNameClient(this, new ClientEventArgs());
                    eventNameSetClient(this, new ClientEventArgs());
                }
                catch (Exception e)
                {
                    MessagesBox += e;
                    eventMessageBox(this, new ClientEventArgs());
                }
            }
        }
    }
}
