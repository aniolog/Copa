using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.Model
{
    public class DataBase
    {
        private static Context CurrentContext;

        public static Context GetInstance() {

            if (CurrentContext == null) {
                CurrentContext = new Context();
            }
            return CurrentContext;
        }
    }
}