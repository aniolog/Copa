using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Routing;

namespace Server.Filters
{
    public class CrewMemberRegisterRequest : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Models.Request Object = 
                (Models.Request)actionExecutedContext.ActionContext.ActionArguments["value"];
            JsonObject _jsonMessage=new JsonObject();
            _jsonMessage.Event =
                Server.Filters.JsonObject.ServerEvent.CrewMemberRegistedRequest;
            _jsonMessage.TriggerBy = "yo";

            WebSockets.List.LogisticsWebSocketList.
                BroadCast(JsonConvert.SerializeObject(_jsonMessage));
            



        }
    }
}