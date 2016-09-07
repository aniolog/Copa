using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;

namespace Server.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            string[] _messages = context.Exception.GetType().ToString().Split('.');
            string _message = (_messages.Length == 3) ? _messages[2] : _messages[1];
            context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                Content= new ObjectContent<String>(
                      _message,
                       new JsonMediaTypeFormatter(),"application/json"),
            };
            
        }

    }
}