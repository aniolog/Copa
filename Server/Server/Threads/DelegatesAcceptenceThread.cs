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

        private Persistences.RequestPersistence RequestPersistence;

        public DelegatesAcceptenceThread(Models.Request Request)
        {
            this.Request = Request;
            Thread _acceptenceThread = new Thread(this.Start);
            _acceptenceThread.Start();
        }

        private void Start() {
           System.Threading.Thread.Sleep(45 * 60 * 1000);
           this.CurrentContext = new Context();
           this.RequestPersistence =
                new Persistences.RequestPersistence(this.CurrentContext);
           this.Request = this.RequestPersistence.FindById(this.Request.Id);
           if((!(this.Request.IsApproved))&&(this.Request.CancelDelegate==null)){

               this.Request.IsApproved = false;
               this.Request.CancelReason = 
                   Threads.ThreadResources.CancelationDueToInactivityEvent;
               this.RequestPersistence.AddOrUpdateRequest(this.Request);


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

               _gcmPush.SendToUser
                   (JsonObject.ServerEvent.TeamMemberCanceldByInactivity.ToString(),""
                   ,Request.Id.ToString(), false);
               #endregion


               #region Push message to crewmember
               Device _memberDevice = 
                   Request.Team.First().Member.Device;
               Pushs.Push _pushService = 
                   Pushs.PushFactory.GetPushService(_memberDevice.Type);
               _pushService.AddToken(_memberDevice.Token);
               String _title;
               String _message;

               if (_memberDevice.Language == Device.DeviceLanguage.EN)
               {
                   _title = Pushs.PushResources.Notification_EN;
                   _message = Pushs.PushResources.DelegateInactivityMessage_EN;
               }
               else if (_memberDevice.Language == Device.DeviceLanguage.ES)
               {
                   _title = Pushs.PushResources.Notification_ES;
                   _message = Pushs.PushResources.DelegateInactivityMessage_ES;
               }
               else
               {
                   _title = Pushs.PushResources.Notification_BR;
                   _message = Pushs.PushResources.DelegateInactivityMessage_BR;
               }
               _pushService.SendToUser(_title,_message,Request.Id.ToString(),_memberDevice.Type);
               #endregion


           }
        }
    }
}