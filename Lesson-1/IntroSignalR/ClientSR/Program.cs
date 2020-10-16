using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSR
{
    class Program
    {
        static HubConnection HubConnection;
        static async Task Main(string[] args)
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:50288/nitify")
                .Build();

            HubConnection.On<string>("Send", message => Console.WriteLine($"Message: {message}"));

            await HubConnection.StartAsync();

            bool isExit = false;

            while(!isExit)
            {
                var message = Console.ReadLine();

                if (message != "exit")
                    await HubConnection.SendAsync("SendMessage", message);
                else
                    isExit = true;
            }

            Console.ReadLine();
        }
    }
}
