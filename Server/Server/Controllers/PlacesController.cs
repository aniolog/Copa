using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    public class PlacesController : ApiController
    {
        // GET api/places
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/places/5
        public string Get(int id)
        {
            Models.Model.DataBase.GetInstance().Places.Add(new Models.Place());
            return "value";
        }

        // POST api/places
        public void Post([FromBody]string value)
        {
        }

        // PUT api/places/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/places/5
        public void Delete(int id)
        {

        }
    }
}
