using System.Net.WebSockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Device.Gpio;

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

            //builder.Urls.Add("http://*:3000");

            var app = builder.Build();

            app.Urls.Add("http://*:" + PORT);

            app.UseWebSockets();

            app.Map("/", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        while (true)
                        {
                            //await webSocket.SendAsync(Encoding.ASCII.GetBytes($"Test - {DateTime.Now}"), WebSocketMessageType.Text, true, CancellationToken.None);
                            await webSocket.SendAsync(Encoding.ASCII.GetBytes($"Test - {DateTime.Now}"), WebSocketMessageType.Binary, true, CancellationToken.None);
                            await Task.Delay(1000);
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            });


            //app.MapGet("/", () => "Hello World!");

            await app.RunAsync();
        }
    }
}