using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Logics
{
    public class BaseLogic
    {
        /// <summary>
        /// The Curent database context
        /// </summary>
        public   Models.Context CurrentContext{set;get;}

        public BaseLogic()
        {
            this.CurrentContext = new Models.Context();
        }

    }
}