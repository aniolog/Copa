using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Pushs.Ios
{
    public class ApnMessage
    {
        public String alert { set; get; }

        public String message { set; get; }

        public String data { set; get; }
    }


}