using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Logics
{
    public class TeamMemberLogic:BaseLogic
    {
        Persistences.TeamMemberPersistence TeamMemberPersistence;
        public TeamMemberLogic():base()
        {
            this.TeamMemberPersistence = 
                new Persistences.TeamMemberPersistence(this.CurrentContext);
        }

        public void UpdateTeamMember(int TeamMemberId,TeamMember UpdatedTeamMember) {
            TeamMember _currentTeamMember = this.TeamMemberPersistence.FindById(TeamMemberId);

            if (_currentTeamMember.Request.RequestDate < DateTime.Now)
            {
                throw new Exceptions.RequestHasExpiredException();
            }

            _currentTeamMember.Lat = UpdatedTeamMember.Lat;
            _currentTeamMember.Long = UpdatedTeamMember.Long;

            this.TeamMemberPersistence.AddOrUpdateTeamMember(_currentTeamMember);
        }
    }
}