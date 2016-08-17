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
        public ICollection<TeamMember> TeamMembers { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required] 
        public CrewMember CrewMember { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Request> Requests { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public Place()
        {
            this.TeamMembers = new HashSet<TeamMember>();
            this.Requests = new HashSet<Request>();


        }
        
    }
}