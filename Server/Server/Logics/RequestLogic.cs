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

   
        /// <summary>
        /// This method allows each delegate to register a request
        /// Once the request has passed all the validation 
        /// the request will be stored in the database
        /// and then the server will send the notification to each team members
        /// in the request
        /// </summary>
        /// <param name="DelegateId"></param>
        /// <param name="NewRequest"></param>
        public void DelegateRegisterRequest(int DelegateId,Models.Request NewRequest){
            try
            {
                NewRequest.Id = 0;

                if (NewRequest.RequestDate < DateTime.Now.AddHours(1))
                {
                    throw new Exceptions.InvalidDateException();
                }
                if (NewRequest.Team.Any() == false)
                {
                    throw new Exceptions.NoSuitableTeamException();
                }

                foreach (TeamMember _teamMember in NewRequest.Team)
                {
                    _teamMember.Id = 0;
                    Models.CrewMember _crewMember =
                        this.CrewMemberPersistence.FindById(_teamMember.Member.Id);
                    if (_crewMember == null)
                    {
                        throw new Exceptions.CrewMemberNotFoundException();
                    }
                    _teamMember.Member = _crewMember;
                }

                Models.LogisticsDelegate _delegate =
                    this.LogisteDelegatePersistence.FindById(DelegateId);
                NewRequest.IsApproved = true;
                NewRequest.RegisterDelegate = _delegate;
                NewRequest.ApproveDelegate = _delegate;
                NewRequest.Provider =
                    this.ProviderPersistence.FindById(NewRequest.Id);
                Models.Request _savedRequest =
                    this.RequestPersistence.AddOrUpdateRequest(NewRequest);

                foreach (TeamMember _threadTeamMember in _savedRequest.Team)
                {
                    new Threads.CrewMemberAcceptenceThread(_threadTeamMember);
                    #region Push notification to CrewMember
                        Models.Device _memberDevice = _threadTeamMember.Member.Device;
                        Pushs.Push _pushService =
                            Pushs.PushFactory.GetPushService(_memberDevice.Type);
                        _pushService.AddToken(_memberDevice.Token);
                        String _title;
                        String _message;

                        if (_memberDevice.Language == Device.DeviceLanguage.EN)
                        {
                            _title = Pushs.PushResources.Notification_EN;
                            _message = Pushs.PushResources.DelegateRegisterNewRequestMessage_EN;
                        }
                        else if (_memberDevice.Language == Device.DeviceLanguage.ES)
                        {
                            _title = Pushs.PushResources.Notification_ES;
                            _message = Pushs.PushResources.DelegateRegisterNewRequestMessage_ES;
                        }
                        else
                        {
                            _title = Pushs.PushResources.Notification_BR;
                            _message = Pushs.PushResources.DelegateRegisterNewRequestMessage_BR;
                        }
                        String _serializedRequest = Newtonsoft.Json.JsonConvert.SerializeObject(NewRequest);
                        _pushService.SendToUser(_title, _message, _serializedRequest, _memberDevice.Type);
                    #endregion
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        /// <summary>
        /// This method allows each delegate to accept a crew member registerd request
        /// if the request has been rejected before the method will trow an exception
        /// the same behavior is preset if the request has been canceld before.
        /// once the request has been rejected the server will send the proper notification
        /// to the crew member and each logistic delegate.
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="DelegateId"></param>
        /// <param name="ProviderId"></param>
        public void DelegateAcceptRequest(long RequestId,long DelegateId,long ProviderId) {
            try
            {
                Models.Request _request = this.RequestPersistence.FindById(RequestId);

                if ((_request.IsApproved == false)&&(_request.CancelReason!=null))
                {
                    throw new Exception("Request has been cancel");
                }
                Models.LogisticsDelegate _delegate =
                    this.LogisteDelegatePersistence.FindById(DelegateId);
                _request.ApproveDelegate = _delegate;
                _request.CancelDelegate = null;
                _request.IsApproved = true;
                _request.Provider = this.ProviderPersistence.FindById(ProviderId);
                _request = this.RequestPersistence.AddOrUpdateRequest(_request);

                #region Push notification to CrewMember
                Models.CrewMember _crewMember = _request.Team.First().Member;
                Models.Device _memberDevice = _crewMember.Device;
                Pushs.Push _pushService =
                    Pushs.PushFactory.GetPushService(_memberDevice.Type);
                _pushService.AddToken(_memberDevice.Token);
                String _title;
                String _message;

                if (_memberDevice.Language == Device.DeviceLanguage.EN)
                {
                    _title = Pushs.PushResources.Notification_EN;
                    _message = Pushs.PushResources.DelegateAcceptedRequestMessage_EN;
                }
                else if (_memberDevice.Language == Device.DeviceLanguage.ES)
                {
                    _title = Pushs.PushResources.Notification_ES;
                    _message = Pushs.PushResources.DelegateAcceptedRequestMessage_ES;
                }
                else
                {
                    _title = Pushs.PushResources.Notification_BR;
                    _message = Pushs.PushResources.DelegateAcceptedRequestMessage_BR;
                }

                String _serializedRequest = Newtonsoft.Json.JsonConvert.SerializeObject(_request);
                _pushService.SendToUser(_title, _message,_serializedRequest, _memberDevice.Type);
                #endregion

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.DelegateAcceptedRequest;
                _jsonMessage.TriggerBy = DelegateId.ToString();
                _jsonMessage.Data = _request.Id.ToString();
                WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion

                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach (LogisticsDelegate _delegateGcm in CurrentContext.LogisticDelegates)
                {
                    _gcmPush.AddToken(_delegateGcm.Device.Token);
                }
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.DelegateAcceptedRequest.ToString(),""
                    , _request.Id.ToString(), false);
                #endregion
            }
            catch (Exception E)
            {
                throw E;
            }

        }

        /// <summary>
        /// This method allows each delegate to reject a crew member register request
        /// if the request has been confirmed before the method will trow an exception
        /// the same behavior is preset if the request has been canceld before.
        /// once the request has been rejected the server will send the proper notification
        /// to the crew member and each logistic delegate.
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="DelegateId"></param>
        public void DelegateRejectRequest(long RequestId, long DelegateId,String CancelReason)
        {
            try
            {
                
                Models.Request _request = this.RequestPersistence.FindById(RequestId);
                if (_request.IsApproved == true)
                {
                    throw new Exception("Request has been accepted");
                }
                if((_request.CancelReason!=null)&&(_request.IsApproved==false)){
                    throw new Exception("Request has been canceld");
                }

                Models.LogisticsDelegate _delegate =
                    this.LogisteDelegatePersistence.FindById(DelegateId);
                _request.CancelDelegate = _delegate;
                _request.ApproveDelegate = null;
                _request.IsApproved = false;
                _request.CancelReason = CancelReason;
                this.RequestPersistence.AddOrUpdateRequest(_request);

                #region Push notification to CrewMember
                Models.CrewMember _crewMember = _request.Team.First().Member;
                Models.Device _memberDevice = _crewMember.Device;
                Pushs.Push _pushService =
                    Pushs.PushFactory.GetPushService(_memberDevice.Type);
                _pushService.AddToken(_memberDevice.Token);
                String _title;
                String _message;
                if (_memberDevice.Language == Device.DeviceLanguage.EN)
                {
                    _title = Pushs.PushResources.Notification_EN;
                    _message = Pushs.PushResources.DelegateRejectedRequestMessage_EN;
                }
                else if (_memberDevice.Language == Device.DeviceLanguage.ES)
                {
                    _title = Pushs.PushResources.Notification_ES;
                    _message = Pushs.PushResources.DelegateRejectedRequestMessage_ES;
                }
                else
                {
                    _title = Pushs.PushResources.Notification_BR;
                    _message = Pushs.PushResources.DelegateRejectedRequestMessage_BR;
                }

                String _serializedRequest = Newtonsoft.Json.JsonConvert.SerializeObject(_request);
                _pushService.SendToUser(_title, _message,_serializedRequest, _memberDevice.Type);
                #endregion

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.DelegateRejectedRequest;
                _jsonMessage.TriggerBy = DelegateId.ToString();
                _jsonMessage.Data = _request.Id.ToString();
                WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion

                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach (LogisticsDelegate _notifcationDelegate in CurrentContext.LogisticDelegates)
                {
                    if (_delegate.Device != null)
                    {
                        _gcmPush.AddToken(_notifcationDelegate.Device.Token);
                    }
                }
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.DelegateRejectedRequest.ToString(),
                    "", _crewMember.Id.ToString(), false);
                #endregion

            }
            catch (Exception E)
            {
                throw E;
            }

         }

        /// <summary>
        /// This method allows each crew member to register a new request
        /// the request need to fullfill the follow requeriments:
        /// -The datetime of the requests neeed to be at least an hour after
        /// the registration time
        /// -Need to have a suitable team, this means that the team needs to have 
        /// at least a crew member.
        /// Once the request have been stored in the database the server will 
        /// send the proper notifications to each logistic delegates
        /// </summary>
        /// <param name="CrewMemberId"></param>
        /// <param name="NewRequest"></param>
        public void CrewMemberRegisterRequest(long CrewMemberId, Models.Request NewRequest)
        {
            try
            {
                NewRequest.Id = 0;

                if (NewRequest.RequestDate < DateTime.Now.AddHours(1))
                {
                    throw new Exceptions.InvalidDateException();
                }
                if (NewRequest.Team.Any() == false)
                {
                    throw new Exceptions.NoSuitableTeamException();
                }

                Models.CrewMember _crewMember =
                    this.CrewMemberPersistence.FindById(CrewMemberId);
                if (_crewMember == null)
                {
                    throw new Exceptions.CrewMemberNotFoundException();
                }

                Models.TeamMember _teamMember = NewRequest.Team.First();
                _teamMember.Member = _crewMember;
                _teamMember.IsAccepted = true;
                _teamMember.CancelationReason = null;

                NewRequest.RegisterDelegate = null;
                NewRequest.ApproveDelegate = null;
                NewRequest.CancelDelegate = null;
                NewRequest.IsApproved = false;
                Models.Request _savedRequest =
                    this.RequestPersistence.AddOrUpdateRequest(NewRequest);
                new Threads.DelegatesAcceptenceThread(_savedRequest);

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.CrewMemberRegistedRequest;
                _jsonMessage.TriggerBy =_teamMember.Member.Name+" "+_teamMember.Member.LastName;
                _jsonMessage.Data = _savedRequest.Id.ToString();
                WebSockets.List.LogisticsWebSocketList.GetInstance();
                 WebSockets.List.LogisticsWebSocketList
                     .BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion

                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach (LogisticsDelegate _delegate in CurrentContext.LogisticDelegates)
                {
                    if (_delegate.Device != null)
                    {
                        _gcmPush.AddToken(_delegate.Device.Token);
                    }
                }
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.CrewMemberRegistedRequest.ToString(),
                    "", _crewMember.Id.ToString(), false);
                #endregion
            }
            catch (Exceptions.CrewMemberNotFoundException E)
            {
                throw E;
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException E)
            {
                throw E;
            }
        }

 
        /// <summary>
        /// This method allows each crew member accept each request that he is involve to
        /// by a logistic delegate
        /// </summary>
        /// <param name="TeamMemberId"></param>
        /// <param name="CrewMemberId"></param>
        public void CrewMemberAcceptRequest(long TeamMemberId ,long CrewMemberId)
        {
            try
            {
                Models.TeamMember _teamMember = this.TeamMemberPersistence.FindById(TeamMemberId);

                Models.CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);

                Models.Request _request = this.RequestPersistence.FindById(_teamMember.Request.Id);

                if (_crewMember == null)
                {
                    throw new Exceptions.CrewMemberNotFoundException();
                }

                if (_crewMember.Id == _teamMember.Id)
                {
                    throw new Exception("Request does not belongs to crew member");
                }
                if (_teamMember.CancelationReason != null)
                {
                    if (_teamMember.IsAccepted == false)
                    {

                        throw new Exception("Team member already canceld this request");
                    }
                }

                _teamMember.IsAccepted = true;
                _teamMember.Member = _crewMember;
                _teamMember.Request = _request;
                this.TeamMemberPersistence.AddOrUpdateTeamMember(_teamMember);

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.CrewMemberAcceptedRequest;
                _jsonMessage.TriggerBy = _crewMember.Name + " " + _crewMember.LastName;
                _jsonMessage.Data = _teamMember.Id.ToString();
                WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion

                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach (LogisticsDelegate _delegate in CurrentContext.LogisticDelegates)
                {
                    _gcmPush.AddToken(_delegate.Device.Token);
                }
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.CrewMemberAcceptedRequest.ToString(),""
                    , _teamMember.Id.ToString(), false);
                #endregion

            }
            catch (Exceptions.CrewMemberNotFoundException E)
            {
                throw E;
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        /// <summary>
        /// This method allows each crew member reject each request that he is involve to
        /// by a logistic delegate 
        /// </summary>
        /// <param name="TeamMemberId"></param>
        /// <param name="CrewMemberId"></param>
        /// <param name="CancelReason"></param>
        public void CrewMemberRejectRequest(long TeamMemberId, long CrewMemberId, String CancelReason)
        {
            try
            {
                Models.TeamMember _teamMember = this.TeamMemberPersistence.FindById(TeamMemberId);

                Models.CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);

                Models.Request _request = this.RequestPersistence.FindById(_teamMember.Request.Id);

                if (_crewMember == null)
                {
                    throw new Exceptions.CrewMemberNotFoundException();
                }

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
                _teamMember.CancelationReason = CancelReason;
                _teamMember.Member = _crewMember;
                _teamMember.Request = _request;
                this.TeamMemberPersistence.AddOrUpdateTeamMember(_teamMember);

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.CrewMemberRejectedRequest;
                _jsonMessage.TriggerBy = _crewMember.Name + " " + _crewMember.LastName; ;
                _jsonMessage.Data = _teamMember.CancelationReason;
                WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion

                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach (LogisticsDelegate _delegate in CurrentContext.LogisticDelegates)
                {
                    _gcmPush.AddToken(_delegate.Device.Token);
                }
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.CrewMemberRejectedRequest.ToString(),
                    "", _teamMember.Id.ToString(), false);
                #endregion
            }
            catch (Exceptions.CrewMemberNotFoundException E)
            {
                throw E;
            }
            catch (Exception E)
            {
                throw E;
            }
           
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="TeamMemberId"></param>
       /// <param name="CrewMemberId"></param>
       /// <param name="ModifiedTeamMember"></param>
        public void CrewMemberModifyRequest
            (long TeamMemberId, long CrewMemberId,TeamMember ModifiedTeamMember)
        {
            try
            {
                Models.TeamMember _teamMember = this.TeamMemberPersistence.FindById(TeamMemberId);

                Models.CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);

                Models.Request _request = this.RequestPersistence.FindById(_teamMember.Request.Id);

                if (_crewMember == null)
                {
                    throw new Exceptions.CrewMemberNotFoundException();
                }

                if (_crewMember.Id == _teamMember.Id)
                {
                    throw new Exception("Request does not belong to crew member");
                }
                if (!(DateTime.Now.AddMinutes(15) < _teamMember.Request.RequestDate))
                {
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
                _teamMember.Member = _crewMember;
                _teamMember.Request = _request;
                this.TeamMemberPersistence.AddOrUpdateTeamMember(_teamMember);

                #region Web socket message to logistic delegate
                JsonObject _jsonMessage = new JsonObject();
                _jsonMessage.Event = JsonObject.ServerEvent.CrewMemberModifiedRequest;
                _jsonMessage.TriggerBy = _crewMember.Name+" "+_crewMember.LastName;
                _jsonMessage.Data = _teamMember.Id.ToString();
                WebSockets.List.LogisticsWebSocketList.BroadCast(JsonConvert.SerializeObject(_jsonMessage));
                #endregion

                #region Gcm Push message to logistics delegates
                Pushs.Push _gcmPush = Pushs.PushFactory.GetGcmPushSender();
                foreach (LogisticsDelegate _delegate in CurrentContext.LogisticDelegates)
                {
                    _gcmPush.AddToken(_delegate.Device.Token);
                }
                _gcmPush.SendToUser
                    (JsonObject.ServerEvent.CrewMemberModifiedRequest.ToString(),""
                    , _teamMember.Id.ToString(), false);
                #endregion
            }catch (Exceptions.CrewMemberNotFoundException E)
            {
                throw E;
            }
            catch (Exception E)
            {
                throw E;
            }
        
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="CrewMemberId"></param>
        /// <returns></returns>
        public List<Models.Request> FindPendignTeamMember(long CrewMemberId)
        {

            CrewMember _selectedCrewMember = this.CrewMemberPersistence.FindById(CrewMemberId);
            IQueryable<Request> SelectedRequest= 
                this.RequestPersistence.GetPendingTeamMemberRequest(_selectedCrewMember);
            return (SelectedRequest!=null) ? SelectedRequest.ToList() : new List<Request>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Models.Request> FindPendingRequest() {
            return this.RequestPersistence.GetPendingRequest().ToList(); 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CrewMemberId"></param>
        /// <returns></returns>
        public List<Models.Request> FindNextTeamMember(long CrewMemberId)
        {

            CrewMember _selectedCrewMember = this.CrewMemberPersistence.FindById(CrewMemberId);
            return this.RequestPersistence.GetTeamMemberNextRequests(_selectedCrewMember);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Models.Request> FindNextRequest()
        {
            return this.RequestPersistence.GetNextRequests().ToList();
        }
    
    
    
    
    }
}