using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [Route("api/teammembers")]
    [Authorize]
    public class TeamMemberController : ApiController
    {
       
        // GET api/teammember/5
        public string Get(int id)
        {
            return "value";
        }



    }
}
