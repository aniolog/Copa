using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
   // [Authorize(Roles = "logisticdelegate")]
    [RoutePrefix("api/vehicles")]
    public class VehiclesController : ApiController
    {


        [Route("types")]
        public List<Models.Vehicle.VehicleType> GetTypes(){
            Logics.VehicleLogic _logic = new Logics.VehicleLogic();
            return _logic.GetAllTypes();
        }
    }
}
