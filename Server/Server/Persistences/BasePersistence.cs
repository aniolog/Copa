using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public abstract class BasePersistence
    {
        /// <summary>
        /// 
        /// </summary>
        public Models.Context CurrentContext{set;get;}


    }
}