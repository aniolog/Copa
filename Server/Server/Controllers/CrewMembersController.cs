using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/crewmembers")]
    public class CrewMembersController : ApiController
    {
        // GET api/crewmembers
        [HttpGet]
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/crewmembers/5
        [HttpGet]
        [Route("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/crewmembers
        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public void Post([FromBody]Models.CrewMember value)
        {
            Logics.CrewMemberLogic _logic = new Logics.CrewMemberLogic();
            _logic.AddCrewMember(value);
        }

        // PUT api/crewmembers/5
        [Authorize(Roles="crewmember")]
        [HttpPut]
        [Route("")]
        public void Put([FromBody]Models.CrewMember value)
        {
            int Id =int.Parse(RequestContext.Principal.Identity.Name);
            Logics.CrewMemberLogic _logic = new Logics.CrewMemberLogic();
            _logic.UpdateCrewMember(Id, value);
        }

        // GET api/crewmembers/confirm/
        [AllowAnonymous]
        [Route("confirm/{ConfirmationId}")]
        [HttpGet]
        public void Confirm([FromUri] String ConfirmationId)
        {
            Logics.CrewMemberLogic _logic = new Logics.CrewMemberLogic();
            _logic.ConfirmCrewMemberAccount(ConfirmationId);
        }

        // GET api/crewmembers/resetpassword/aniolog@gmail.com
        [AllowAnonymous]
        [Route("resetpassword/{Email}")]
        [HttpGet]
        public void RequestResetPassword([FromUri] String Email)
        {
            Logics.CrewMemberLogic _logic = new Logics.CrewMemberLogic();
            _logic.RequestResetPassword(Email);
        }

        // POST api/crewmembers/resetPassword/
        [AllowAnonymous]
        [Route("resetPassword/{ResetId}")]
        [HttpPost]
        public void ResetPassword([FromBody] Models.CrewMember ResetCrewMember, [FromUri] String ResetId)
        {
            Logics.CrewMemberLogic _logic = new Logics.CrewMemberLogic();
            _logic.ResetPassword(ResetCrewMember, ResetId);
        }
    }
}
