using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class LogisticsDelegate:User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        /// <summary>
        /// The real name of the logistics delegate
        /// </summary>
        [Required] 
        public String Name { set; get; }

        /// <summary>
        /// The real last name of the logistics delegate
        /// </summary>
        [Required] 
        public String LastName { set; get; }

        /// <summary>
        /// This is attribute currently is not being use in any instance.
        /// </summary>
        public String SapId { set; get; }

        /// <summary>
        /// This attribute repesent that the delegate is an admin
        /// </summary>
        public Boolean IsAdmin { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public LogisticsDelegate():base()
        {
            
        }
    }
}