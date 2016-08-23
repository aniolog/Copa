using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Server.Filters
{
    public class DelegateAceptedRequest : ActionFilterAttribute
    {
        //  Models.Complex Object =(Models.Complex) actionExecutedContext.ActionContext.ActionArguments["value"];

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}