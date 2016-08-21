using Newtonsoft.Json;
using RestSharp;
using Server.Models;
using Server.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Rests
{
    public class GeoCodingRequest:Server.Rest.Request 
    {
        public GeoCodingRequest(String address)
        {
            String _formatedAddress = address.Replace(" ","+");
            String _baseUrl = 
                String.Format(RestResources.GeoCoding_BaseUrl,_formatedAddress,RestResources.GeoCoding_ApiKey);
            this.Client = new RestSharp.RestClient(_baseUrl);
            this.ServerRequest =
                new RestSharp.RestRequest("", Method.GET);
            this.Response=this.MakeRequest();
        }

        public List<Place> GetGeoCoding()
        {
            List<Place> _reponsePlaces = new List<Place>();
            GoogleGeoCodeResponse _objectReponse=
                JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(this.Response);
            foreach (results _result in _objectReponse.results) {
                Place _place = new Place();
                _place.Name = _result.formatted_address;
                _place.Lat = float.Parse(_result.geometry.location.lat);
                _place.Long = float.Parse(_result.geometry.location.lng);
                _reponsePlaces.Add(_place);
            }
            return _reponsePlaces;
        }


    }

}