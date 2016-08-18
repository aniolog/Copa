using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    public class CurrenciesController : ApiController
    {
        // GET api/currencies
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/currencies/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/currencies
        public void Post([FromBody]string value)
        {
        }

        // PUT api/currencies/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/currencies/5
        public void Delete(int id)
        {
        }
    }
}
