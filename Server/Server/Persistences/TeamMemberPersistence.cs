using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class TeamMemberPersistence
    {
        /// <summary>
        /// The Curent database context
        /// </summary>
        private Models.Context CurrentContext;

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
    }
}