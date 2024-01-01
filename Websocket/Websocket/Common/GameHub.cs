using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Websocket.Common
{
    public class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var clientId = Context.ConnectionId;
            var clientIpAddress = Context.GetHttpContext().Connection.RemoteIpAddress;

            Console.WriteLine($"Client connected: ID={clientId}, IP={clientIpAddress}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var clientId = Context.ConnectionId;

            Console.WriteLine($"Client disconnected: ID={clientId}");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateParameters(ParameterChangeRequest parameter)
        {
            Debug.WriteLine("parameter: " + parameter.parameter);
            await Clients.All.SendAsync("ReceivedParameters", parameter);
        }

        public async Task UpdateScene(SceneChange sceneChange)
        {
            Debug.WriteLine("parameter: " + sceneChange.destinationScene);
            await Clients.All.SendAsync("ReceivedSceneChange", sceneChange);
        }
    }
}