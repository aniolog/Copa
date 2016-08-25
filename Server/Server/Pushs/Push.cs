using Newtonsoft.Json;
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
        abstract  public void Send(String Message);
        abstract public void AddToken(String Token);


        public void SendToUser(String Tittle, String Data, Boolean Type) { 
        
                #region Push message to User
                Pushs.Push _pushService = (!(Type)) ?
                    Pushs.PushFactory.GetGcmPushSender() :
                    Pushs.PushFactory.GetIosPushSender();
                String _message;

                if (!(Type))
                {
                    Pushs.GCM.GcmMessage _gcmMember = new Pushs.GCM.GcmMessage();
                    _gcmMember.title = Tittle;
                    _gcmMember.data = Data;
                    _message = JsonConvert.SerializeObject(_gcmMember);
                }
                else
                {
                    Pushs.Ios.ApnMessage _apnMember = new Pushs.Ios.ApnMessage();
                    _apnMember.alert = Tittle;
                    _apnMember.data =  Data;
                    _message = JsonConvert.SerializeObject(_apnMember);
                }
                this.Send(_message);
                #endregion
        
        
        }


    }
}
