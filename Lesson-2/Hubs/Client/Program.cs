using Common;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static HubConnection hubConnection;
        static async Task Main(string[] args)
        {
            await InitSignalRConnaction();
            bool isExit = false;

            while(!isExit)
            {

                var input = Console.ReadLine();

                if(!string.IsNullOrWhiteSpace(input))
                {
                    if (input == "exit")
                        isExit = true;
                    else if (input == "setname")
                    {
                        Console.WriteLine("write name");
                        var name = "";

                        while(string.IsNullOrWhiteSpace(name))                        
                            name = Console.ReadLine();
                        
                        await hubConnection.SendAsync("SetName", name);


                    }
                    else
                    {
                        await hubConnection.SendAsync("SendMessage", new Message { Title = "Title", Body = input });
                        Console.WriteLine("Send");
                    }
                }
            }

            Console.ReadLine();
        }

        private static Task InitSignalRConnaction()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:50737/notify")
                .Build();

            hubConnection.On<Message>("Send", message => {
                Console.WriteLine($"Message server Title: {message.Title} /n Body: {message.Body}");
            });

            return hubConnection.StartAsync();
        }
    }
}
