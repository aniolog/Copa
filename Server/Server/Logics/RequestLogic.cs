using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.WebSockets;
using Newtonsoft.Json;

namespace Server.Logics
{
    public class RequestLogic:BaseLogic
    {
        Persistences.LogisticsDelegatePersistence LogisteDelegatePersistence;
        Persistences.CrewMemberPersistence CrewMemberPersistence;
        Persistences.RequestPersistence RequestPersistence;

        public RequestLogic():base()
        {
            this.LogisteDelegatePersistence =
                new Persistences.LogisticsDelegatePersistence(this.CurrentContext);
            this.CrewMemberPersistence = 
                new Persistences.CrewMemberPersistence(this.CurrentContext);
            this.RequestPersistence = new Persistences.RequestPersistence(this.CurrentContext);

        }


        public void DelegateRegisterRequest(int DelegateId,Models.Request NewRequest) {
            NewRequest.Id = 0;

            if (NewRequest.RequestDate < DateTime.Now) {
                throw new Exception("Invalid Date");
            }
            if (NewRequest.Team.Any() == false)
            {
                throw new Exception("No suitable team");
            }


            foreach (TeamMember _teamMember in NewRequest.Team) {

                _teamMember.Id = 0;

                Models.CrewMember _crewMember =
                    this.CrewMemberPersistence.FindById(_teamMember.Member.Id);

                if (_crewMember == null)
                {
                    throw new Exception("Crew member not found");
                }

                #region Push notification to CrewMember
                Models.Device _memberDevice = _crewMember.Device;
                Pushs.Push _pushService = 
                    Pushs.PushFactory.GetPushService(_memberDevice.Type);
                _pushService.AddToken(_memberDevice.Token);
                String _title;

                if (_memberDevice.Language == Device.DeviceLanguage.EN)
                {
                    _title = Pushs.PushResources.DelegateRegisterNewRequest_EN;
                }
                else if (_memberDevice.Language == Device.DeviceLanguage.ES)
                {
                    _title = Pushs.PushResources.DelegateRegisterNewRequest_ES;
                }
                else
                {
                    _title = Pushs.PushResources.DelegateRegisterNewRequest_BR;
                }

                _pushService.SendToUser(_title,"", _memberDevice.Type);
                #endregion


                new Threads.CrewMemberAcceptenceThread(_teamMember,this.CurrentContext);

                _teamMember.Member = _crewMember;
                
            }
        

            Models.LogisticsDelegate _delegate =
                this.LogisteDelegatePersistence.FindById(DelegateId);

            NewRequest.RegisterDelegate = _delegate;
            NewRequest.ApproveDelegate = _delegate;

            this.RequestPersistence.AddOrUpdateRequest(NewRequest);


        }

        public void CrewMemberRegisterRequest(long CrewMemberId, Models.Request NewRequest)
        {
           
                NewRequest.Id = 0;

                if (NewRequest.RequestDate < DateTime.Now)
                {
                    throw new Exception("Invalid Date");
                }
                if (NewRequest.Team.Any()==false) {
                    throw new Exception("No suitable team");
                }

                Models.CrewMember _crewMember = 
                    this.CrewMemberPersistence.FindById(CrewMemberId);

                if (_crewMember == null) {
                    throw new Exception("Crew member not found");
                }

                Models.TeamMember _teamMember = NewRequest.Team.First();
                _teamMember.Member = _crewMember;

                NewRequest.RegisterDelegate = null;
                NewRequest.ApproveDelegate = null;
                NewRequest.CancelDelegate = null;
                NewRequest.IsApproved = false;
                this.RequestPersistence.AddOrUpdateRequest(NewRequest);
                new Threads.DelegatesAcceptenceThread(NewRequest,this.CurrentContext);

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.CrewMemberRegistedRequest;
                _jsonMessage.TriggerBy = _crewMember.Id.ToString();
                _jsonMessage.Data = "";
                WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion

                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach (LogisticsDelegate _delegate in CurrentContext.LogisticDelegates)
                {
                    _gcmPush.AddToken(_delegate.Device.Token);
                }
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.CrewMemberRegistedRequest.ToString()
                    ,_crewMember.Id.ToString(), false);
                #endregion
           
        }

    }
}