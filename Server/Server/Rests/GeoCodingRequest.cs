using Newtonsoft.Json;
using Server.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Rests
{
    public class GeoCodingRequest:Request 
    {
        public GeoCodingRequest(String address)
        {
            String _formatedAddress = address.Replace(" ","+");
            this.Client = new RestSharp.RestClient(String.Format(RestResources.GeoCoding_BaseUrl, 
                _formatedAddress, RestResources.GeoCoding_ApiKey));
            this.ServerRequest = new RestSharp.RestRequest();
        }

        public List<Models.Place> GetGeoCoding(){
            String _reponse=this.MakeRequest();
            Dictionary<string, string> values =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(_reponse);
            return null;
        }


    }
}