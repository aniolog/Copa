using Newtonsoft.Json;
using Server.Filters;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Server.Threads
{
    public class CrewMemberAcceptenceThread
    {
        /// <summary>
        /// Team member that the thread will checkout 
        /// </summary>
        private Models.TeamMember TeamMember;

        /// <summary>
        /// Context of the database
        /// </summary>
        private Models.Context CurrentContext;

        /// <summary>
        /// This constructor instantiate and start the thread.
        /// </summary>
        /// <param name="TeamMember"></param>
        /// <param name="CurrentContext"></param>
        public CrewMemberAcceptenceThread(Models.TeamMember TeamMember, Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
            this.TeamMember = TeamMember;
            Thread _acceptenceThread = new Thread(this.Start);
            _acceptenceThread.Start();

        }

        /// <summary>
        /// Thread that checks if the crewmember has answer the request
        /// in case that the crewmember has not answer the request the thread
        /// will denied and send the proper notifications.
        /// </summary>
        private void Start(){
            System.Threading.Thread.Sleep(5 * 60 * 1000);
            if (!(TeamMember.IsAccepted)) {


                TeamMember.IsAccepted = false;
                TeamMember.CancelationReason = 
                    Threads.ThreadResources.CancelationDueToInactivityEvent;
                CurrentContext.SaveChanges();
            

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.TeamMemberCanceldByInactivity;
                _jsonMessage.TriggerBy = this.GetType().ToString();
                _jsonMessage.Data = Threads.ThreadResources.CancelationDueToInactivityEvent;
                WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion
                
                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach(LogisticsDelegate _delegate in CurrentContext.LogisticDelegates){
                    _gcmPush.AddToken(_delegate.Device.Token);
                    
                }

                Pushs.GCM.GcmMessage _gcmMessage = new Pushs.GCM.GcmMessage();
                _gcmMessage.title = JsonObject.ServerEvent.TeamMemberCanceldByInactivity.ToString();
                _gcmMessage.data = this.TeamMember.Id.ToString();
                _gcmPush.Send(JsonConvert.SerializeObject(_gcmMessage));
                #endregion

                #region Push message to crewmember
                Device  _memberDevice=TeamMember.Member.Device;
                Pushs.Push _pushService = (!(_memberDevice.Type)) ?
                    Pushs.PushFactory.GetGcmPushSender() :
                    Pushs.PushFactory.GetIosPushSender();
                _pushService.AddToken(_memberDevice.Token);
                String _message;
                String _title;

                if (_memberDevice.Language == Device.DeviceLanguage.en)
                {
                    _title = Pushs.PushResources.TeamMemberInactivity_EN;
                }
                else if (_memberDevice.Language == Device.DeviceLanguage.es)
                {
                    _title = Pushs.PushResources.TeamMemberInactivity_ES;
                }
                else
                {
                    _title = Pushs.PushResources.TeamMemberInactivity_BR;
                }
        
                if (_memberDevice.Type)
                {
                    Pushs.GCM.GcmMessage _gcmMember = new Pushs.GCM.GcmMessage();
                    _gcmMember.title = _title;
                    _gcmMember.data = TeamMember.Id.ToString();
                    _message = JsonConvert.SerializeObject(_gcmMember);
                }
                else {
                    Pushs.Ios.ApnMessage _apnMember = new Pushs.Ios.ApnMessage();
                    _apnMember.alert = _title;
                    _apnMember.data = TeamMember.Id.ToString();
                    _message = JsonConvert.SerializeObject(_apnMember);
                }

                _pushService.Send(_message);
                #endregion
            }
        }
    }
}