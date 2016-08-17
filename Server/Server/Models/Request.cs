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

        public String ProviderHasCanceldReason { set; get; }

        [Required] 
        public DateTime RequestDate { set; get; }

        public Place Destination { set; get; }

        public ICollection<Vehicle> RequestedVehicles { set; get; }

        public LogisticsDelegate CancelDelegate { set; get; }

      
        public LogisticsDelegate RegisterDelegate { set; get; }

        public LogisticsDelegate ApproveDelegate { set; get; }

        public ICollection<TeamMember> Team { set; get; }

        public Request()
        {
            this.RequestedVehicles = new HashSet<Vehicle>();
            this.Team = new HashSet<TeamMember>();

        }
        
    }
}