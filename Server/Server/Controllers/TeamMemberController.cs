using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [RoutePrefix("api/teammembers")]
    [Authorize]
    public class TeamMemberController : ApiController
    {

        [Authorize(Roles = "crewmember")]
        [HttpPut]
        [Route("{Id}")]
        public void Put([FromUri] int Id,[FromBody]Models.TeamMember value)
        {
            Logics.TeamMemberLogic _logic = new Logics.TeamMemberLogic();
            _logic.UpdateTeamMember(Id, value);
        }



    }
}
