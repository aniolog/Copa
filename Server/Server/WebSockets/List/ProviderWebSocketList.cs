using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Server.WebSockets.List
{
    public class ProviderWebSocketList
    {
        private static  Dictionary<int, List<Sockets.ProviderWebSocket>> Sockets;

        private ProviderWebSocketList()
        {

        }


        public static Dictionary<int, List<Sockets.ProviderWebSocket>> GetInstance()
        {
            if(Sockets==null){
                Sockets = new Dictionary<int, List<Sockets.ProviderWebSocket>>();
            }
            return Sockets;
        }


        public static void Send(int Key, String Message){
        
            Byte[] MessageByte = Encoding.UTF8.GetBytes(Message);

            List<Sockets.ProviderWebSocket> ProviderSockets = Sockets[Key];

            foreach (WebSockets.Sockets.ProviderWebSocket Socket in ProviderSockets)
            {

                Socket.SendText(MessageByte, true);
            }
        
        }
    }
}