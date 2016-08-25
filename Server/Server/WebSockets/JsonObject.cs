using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.WebSockets
{
    public class JsonObject
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ServerEvent Event { set; get; }

        public String TriggerBy { set; get; }

        public String Data { set; get; }


        public enum ServerEvent 
        {
            CrewMemberAcceptedRequest,
            CrewMemberRejectedRequest,
            CrewMemberModifiedRequest,
            CrewMemberRegistedRequest,
            DelegateAcceptedRequest,
            DelegateModifiedRequest,
            DelegateRegisterRequest,
            DelegateRejectedRequest,
            TeamMemberCanceldByInactivity,
            RequestCancelByInactivity
        }


    }
}