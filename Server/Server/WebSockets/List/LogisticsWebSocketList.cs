using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Server.WebSockets.List
{
    public class LogisticsWebSocketList
    {
        private static Dictionary<int, Sockets.LogisticsWebSocket> Sockets;

        private LogisticsWebSocketList()
        {

        }

        public static Dictionary<int, Sockets.LogisticsWebSocket> GetInstance()
        {
            if (Sockets == null) {
                Sockets = new Dictionary<int, Sockets.LogisticsWebSocket>();
            }
            return Sockets;
        }


        public static void BroadCast(String Message) {
            GetInstance();

            Byte[] MessageByte = Encoding.UTF8.GetBytes(Message);

            foreach (var Items in Sockets) {

                Items.Value.SendText(MessageByte, true);
            }
        
        }

    }
}