//using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;

namespace WebSocketServer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder();
            //builder.WebHost.UseUrls("http://localhost:6666");
            //var app = builder.Build();
            var WebsocketOptions = new WebSocketOptions
            {

            };

            app.UseWebSockets();
            app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        while (true)
                        {
                            await webSocket.SendAsync(Encoding.ASCII.GetBytes($"Test - {DateTime.Now}"), WebSocketMessageType.Text, true, CancellationToken.None);
                            await Task.Delay(1000);
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
    }
}