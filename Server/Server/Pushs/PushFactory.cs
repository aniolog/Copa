using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Pushs
{
    public class PushFactory
    {
        public static Push GetGcmPushSender() {
            return new GCM.GCMPush();
        }

        public static Push GetIosPushSender()
        {
            return new Pushs.Ios.IosPush();
        }
    }
}