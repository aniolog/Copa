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

        public void GetPendingTeamMember(long CrewMemberId) {
            this.CurrentContext.Configuration.ProxyCreationEnabled = false;

        }
    }
}