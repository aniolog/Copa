using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        /// <summary>
        /// Device token, the value of this attribute is generated in the push service provider 
        /// and later send to the backen through the user device
        /// </summary>
        [Required] 
        public String Token { set; get; }

        /// <summary>
        /// this attribute determines the push service that the device is associated with
        /// false for Gcm and true for ios
        /// </summary>
        [Required] 
        public Boolean Type{set; get;}

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DeviceLanguage Language { set; get; }    

        public enum DeviceLanguage { 
            EN,ES,BR
        }
    }
}