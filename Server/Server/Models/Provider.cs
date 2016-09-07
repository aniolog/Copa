using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Provider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required] 
        public String Name { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public String SapId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required] 
        public String Telephone { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public String ContactName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual string Address { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Vehicle> Vehicles { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Request> Requests { set; get; }


        /// <summary>
        /// 
        /// </summary>
        public Provider():base()
        {
            this.Vehicles = new HashSet<Vehicle>();
            this.Requests = new HashSet<Request>();
        }

    }
}