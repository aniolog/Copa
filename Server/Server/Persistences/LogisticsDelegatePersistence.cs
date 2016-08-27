using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class LogisticsDelegatePersistence:BasePersistence
    {
        

        public LogisticsDelegatePersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }

        /// <summary>
        /// this method finds a logistics delegate with the given pass confirmation id
        /// </summary>
        /// <param name="ConfirmationId"></param>
        /// <returns>Logistics delegate  that uses the given confirmation id</returns>
        public Models.LogisticsDelegate FindLogisticsDelegateByConfirmationId(string ConfirmationId)
        {
            try
            {
                IQueryable<Models.LogisticsDelegate> _selectedLogisticsDelegate =
                                          from _LogisticsDelegate in CurrentContext.LogisticDelegates
                                          where _LogisticsDelegate.ConfirmAccountId == ConfirmationId
                                          select _LogisticsDelegate;

                return _selectedLogisticsDelegate.First();
            }
            catch (System.InvalidOperationException E)
            {
                throw new Exceptions.LogisticDelegateNotFoundException();
            }
        }


        /// <summary>
        /// this method finds a logistics delegate with the given reset password id
        /// </summary>
        /// <param name="ResetPassworId"></param>
        /// <returns></returns>
        public Models.LogisticsDelegate FindLogisticsDelegateByResetPassworId(string ResetPassworId)
        {
            try
            {
                IQueryable<Models.LogisticsDelegate> _selectedLogisticsDelegate =
                                          from _logisticsDelegate in CurrentContext.LogisticDelegates
                                          where _logisticsDelegate.ResetPasswordId == ResetPassworId
                                          select _logisticsDelegate;

                return _selectedLogisticsDelegate.First();
            }
            catch (System.InvalidOperationException E)
            {

                throw new Exceptions.LogisticDelegateNotFoundException();

            }
        }

        /// <summary>
        /// this method finds the Logistics delegate with the given pass email
        /// </summary>
        /// <param name="LogisticsDelegateEmail"></param>
        /// <returns>Logistics delegate that uses the given email</returns>
        public Models.LogisticsDelegate FindLogisticsDelegateByEmail(string LogisticsDelegateEmail)
        {
            try
            {
                IQueryable<Models.LogisticsDelegate> _selectedLogisticsDelegate =
                                          from _logisticsDelegate in CurrentContext.LogisticDelegates
                                          where _logisticsDelegate.Email == LogisticsDelegateEmail
                                          select _logisticsDelegate;

                return _selectedLogisticsDelegate.First();
            }
            catch (System.InvalidOperationException E)
            {

                return null; 

            }
        }


        /// <summary>
        /// Finds all logistics delegates that has not confirm their accounts in the pass 24 hours
        /// </summary>
        /// <returns>All logistics delegates that has not confirm their accounts</returns>
        public IQueryable<Models.LogisticsDelegate> FindExpireLogisticsDelegateConfirmation()
        {
            try
            {
                IQueryable<Models.LogisticsDelegate> _selectedLogisticsDelegate =
                                          from _logisticsDelegate in CurrentContext.LogisticDelegates
                                          where _logisticsDelegate.ConfirmationLimit < DateTime.Now
                                          select _logisticsDelegate;

                return _selectedLogisticsDelegate;
            }
            catch (System.InvalidOperationException E)
            {
                throw new Exceptions.LogisticDelegateNotFoundException();
            }
        }

        /// <summary>
        /// This method saves or updates the given logistics delegate
        /// </summary>
        /// <param name="LogisticsDelegate"></param>
        /// <returns>the logistics delegate after being saved or updated</returns>
        public Models.LogisticsDelegate AddOrUpdateLogisticsDelegate(Models.LogisticsDelegate LogisticsDelegate)
        {
            if (LogisticsDelegate.Id == 0)
            {
                CurrentContext.LogisticDelegates.Add(LogisticsDelegate);
                CurrentContext.SaveChanges();
                CurrentContext.Entry(LogisticsDelegate).GetDatabaseValues();
            }
            else
            {
                CurrentContext.SaveChanges();
                
             
            }
            return LogisticsDelegate;

        }


        /// <summary>
        /// This method finds all logistics delegates
        /// </summary>
        /// <returns>All logistics delegates</returns>
        public IQueryable<Models.LogisticsDelegate> FindAll()
        {
            return CurrentContext.LogisticDelegates;
        }


        /// <summary>
        /// This method finds a logistics delegate that has the given id
        /// </summary>
        /// <param name="LogisticsDelegateId"></param>
        /// <returns>The logistics delegate with the given id</returns>
        public Models.LogisticsDelegate FindById(long LogisticsDelegateId)
        {
            try
            {
                return CurrentContext.LogisticDelegates.Find(LogisticsDelegateId);
            }
            catch (Exception E) {

                throw new Exceptions.LogisticDelegateNotFoundException();

            }
        }

        /// <summary>
        /// Deletes a list of logistics delegates
        /// </summary>
        /// <param name="DeleteLogisticsDelegate"></param>
        public void DeleteLogisticsDelegate(IQueryable<Models.LogisticsDelegate> LogisticsDelegates)
        {
            foreach (Models.LogisticsDelegate _deletedLogisticsDelegate in LogisticsDelegates)
            {
                CurrentContext.LogisticDelegates.Remove(_deletedLogisticsDelegate);
            }
            CurrentContext.SaveChanges();
        }


    }
}