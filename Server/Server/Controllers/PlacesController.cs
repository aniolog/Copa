using Server.Logics;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server.Controllers
{
    [Authorize(Roles="crewmember")]
    [RoutePrefix("api/places")]
    public class PlacesController : ApiController
    {
        // GET api/places
        [Route("")]
        [HttpGet]
        public IEnumerable<Place> Get()
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            PlaceLogic _logic = new PlaceLogic();
            return _logic.FindPlaces(Id);
        }


        // POST api/places
        [Route("")]
        [HttpPost]
        public void Post([FromBody]Place value)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            PlaceLogic _logic = new PlaceLogic();
            _logic.AddPlace(Id,value);
        }

        // PUT api/places/1
        [Route("{PlaceId}")]
        [HttpPut]
        public void Put([FromUri] int PlaceId, [FromBody]Place value)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            PlaceLogic _logic = new PlaceLogic();
            value.Id = PlaceId;
            _logic.UpdatePlace(value,Id);
        }

        // DELETE api/places/5
        [Route("{PlaceId}")]
        [HttpDelete]
        public void Delete([FromUri] int PlaceId)
        {
            int Id = int.Parse(RequestContext.Principal.Identity.Name);
            PlaceLogic _logic = new PlaceLogic();
            _logic.DeletePlace(PlaceId,Id);
        }


        [Route("geocoding/{Address}")]
        [HttpGet]
        public List<Place> GetGeoCoding([FromUri] String Address) {
            Rests.GeoCodingRequest Request = new Rests.GeoCodingRequest(Address);
            return Request.GetGeoCoding();
            
        }

    }
}
