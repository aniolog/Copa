using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Pushs
{
    public class PushFactory
    {
        public static Push GetPushSender() {

            return new GCM.GCMPush();
        }
    }
}