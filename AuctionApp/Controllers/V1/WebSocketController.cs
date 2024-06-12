using System.Net.WebSockets;
using AuctionApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketHandler _webSocketHandler;

        public WebSocketController(WebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        //[HttpGet("/ws")]
        //public async Task Get()
        //{
        //    if (HttpContext.WebSockets.IsWebSocketRequest)
        //    {
        //        var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        //        _webSocketHandler.AddSocket(webSocket);

        //        await Echo(webSocket);
        //    }
        //    else
        //    {
        //        HttpContext.Response.StatusCode = 400;
        //    }
        //}

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}