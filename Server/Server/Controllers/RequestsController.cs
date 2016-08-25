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


        [Filters.CrewMemberRegisterRequest]
        [Authorize(Roles = "crewmember")]
        [Route("crewmember")]
        [HttpPost]
        public void CrewMemberRegisterRequest([FromBody] Models.Request value) {
            var dos = 1 + 1;
        
        
        }

        [Filters.CrewMemberModifiedRequest]
        [Authorize(Roles = "crewmember")]
        [Route("crewmember")]
        [HttpPut]
        public void CrewMemberModifiedRequest([FromBody] Models.Request value)
        {
            var dos = 1 + 1;


        }

        [Filters.CrewMemberAcceptedRequest]
        [Authorize(Roles = "crewmember")]
        [Route("crewmember/{RequestId}")]
        [HttpGet]
        public void CrewMemberAcceptedRequest([FromUri] long RequestId)
        {
            var dos = 1 + 1;


        }

        [Filters.CrewMemberRejectedRequest]
        [Authorize(Roles = "crewmember")]
        [Route("crewmember/{RequestId}")]
        [HttpDelete]
        public void CrewMemberDeletedRequest([FromUri] long RequestId)
        {
            var dos = 1 + 1;

        }


        [Filters.DelegateRegisterRequest]
        [Authorize(Roles = "logisticdelegate")]
        [Route("logisticdelegate")]
        [HttpPost]
        public void LogisticDelegateRegisterRequest([FromBody] Models.Request value)
        {
            var dos = 1 + 1;


        }


        [Filters.DelegateAceptedRequest]
        [Authorize(Roles = "logisticdelegate")]
        [Route("logisticdelegate/{RequestId}")]
        [HttpGet]
        public void LogisticDelegateAcceptedRequest([FromUri] long RequestId)
        {
            var dos = 1 + 1;


        }

        [Filters.DelegateRejectedRequest]
        [Authorize(Roles = "logisticdelegate")]
        [Route("logisticdelegate/{RequestId}")]
        [HttpDelete]
        public void LogisticDelegateDeletedRequest([FromUri] long RequestId)
        {
            var dos = 1 + 1;

        }











    }
}
