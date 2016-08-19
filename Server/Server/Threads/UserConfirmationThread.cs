using Server.Models;
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
                using (var _context = new Context())
                {
                    IQueryable<Models.CrewMember> _selectedCrewMember =
                                              from _crewMember in _context.CrewMembers
                                              where _crewMember.ConfirmationLimit < DateTime.Now
                                              select _crewMember;

                    foreach (CrewMember _deletedCrewMember in _selectedCrewMember)
                    {
                        _context.CrewMembers.Remove(_deletedCrewMember);
                    }
                    _context.SaveChanges();
                }
            }
        }
    }
}