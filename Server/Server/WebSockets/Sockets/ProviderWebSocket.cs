using Owin.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Web;

namespace Server.WebSockets.Sockets
{
    public class ProviderWebSocket : WebSocketConnection
    {
        private String Uid;

        public override void OnOpen()
        {
           int ProviderId = int.Parse(this.Arguments["Id"]);

           Dictionary<int, List<Sockets.ProviderWebSocket>> Sockets =
                List.ProviderWebSocketList.GetInstance();



           if (!(Sockets.ContainsKey(ProviderId))) {
               Sockets[ProviderId] = new List<ProviderWebSocket>();
           }

            Sockets[ProviderId].Add(this);


        }


        public override void OnClose(System.Net.WebSockets.WebSocketCloseStatus? closeStatus, string closeStatusDescription)
        {
            int ProviderId = int.Parse(this.Arguments["Id"]);

            Dictionary<int, List<Sockets.ProviderWebSocket>> Sockets =
                 List.ProviderWebSocketList.GetInstance();

            Sockets[ProviderId].Remove(this);

            if(Sockets[ProviderId].Count==0){
                Sockets.Remove(ProviderId);

            }
           
        }
        
    }
}