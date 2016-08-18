using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    public class LogisticsDelegatesController : ApiController
    {
        // GET api/logisticsdelegates
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/logisticsdelegates/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/logisticsdelegates
        public void Post([FromBody]string value)
        {
        }

        // PUT api/logisticsdelegates/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/logisticsdelegates/5
        public void Delete(int id)
        {
        }
    }
}
