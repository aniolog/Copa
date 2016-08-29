using Newtonsoft.Json;
using Server.Filters;
using Server.Models;
using Server.Persistences;
using Server.WebSockets;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        /// 
        /// </summary>
        private Persistences.TeamMemberPersistence TeamMemberPersitence;


        /// <summary>
        /// 
        /// </summary>
        private Persistences.RequestPersistence RequestPersistence;


        /// <summary>
        /// 
        /// </summary>
        private Persistences.CrewMemberPersistence CrewMemberPersistence;


        /// <summary>
        /// This constructor instantiate and start the thread.
        /// </summary>
        /// <param name="TeamMember"></param>
        /// <param name="CurrentContext"></param>
        public CrewMemberAcceptenceThread(Models.TeamMember TeamMember)
        {
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
            
                System.Threading.Thread.Sleep(30 * 60 * 1000);
                this.CurrentContext = new Context();
                this.TeamMemberPersitence =
                    new Persistences.TeamMemberPersistence(this.CurrentContext);
                this.RequestPersistence = new RequestPersistence(this.CurrentContext);
                this.CrewMemberPersistence = new CrewMemberPersistence(this.CurrentContext);
                this.TeamMember = this.TeamMemberPersitence.FindById(this.TeamMember.Id);


                if (this.TeamMember.IsAccepted == null)
                {

                    
                      this.TeamMember.IsAccepted = false;
                      this.TeamMember.CancelationReason =
                        Threads.ThreadResources.CancelationDueToInactivityEvent;
                      this.TeamMember.Member = 
                          this.CrewMemberPersistence.FindById(this.TeamMember.Member.Id);
                      this.TeamMember.Request = 
                          this.RequestPersistence.FindById(this.TeamMember.Request.Id);

                    this.TeamMemberPersitence.AddOrUpdateRequest(this.TeamMember);
                    

                    #region Web socket message to logistic delegate
                    JsonObject _jsonMessage = new JsonObject();
                    _jsonMessage.Event = JsonObject.ServerEvent.TeamMemberCanceldByInactivity;
                    _jsonMessage.TriggerBy = this.GetType().ToString();
                    _jsonMessage.Data = Threads.ThreadResources.CancelationDueToInactivityEvent;
                    WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                    #endregion

                    #region Gcm Push message to logistics delegates
                    Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                    foreach (LogisticsDelegate _delegate in CurrentContext.LogisticDelegates)
                    {
                        _gcmPush.AddToken(_delegate.Device.Token);
                    }
                    _gcmPush.SendToUser
                        (JsonObject.ServerEvent.TeamMemberCanceldByInactivity.ToString(),
                        "", this.TeamMember.Id.ToString(), false);
                    #endregion

                    #region Push message to crewmember
                    Device _memberDevice = TeamMember.Member.Device;
                    Pushs.Push _pushService =
                        Pushs.PushFactory.GetPushService(_memberDevice.Type);
                    _pushService.AddToken(_memberDevice.Token);
                    String _title;
                    String _message;

                    if (_memberDevice.Language == Device.DeviceLanguage.EN)
                    {
                        _title = Pushs.PushResources.Notification_EN;
                        _message = Pushs.PushResources.TeamMemberInactivityMessage_EN;
                    }
                    else if (_memberDevice.Language == Device.DeviceLanguage.ES)
                    {
                        _title = Pushs.PushResources.Notification_ES;
                        _message = Pushs.PushResources.TeamMemberInactivityMessage_ES;
                    }
                    else
                    {
                        _title = Pushs.PushResources.Notification_BR;
                        _message = Pushs.PushResources.TeamMemberInactivityMessage_BR;
                    }

                    _pushService.SendToUser(_title, _message, TeamMember.Id.ToString(), _memberDevice.Type);

                    #endregion
                }
            
        }
    }
}