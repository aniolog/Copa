using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Server.WebSockets;
using Server.Filters;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Threads
{
    public class DelegatesAcceptenceThread
    {
        private Models.Request Request;

        private Models.Context CurrentContext;

        public DelegatesAcceptenceThread(Models.Request Request, Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
            this.Request = Request;
            Thread _acceptenceThread = new Thread(this.Start);
            _acceptenceThread.Start();
        }

        private void Start() {
            System.Threading.Thread.Sleep(45 * 60 * 1000);
           if((!(this.Request.IsApproved))&&(this.Request.CancelDelegate==null)){

               this.Request.IsApproved = false;
               this.Request.CancelReason = 
                   Threads.ThreadResources.CancelationDueToInactivityEvent;
               CurrentContext.SaveChanges();


               #region Web socket message to logistic delegate
               JsonObject _jsonMessage = new JsonObject();
               _jsonMessage.Event = JsonObject.ServerEvent.RequestCancelByInactivity;
               _jsonMessage.TriggerBy = this.GetType().ToString();
               _jsonMessage.Data = Request.Id.ToString() ;
               WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
               #endregion


               #region Gcm Push message to logistics delegates
               Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
               foreach (LogisticsDelegate _delegate in CurrentContext.LogisticDelegates)
               {
                   _gcmPush.AddToken(_delegate.Device.Token);

               }

               Pushs.GCM.GcmMessage _gcmMessage = new Pushs.GCM.GcmMessage();
               _gcmMessage.title = JsonObject.ServerEvent.CrewMemberRejectedRequest.ToString();
               _gcmMessage.data = Request.Id.ToString();
               _gcmPush.Send(JsonConvert.SerializeObject(_gcmMessage));
               #endregion


               #region Push message to crewmember
               Device _memberDevice = 
                   Request.Team.First().Member.Device;
               Pushs.Push _pushService = (!(_memberDevice.Type)) ?
                   Pushs.PushFactory.GetGcmPushSender() :
                   Pushs.PushFactory.GetIosPushSender();
               _pushService.AddToken(_memberDevice.Token);
               String _message;
               String _title;

               if (_memberDevice.Language == Device.DeviceLanguage.en)
               {
                   _title = Pushs.PushResources.DelegateInactivity_EN;
               }
               else if (_memberDevice.Language == Device.DeviceLanguage.es)
               {
                   _title = Pushs.PushResources.DelegateInactivity_ES;
               }
               else
               {
                   _title = Pushs.PushResources.DelegateInactivity_BR;
               }

               if (_memberDevice.Type)
               {
                   Pushs.GCM.GcmMessage _gcmMember = new Pushs.GCM.GcmMessage();
                   _gcmMember.title = _title;
                   _gcmMember.data = Request.Id.ToString();
                   _message = JsonConvert.SerializeObject(_gcmMember);
               }
               else
               {
                   Pushs.Ios.ApnMessage _apnMember = new Pushs.Ios.ApnMessage();
                   _apnMember.alert = _title;
                   _apnMember.data = Request.Id.ToString();
                   _message = JsonConvert.SerializeObject(_apnMember);
               }

               _pushService.Send(_message);
               #endregion


           }
        }
    }
}