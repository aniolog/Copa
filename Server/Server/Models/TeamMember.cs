using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class TeamMember
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
        public float Lat{ set; get;}

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public float Long { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAccepted { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public String CancelationReason { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public bool ProviderHasDeliver { set; get; }
        

        /// <summary>
        /// 
        /// </summary>
        [Required] 
        public CrewMember Member { set; get; }

        /// <summary>   
        /// 
        /// </summary>
        [Required]
        public virtual Request Request { set; get; }

    }
}