using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Pushs.Ios
{
    public class IosPush:Push
    {


        public override void Send(string Title, string Message, string Data)
        {
            throw new NotImplementedException();
        }

        public override void AddToken(string Token)
        {
            throw new NotImplementedException();
        }
    }
}