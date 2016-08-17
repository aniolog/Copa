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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        public String CancelationReason { set; get; }

        public bool ProviderHasDeliver { set; get; }

        [Required] 
        public CrewMember Member { set; get; }


        [Required] 
        public Request Request { set; get; }
    }
}