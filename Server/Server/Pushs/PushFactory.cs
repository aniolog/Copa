using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Pushs
{
    public class PushFactory
    {

        public static Push GetPushService(Boolean DeviceType) {

            if (!(DeviceType))
            {
                return new GCM.GCMPush();
            }
            return new Pushs.Ios.IosPush();
       
        
        }

        public static Push GetGcmPushSender() {
            return new GCM.GCMPush();
        }

        public static Push GetIosPushSender()
        {
            return new Pushs.Ios.IosPush();
        }
    }
}