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
            Thread _crewMemberThread = new Thread(this.DeleteCrewMemberNotConfirmAccounts);
            _crewMemberThread.Start();

            Thread _delegateThread = new Thread(this.DeleteDelegateNotConfirmAccounts);
            _delegateThread.Start();



        }

        /// <summary>
        /// This method deletes all crew members that has not confirm their accounts
        /// </summary>
        public void DeleteCrewMemberNotConfirmAccounts() {
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

        /// <summary>
        /// This method deletes all logistics delegates that has not confirm their accounts
        /// </summary>
        public void DeleteDelegateNotConfirmAccounts()
        {
            while (true)
            {
                using (var _context = new Context())
                {
                    IQueryable<Models.LogisticsDelegate> _selectedDelegate =
                                              from _delegate in _context.LogisticDelegates
                                              where _delegate.ConfirmationLimit < DateTime.Now
                                              select _delegate;

                    foreach (LogisticsDelegate _deletedDelegate in _selectedDelegate)
                    {
                        _context.LogisticDelegates.Remove(_deletedDelegate);
                    }
                    _context.SaveChanges();
                }
            }
        }










    }
}