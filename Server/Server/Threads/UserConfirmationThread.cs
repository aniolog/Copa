using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Server.Threads
{
    public class UserConfirmationThread
    {

        private Persistences.CrewMemberPersistence CrewMemberPersistence;

        /// <summary>
        /// the constructor instantiate the crew member persistence class and starts
        /// the confirmation check threads
        /// </summary>
        public UserConfirmationThread()
        {
            this.CrewMemberPersistence =
                new Persistences.CrewMemberPersistence();
            Thread _crewMemberThread = new Thread(this.DeleteNotConfirmAccounts);
            _crewMemberThread.Start();
        }

        /// <summary>
        /// This method deletes all crew members that has not confirm their accounts
        /// </summary>
        public void DeleteNotConfirmAccounts() {
            while (true)
            {
                Thread.Sleep(1000);
                IQueryable<Models.CrewMember> _crewMembers =
                    this.CrewMemberPersistence.FindExpireCrewMembersConfirmation();
                this.CrewMemberPersistence.DeleteCrewMember(_crewMembers);
            }
        }
    }
}