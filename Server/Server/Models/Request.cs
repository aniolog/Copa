using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        public bool ProviderHasCanceld { set; get; }

        public String CancelReason { set; get; }

        [Required] 
        public DateTime RequestDate { set; get; }

        [Required]
        public float Lat { set; get; }

        [Required]
        public float Long { set; get; }

        public bool IsApproved { set; get; }

        public virtual ICollection<RequestedVehicle> RequestedVehicles { set; get; }

        public virtual LogisticsDelegate CancelDelegate { set; get; }

        public virtual LogisticsDelegate RegisterDelegate { set; get; }

        public virtual LogisticsDelegate ApproveDelegate { set; get; }

        public virtual ICollection<TeamMember> Team { set; get; }

        public virtual Provider Provider { set; get; }

        public Request()
        {
            this.RequestedVehicles = new HashSet<RequestedVehicle>();
            this.Team = new HashSet<TeamMember>();

        }
        
    }
}