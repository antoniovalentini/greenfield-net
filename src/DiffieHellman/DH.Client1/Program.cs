using System;
using System.Threading;
using System.Threading.Tasks;
using DH.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace DH.Client1
{
    internal class Program
    {
        private const string Username = "Client1";
        private static bool _cancelled;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Client1!");
            Console.CancelKeyPress += (sender, e) =>
            {
                _cancelled = true;
                e.Cancel = true;
            };

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44399/chatHub")
                .Build();

            await connection.StartAsync();

            var me = new UserEndpoint(Username);
            connection.On("ExchangeKey",
                (string user, byte[] publickey) =>
                {
                    if (string.Equals(user, Username, StringComparison.CurrentCultureIgnoreCase)) return;
                    me.CalculateExchangeKey(publickey);
                    Console.WriteLine($"Key received from: {user}");
                });
            connection.On("ReceiveMessage",
                (string user, byte[] encMessage, byte[] iv) =>
                {
                    if (string.Equals(user, Username, StringComparison.CurrentCultureIgnoreCase)) return;
                    var message = me.Receive(encMessage, iv);
                    Console.WriteLine($"{user}: {message}");
                });

            Thread.Sleep(2000);
            await connection.InvokeAsync("ExchangeKey", Username, me.PublicKey);
            
            while (!_cancelled)
            {
                var mes = Console.ReadLine();
                if (string.IsNullOrEmpty(mes)) continue;
                if (!me.KeyExchanged)
                    await connection.InvokeAsync("ExchangeKey", Username, me.PublicKey);
                me.Send(mes, out var encryptedMessage, out var iv);
                await connection.InvokeAsync("SendMessage", Username, encryptedMessage, iv);
            }
        }
    }
}
