using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/logisticsdelegates")]
    public class LogisticsDelegatesController : ApiController
    {
        // GET api/logisticsdelegates
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/logisticsdelegates/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/logisticsdelegates
        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public void Post([FromBody]Models.LogisticsDelegate value)
        {
            Logics.LogisticsDelegateLogic _logic = new Logics.LogisticsDelegateLogic();
            _logic.AddLogisticDelegate(value);
        }

        // PUT api/logisticsdelegates/5
        [Authorize(Roles = "logisticsdelegate")]
        [HttpPut]
        [Route("")]
        public void Put([FromBody]Models.LogisticsDelegate value)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            Logics.LogisticsDelegateLogic _logic = new Logics.LogisticsDelegateLogic();
            _logic.UpdateLogisticDelegate(Id,value);

        }

        // GET api/logisticsdelegates/confirm/
        [AllowAnonymous]
        [Route("confirm/{ConfirmationId}")]
        [HttpGet]
        public void Confirm([FromUri] String ConfirmationId)
        {
            Logics.LogisticsDelegateLogic _logic = new Logics.LogisticsDelegateLogic();
            _logic.ConfirmLogisticDelegateAccount(ConfirmationId);
        }

        // GET api/logisticsdelegates/resetpassword/aniolog@gmail.com
        [AllowAnonymous]
        [Route("resetpassword/{Email}")]
        [HttpGet]
        public void RequestResetPassword([FromUri] String Email)
        {
            Logics.LogisticsDelegateLogic _logic = new Logics.LogisticsDelegateLogic();
            _logic.RequestResetPassword(Email);
        }

        // POST api/logisticsdelegates/resetPassword/
        [AllowAnonymous]
        [Route("resetPassword/{ResetId}")]
        [HttpPost]
        public void ResetPassword([FromBody] Models.LogisticsDelegate ResetDelegate, [FromUri] String ResetId)
        {
            Logics.LogisticsDelegateLogic _logic = new Logics.LogisticsDelegateLogic();
            _logic.ResetPassword(ResetDelegate,ResetId);
        }
    }
}
