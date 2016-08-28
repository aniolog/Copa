using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Currency
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
        public String Symbol { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Rate> Rates { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public Currency()
        {
            this.Rates = new HashSet<Rate>(); 
        }

    }
}