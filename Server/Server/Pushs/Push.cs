using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Pushs
{
    public abstract class Push
    {
        abstract public void Send(String Title,String Message, String Data);
        abstract public void AddToken(String Token);


        public void SendToUser(String Title, String Message, String Data, Boolean Type) { 
        
      

         
                //this.Send(Title,Message,Data);
              
        
        
        }


    }
}
