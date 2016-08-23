using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
   // [Authorize(Roles = "logisticdelegate")]
    [RoutePrefix("api/providers")]
    public class ProvidersController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IEnumerable<Models.Provider> Get()
        {
            Logics.ProviderLogic _logic = new Logics.ProviderLogic();
            return _logic.GetProviders();
        }

        // GET api/provides/5
        [Route("{Id}")]
        [HttpGet]
        public Models.Provider Get(long Id)
        {
            Logics.ProviderLogic _logic = new Logics.ProviderLogic();
            return _logic.FindProvider(Id);
        }

        // POST api/provides
        [Route("")]
        [HttpPost]
        public void Post([FromBody]Models.Provider value)
        {
            Logics.ProviderLogic _logic = new Logics.ProviderLogic();
            _logic.AddProvider(value);
        }

        // PUT api/provides/5
        [Route("{Id}")]
        [HttpPut]
        public void Put(long Id, [FromBody]Models.Provider value)
        {
            Logics.ProviderLogic _logic = new Logics.ProviderLogic();
            _logic.UpdateProvider(Id,value);
        }

    }
}
