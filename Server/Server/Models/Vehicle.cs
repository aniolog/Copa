using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Required] 
        public VehicleType Type { set; get; }

        public ICollection<Provider> Providers { set; get; }

        public ICollection<Request> Request { set; get; }

        public Vehicle()
        {
            this.Providers = new HashSet<Provider>();
            this.Request = new HashSet<Request>();
        }


        public enum VehicleType { 
            Van,Suv,Car
        }
    }
}