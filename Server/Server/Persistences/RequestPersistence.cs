using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class RequestPersistence:BasePersistence
    {
    

        public RequestPersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }



        public Models.Request FindById(long RequestId) {

            try
            {
               return this.CurrentContext.Requests.Find(RequestId);
            }
            catch (Exception E) {
                throw new Exceptions.RequestNotFoundException();
            }
        
        }


        public Models.Request AddOrUpdateRequest(Models.Request Request)
        {
            if (Request.Id == 0)
            {
                CurrentContext.Requests.Add(Request);
                CurrentContext.SaveChanges();
                CurrentContext.Entry(Request).GetDatabaseValues();
            }
            else
            {
                CurrentContext.SaveChanges();


            }
            return Request;

        }


        public IQueryable<Request> GetPendingTeamMemberRequest(Models.CrewMember PendingCrewMember)
        {

            IQueryable<Request> _pendingRequest = from _pendingTeamMember 
                                                  in this.CurrentContext.TeamMembers
                                                  where _pendingTeamMember.Member.Id == PendingCrewMember.Id &&
                                                        _pendingTeamMember.IsAccepted == null &&
                                                        _pendingTeamMember.Request.IsApproved==true
                                                  select _pendingTeamMember.Request;

            return (_pendingRequest.Any())?_pendingRequest:null;
        
        }


        public IQueryable<Request> GetPendingRequest()
        {

            IQueryable<Request> _pendingRequest = from _selectedRequest in this.CurrentContext.Requests
                                                  where _selectedRequest.CancelReason == null &&
                                                  _selectedRequest.IsApproved == false
                                                  select _selectedRequest;

            return (_pendingRequest.Any()) ? _pendingRequest : null;

        }


        public IQueryable<Request> GetNextRequests() {

            IQueryable<Request> _nextRequests = from _selectedRequest in this.CurrentContext.Requests
                                                  where _selectedRequest.CancelReason == null &&
                                                  _selectedRequest.IsApproved == false &&
                                                  _selectedRequest.RequestDate>DateTime.Now
                                                  select _selectedRequest;

            return (_nextRequests.Any()) ? _nextRequests : null;

        }


        public List<Request> GetTeamMemberNextRequests(Models.CrewMember Member)
        {

            var _nextRequests = from _selectedTeamMember in Member.TeamMembers
                                                where _selectedTeamMember.IsAccepted == true &&
                                                _selectedTeamMember.Request.ApproveDelegate != null &&
                                                _selectedTeamMember.Request.IsApproved == true &&
                                                _selectedTeamMember.Request.RequestDate > DateTime.Now
                                                select _selectedTeamMember.Request;

            return (_nextRequests.Any()) ? _nextRequests.ToList() : null;

        }


    
    }
}