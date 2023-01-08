using System.Net.WebSockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace WebSocketServer
{
    public class Program
    {
        private static int PORT = 7777;
        
        public static async Task Main(string[] args)
        {
            using var gpio = new GpioController();

            var keypad = new Keypad(gpio);

            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();

            app.Urls.Add("http://*:" + PORT);

            app.UseWebSockets();

            app.Map("/", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        keypad.WebSocket = webSocket;
                        keypad.OnKeypadButtonPress += Keypad_OnKeypadButtonPress;
                        while (true)
                        {
                            await webSocket.SendAsync(Encoding.ASCII.GetBytes("{ event: 'keepalive', data: '" + DateTime.Now + "' }"), WebSocketMessageType.Binary, true, CancellationToken.None);
                            await Task.Delay(15000);
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            });

            await app.RunAsync();
        }

        private async static void Keypad_OnKeypadButtonPress(object sender, KeypadEventArgs args)
        {
            var keypad = (Keypad)sender;
            await keypad.WebSocket.SendAsync(Encoding.ASCII.GetBytes(args.Json), WebSocketMessageType.Binary, true, CancellationToken.None);
        }
    }
}