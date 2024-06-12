using System.Net.WebSockets;
using System.Text;

namespace AuctionApp.Data
{
    public class WebSocketHandler
    {
        private readonly List<WebSocket> _sockets = new List<WebSocket>();

        public void AddSocket(WebSocket socket)
        {
            _sockets.Add(socket);
        }

        public async Task BroadcastAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}