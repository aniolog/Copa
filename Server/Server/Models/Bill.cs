using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    [Table("Bill")]
    public class Bill
    {
        /// <summary>
        /// This attributes represent the 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required] 
        public DateTime BeginDate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public long SapId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public Rate Rate { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Required] 
        public Provider Provider { set; get; }



    }
}