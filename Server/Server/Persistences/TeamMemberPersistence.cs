using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class TeamMemberPersistence:BasePersistence
    {

        public TeamMemberPersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }


        public Models.TeamMember FindById(long TeamMemberId) {
            try
            {
                return this.CurrentContext.TeamMembers.Find(TeamMemberId);

            }
            catch(System.InvalidOperationException E)
            {
                return null;
            }
        }

        public Models.TeamMember AddOrUpdateRequest(Models.TeamMember TeamMember)
        {
            if (TeamMember.Id == 0)
            {
                CurrentContext.TeamMembers.Add(TeamMember);
                CurrentContext.SaveChanges();
                CurrentContext.Entry(TeamMember).GetDatabaseValues();
            }
            else
            {
                CurrentContext.SaveChanges();


            }
            return TeamMember;

        }
    }
}