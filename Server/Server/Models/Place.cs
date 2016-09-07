using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Place
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
        [Required] 
        public float Lat { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required] 
        public float Long { set; get; }


        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [Required]
        public virtual CrewMember CrewMember { set; get; }


        
    }
}