using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Pushs
{
    public class PushFactory
    {
        public Push GetGcmPushSender() {
            return new GCM.GCMPush();
        }
    }
}