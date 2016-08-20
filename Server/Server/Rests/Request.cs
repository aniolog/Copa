using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Rest
{
    public abstract class Request
    {

        public string BaseUrl { get; set; }

        public RestClient Client { get; set; }

        public RestRequest ServerRequest;

        public String MakeRequest() {
            IRestResponse _response = Client.Execute(ServerRequest);
            if ((_response.StatusCode != System.Net.HttpStatusCode.OK)||
                (_response.StatusCode != System.Net.HttpStatusCode.NoContent)){
                    return _response.Content;
            }

            throw new Exception("Error");
        }

    }
}