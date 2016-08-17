using Owin.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.WebSockets.Sockets
{
    public class LogisticsWebSocket : WebSocketConnection
    {

        public override void OnOpen()
        {
            int LogisticId=int.Parse(this.Arguments["Id"]);

            Dictionary<int, Sockets.LogisticsWebSocket> Sockets=
                List.LogisticsWebSocketList.GetInstance();

            if (Sockets.ContainsKey(LogisticId)) {
                Sockets[LogisticId] = this;
            }
            else
            {
                Sockets.Add(LogisticId, this);
            }
        }

        public override void OnClose(System.Net.WebSockets.WebSocketCloseStatus? closeStatus, string closeStatusDescription)
        {
            int LogisticId = int.Parse(this.Arguments["Id"]);

            Dictionary<int, Sockets.LogisticsWebSocket> Sockets =
                List.LogisticsWebSocketList.GetInstance();

            Sockets.Remove(LogisticId);
           
        }

    }
}