using Newtonsoft.Json;
using Server.Filters;
using Server.Models;
using Server.WebSockets;
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
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.TeamMemberCanceldByInactivity.ToString()
                    ,this.TeamMember.Id.ToString(),false);
                #endregion

                #region Push message to crewmember
                Device  _memberDevice=TeamMember.Member.Device;
                Pushs.Push _pushService = 
                    Pushs.PushFactory.GetPushService(_memberDevice.Type);
                _pushService.AddToken(_memberDevice.Token);
                String _title;

                if (_memberDevice.Language == Device.DeviceLanguage.EN)
                {
                    _title = Pushs.PushResources.TeamMemberInactivity_EN;
                }
                else if (_memberDevice.Language == Device.DeviceLanguage.ES)
                {
                    _title = Pushs.PushResources.TeamMemberInactivity_ES;
                }
                else
                {
                    _title = Pushs.PushResources.TeamMemberInactivity_BR;
                }

                _pushService.SendToUser(_title, TeamMember.Id.ToString(),_memberDevice.Type);

                #endregion
            }
        }
    }
}