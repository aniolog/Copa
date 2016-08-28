using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Rate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        [Required] 
        public int Quantity { set; get; }

        [Required] 
        public DateTime BeginDate { set; get; }


        public DateTime EndDate { set; get; }

        [Required]
        public virtual Currency Currency { set; get; }


        public virtual ICollection<Bill> Bills { set; get; }


        public virtual Provider Provider { set; get; }


        public Rate()
        {
            this.Bills = new HashSet<Bill>();

        }
    }
}