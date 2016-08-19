using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/device")]
    public class DevicesController : ApiController
    {

        // POST api/devices
        [HttpPost]
        [Route("")]
        public void Post([FromBody]Models.Device value)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            if (RequestContext.Principal.IsInRole("crewmember")) {
               
            }
            else if (RequestContext.Principal.IsInRole("logisticdelegate"))
            {

            }
            else { 
            
            }
        }

        // DELETE api/devices/5
        [HttpDelete]
        [Route("{Token}")]
        public void Delete([FromUri] String Token)
        {
        }
    }
}
