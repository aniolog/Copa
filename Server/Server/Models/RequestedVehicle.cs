using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class RequestedVehicle
    {
        
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public long Quantity { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Vehicle Vehicle { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Request Request { set; get; }

    }
}