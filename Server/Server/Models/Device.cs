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
        /// </summary>
        [Required] 
        public Boolean Type{set; get;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<CrewMember> CrewMembers{ set; get;}

        /// <summary>
        /// 
        /// </summary>
        public ICollection<LogisticsDelegate> LogisticsDelegates { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Provider> Providers { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public Device()
        {
            this.CrewMembers = new HashSet<CrewMember>();
            this.LogisticsDelegates = new HashSet<LogisticsDelegate>();
            this.Providers = new HashSet<Provider>();
        }

    }
}