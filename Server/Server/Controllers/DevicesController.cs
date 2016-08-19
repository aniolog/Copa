using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/devices")]
    public class DevicesController : ApiController
    {

        // POST api/devices
        [HttpPost]
        [Route("")]
        public void Post([FromBody]Models.Device value)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            Logics.DeviceLogic _logic = new Logics.DeviceLogic();

            if (RequestContext.Principal.IsInRole("crewmember")) {
                _logic.AddDeviceToCrewMember(value,Id);
            }
            else if (RequestContext.Principal.IsInRole("logisticdelegate"))
            {
                _logic.AddDeviceToLogisticDelegate(value, Id);
            }
            else { 
            
            }
        }

        // DELETE api/devices/5
        [HttpDelete]
        [Route("{Token}")]
        public void Delete([FromUri] String Token)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            Logics.DeviceLogic _logic = new Logics.DeviceLogic();
            if (RequestContext.Principal.IsInRole("crewmember"))
            {
                _logic.DeleteDeviceFromCrewMember(Token, Id);
            }
            else if (RequestContext.Principal.IsInRole("logisticdelegate"))
            {
                _logic.DeleteDeviceFromLogisticDelegate(Token, Id);
            }
            else
            {

            }
        }
    }
}
