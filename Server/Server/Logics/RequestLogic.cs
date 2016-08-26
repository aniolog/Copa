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
        Persistences.TeamMemberPersistence TeamMemberPersistence;
        Persistences.ProviderPersistence ProviderPersistence;

        public RequestLogic():base()
        {
            this.LogisteDelegatePersistence =
                new Persistences.LogisticsDelegatePersistence(this.CurrentContext);
            this.CrewMemberPersistence = 
                new Persistences.CrewMemberPersistence(this.CurrentContext);
            this.RequestPersistence = new 
                Persistences.RequestPersistence(this.CurrentContext);
            this.TeamMemberPersistence = 
                new Persistences.TeamMemberPersistence(this.CurrentContext);
            this.ProviderPersistence =
                new Persistences.ProviderPersistence(this.CurrentContext);

        }


        public void DelegateRegisterRequest(int DelegateId,Models.Request NewRequest){
            NewRequest.Id = 0;

            if (NewRequest.RequestDate < DateTime.Now.AddHours(1)) {
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
            NewRequest.Provider = 
                this.ProviderPersistence.FindById(NewRequest.Id);
            this.RequestPersistence.AddOrUpdateRequest(NewRequest);


        }

        public void DelegateAcceptRequest(long RequestId,long DelegateId,long ProviderId) {

            Models.Request _request = this.RequestPersistence.FindById(RequestId);

            if (_request.IsApproved == false)
            {
                throw new Exception("Request has been cancel");
            }
            Models.LogisticsDelegate _delegate =
                this.LogisteDelegatePersistence.FindById(DelegateId);
            _request.ApproveDelegate = _delegate;
            _request.CancelDelegate = null;
            _request.IsApproved = true;
            _request.Provider = this.ProviderPersistence.FindById(ProviderId);
            this.RequestPersistence.AddOrUpdateRequest(_request);

            //Send notifications
        }

        public void DelegateRejectRequest(long RequestId, long DelegateId)
        {

            Models.Request _request = this.RequestPersistence.FindById(RequestId);
            if (_request.IsApproved == true)
            {
                throw new Exception("Request has been accepted");
            }
            Models.LogisticsDelegate _delegate =
                this.LogisteDelegatePersistence.FindById(DelegateId);
            _request.CancelDelegate = _delegate;
            _request.ApproveDelegate = null;
            _request.IsApproved = false;
            this.RequestPersistence.AddOrUpdateRequest(_request);

            //Send notifications
        }

        public void CrewMemberRegisterRequest(long CrewMemberId, Models.Request NewRequest)
        {

            NewRequest.Id = 0;

            if (NewRequest.RequestDate < DateTime.Now.AddHours(1))
            {
                throw new Exception("Invalid Date");
            }
            if (NewRequest.Team.Any() == false)
            {
                throw new Exception("No suitable team");
            }

            Models.CrewMember _crewMember =
                this.CrewMemberPersistence.FindById(CrewMemberId);

            Models.TeamMember _teamMember = NewRequest.Team.First();
            _teamMember.Member = _crewMember;
            _teamMember.IsAccepted = true;
            _teamMember.CancelationReason = null;

            NewRequest.RegisterDelegate = null;
            NewRequest.ApproveDelegate = null;
            NewRequest.CancelDelegate = null;
            NewRequest.IsApproved = false;
            this.RequestPersistence.AddOrUpdateRequest(NewRequest);
            new Threads.DelegatesAcceptenceThread(NewRequest, this.CurrentContext);

            #region Web socket message to logistic delegate
            JsonObject _jsonMessage = new JsonObject();
            _jsonMessage.Event = JsonObject.ServerEvent.CrewMemberRegistedRequest;
            _jsonMessage.TriggerBy = _crewMember.Id.ToString();
            _jsonMessage.Data = JsonConvert.SerializeObject(NewRequest);
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
                , _crewMember.Id.ToString(), false);
            #endregion

        }

        public void CrewMemberAcceptRequest(long TeamMemberId ,long CrewMemberId)
        {
            Models.TeamMember _teamMember = this.TeamMemberPersistence.FindById(TeamMemberId);

            Models.CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);

            if (_crewMember.Id == _teamMember.Id)
            {
                throw new Exception("Request does not belong to crew member");
            }
            if (_teamMember != null)
            {
                if (_teamMember.IsAccepted == false)
                {

                    throw new Exception("Team member already canceld this request");
                }
            }

            _teamMember.IsAccepted = true;
            this.TeamMemberPersistence.AddOrUpdateRequest(_teamMember);
           
            //send notifications

        }

        public void CrewMemberRejectRequest(long TeamMemberId, long CrewMemberId)
        {
            Models.TeamMember _teamMember = this.TeamMemberPersistence.FindById(TeamMemberId);

            Models.CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);

            if (_crewMember.Id == _teamMember.Id)
            {
                throw new Exception("Request does not belong to crew member");
            }
            if (_teamMember != null)
            {
                if (_teamMember.IsAccepted == true)
                {
                    throw new Exception("Team member already accepted this request");
                }
            }

            _teamMember.IsAccepted = false;
            this.TeamMemberPersistence.AddOrUpdateRequest(_teamMember);

            //send notifications
        }

        public void CrewMemberModifyRequest
            (long TeamMemberId, long CrewMemberId,TeamMember ModifiedTeamMember)
        {
            Models.TeamMember _teamMember = this.TeamMemberPersistence.FindById(TeamMemberId);

            Models.CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);

            if (_crewMember.Id == _teamMember.Id)
            {
                throw new Exception("Request does not belong to crew member");
            }
            if (_teamMember.Request.RequestDate > DateTime.Now.AddMinutes(15)) {
                throw new Exception("Request change time lapse has expired");
            }

            if (_teamMember != null)
            {
                if (_teamMember.IsAccepted == false)
                {
                    throw new Exception("This request has been rejected");
                }
            }

            _teamMember.Lat = ModifiedTeamMember.Lat;
            _teamMember.Long = ModifiedTeamMember.Long;
            this.TeamMemberPersistence.AddOrUpdateRequest(_teamMember);
            //send notifications
        }

    }
}