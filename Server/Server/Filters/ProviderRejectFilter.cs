using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Server.Filters
{
    public class ProviderRejectFilter:ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
          //  Models.Complex Object =(Models.Complex) actionExecutedContext.ActionContext.ActionArguments["value"];
        
        }
    }
}