using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/requets")]
    public class RequestsController : ApiController
    {
        // GET api/requests
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/requests/5
        public string Get(int id)
        {
            return "value";
        }

        [Filters.CrewMemberRegisterRequest]
        [Authorize(Roles = "crewmember")]
        [Route("crewmember")]
        [HttpPost]
        public void CrewMemberRegisterRequest([FromBody] Models.Request value) {
            var dos = 1 + 1;
        
        
        }
        













    }
}
