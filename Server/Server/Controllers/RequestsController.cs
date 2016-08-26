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


        [Authorize(Roles = "crewmember")]
        [Route("crewmember")]
        [HttpPost]
        public void CrewMemberRegisterRequest([FromBody] Models.Request value) {
            long Id = long.Parse(RequestContext.Principal.Identity.Name);
            Logics.RequestLogic _logic = new Logics.RequestLogic();
            _logic.CrewMemberRegisterRequest(Id,value);
        
        
        }

        [Authorize(Roles = "crewmember")]
        [Route("crewmember/{TeamMemberId}")]
        [HttpPut]
        public void CrewMemberModifiedRequest([FromUri] long TeamMemberId,
            [FromBody] Models.TeamMember value)
        {
            long Id = long.Parse(RequestContext.Principal.Identity.Name);
            Logics.RequestLogic _logic = new Logics.RequestLogic();
            _logic.CrewMemberModifyRequest(TeamMemberId, Id, value);


        }

        [Authorize(Roles = "crewmember")]
        [Route("crewmember/{TeamMemberId}")]
        [HttpGet]
        public void CrewMemberAcceptedRequest([FromUri] long TeamMemberId)
        {
            var dos = 1 + 1;


        }

        [Authorize(Roles = "crewmember")]
        [Route("crewmember/{TeamMemberId}")]
        [HttpDelete]
        public void CrewMemberCancelRequest([FromUri] long TeamMemberId)
        {
            var dos = 1 + 1;

        }

        [Authorize(Roles = "logisticdelegate")]
        [Route("logisticdelegate")]
        [HttpPost]
        public void LogisticDelegateRegisterRequest([FromBody] Models.Request value)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            Logics.RequestLogic _logic = new Logics.RequestLogic();
            _logic.DelegateRegisterRequest(Id, value);
        


        }

        [Authorize(Roles = "logisticdelegate")]
        [Route("logisticdelegate/{RequestId}/{ProviderId}")]
        [HttpGet]
        public void LogisticDelegateAcceptedRequest([FromUri] long RequestId,
            [FromUri] long ProviderId)
        {
            long Id = long.Parse(RequestContext.Principal.Identity.Name);
            Logics.RequestLogic _logic = new Logics.RequestLogic();
            _logic.DelegateAcceptRequest(RequestId, Id,ProviderId);
        }


        [Authorize(Roles = "logisticdelegate")]
        [Route("logisticdelegate/{RequestId}")]
        [HttpDelete]
        public void LogisticDelegateCancelRequest([FromUri] long RequestId)
        {
            long Id = long.Parse(RequestContext.Principal.Identity.Name);
            Logics.RequestLogic _logic = new Logics.RequestLogic();
            _logic.DelegateRejectRequest(RequestId, Id);

        }











    }
}
