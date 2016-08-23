using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [RoutePrefix("api/vehicles")]
    public class VehiclesController : ApiController
    {
        // GET api/vehicles
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/vehicles/5
        public string Get(int id)
        {
            return "value";
        }

        [Route("")]
        public List<String> GetTypes(){

            return Enum.GetNames(typeof(Models.Vehicle.VehicleType)).ToList();
            
        }
    }
}
