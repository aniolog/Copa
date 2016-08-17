using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Rest
{
    public abstract class RestRequest
    {
        public string ApiKey { get; set; }

        public string BaseUrl { get; set; }

    }
}