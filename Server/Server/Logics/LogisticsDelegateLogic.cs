using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Server.Logics
{
    public class LogisticsDelegateLogic:BaseLogic
    {
        private Persistences.LogisticsDelegatePersistence LogisticDelegatePersistence;

        public LogisticsDelegateLogic():base()
        {
            this.LogisticDelegatePersistence = new
                Persistences.LogisticsDelegatePersistence(this.CurrentContext);
        }

        public void AddLogisticDelegate(Models.LogisticsDelegate NewLogisticsDelegate)
        {

            if (LogisticDelegatePersistence
                .FindLogisticsDelegateByEmail(NewLogisticsDelegate.Email) != null)
            {
                throw new Exceptions.EmailAlreadyInUseException();
            }
            NewLogisticsDelegate.CheckEmail();
            NewLogisticsDelegate.CheckAndEncryptPassword();
            Models.LogisticsDelegate _savedLogisticDelegate =
                this.LogisticDelegatePersistence.AddOrUpdateLogisticsDelegate(NewLogisticsDelegate);
            Thread _workerThread = new Thread(_savedLogisticDelegate.SendConfirmationEmail);
            _workerThread.Start();

        }

        public void ConfirmLogisticDelegateAccount(String ConfirmationId)
        {
            try
            {
                Models.LogisticsDelegate _confirmedLogisticDelegate =
                    this.LogisticDelegatePersistence
                    .FindLogisticsDelegateByConfirmationId(ConfirmationId);


                if (DateTime.Now > _confirmedLogisticDelegate.ConfirmationLimit)
                {
                    throw new Exception("Se paso");
                }
                _confirmedLogisticDelegate.ConfirmAccountId = null;
                _confirmedLogisticDelegate.ConfirmationLimit = null;
                this.LogisticDelegatePersistence.AddOrUpdateLogisticsDelegate(_confirmedLogisticDelegate);
            }
            catch (Exception E) {
                throw E;
            }

        }

        public void RequestResetPassword(String Email)
        {
            try
            {
                Models.LogisticsDelegate _requestResetPasswordLogisticDelegate =
                    this.LogisticDelegatePersistence.FindLogisticsDelegateByEmail(Email);
                if (_requestResetPasswordLogisticDelegate == null)
                {
                    throw new Exceptions.LogisticDelegateNotFoundException();
                }
                _requestResetPasswordLogisticDelegate.ResetPasswordId = "";
                _requestResetPasswordLogisticDelegate.ResetPasswordLimit = DateTime.Now;
                this.LogisticDelegatePersistence.
                    AddOrUpdateLogisticsDelegate(_requestResetPasswordLogisticDelegate);
                Thread _workerThread = new Thread(_requestResetPasswordLogisticDelegate.SendResetPasswordEmail);
                _workerThread.Start();
            }
            catch (Exception E)
            {
                throw E;

            }

        }


        public void ResetPassword(Models.LogisticsDelegate ResetPasswordLogisticDelegate, String ResetPasswordId)
        {
            try
            {
                Models.LogisticsDelegate _resetPasswordDelegate =
                    this.LogisticDelegatePersistence.FindLogisticsDelegateByResetPassworId(ResetPasswordId);
                if (DateTime.Now > _resetPasswordDelegate.ResetPasswordLimit)
                {
                    throw new Exception("Se paso");
                }
                _resetPasswordDelegate.Password = ResetPasswordLogisticDelegate.Password;
                _resetPasswordDelegate.ResetPasswordId = null;
                _resetPasswordDelegate.ResetPasswordLimit = null;
                _resetPasswordDelegate.CheckAndEncryptPassword();
                this.LogisticDelegatePersistence.AddOrUpdateLogisticsDelegate(_resetPasswordDelegate);
            }
            catch (Exception E) {
                throw E;
            }
        }

        public void UpdateLogisticDelegate(int id, Models.LogisticsDelegate Data)
        {
            try
            {
                Models.LogisticsDelegate _updateLogisticDelegate =
                     this.LogisticDelegatePersistence.FindById(id);
                if (Data.Name == null)
                {
                    throw new Exception("Nombre no puede ser null");
                }

                if (Data.LastName == null)
                {
                    throw new Exception("Apellido no puede ser null");
                }

                _updateLogisticDelegate.Name = Data.Name;
                _updateLogisticDelegate.LastName = Data.LastName;
                _updateLogisticDelegate.Password = Data.Password;
                _updateLogisticDelegate.CheckAndEncryptPassword();
                this.LogisticDelegatePersistence.AddOrUpdateLogisticsDelegate(_updateLogisticDelegate);
            }
            catch (Exception E) {
                throw E;
            }
        }

        public void PromoteDelegate(int id)
        {
            try
            {
                Models.LogisticsDelegate _updateLogisticDelegate =
                     this.LogisticDelegatePersistence.FindById(id);
                _updateLogisticDelegate.IsAdmin = true;
                this.LogisticDelegatePersistence.AddOrUpdateLogisticsDelegate(_updateLogisticDelegate);
            }
            catch (Exception E) {
                throw E;
            }
        }



        public Boolean DelegateHasPermision(int DelegateId) {
            try
            {
                LogisticsDelegate _delegate = this.LogisticDelegatePersistence.FindById(DelegateId);

                if (!(_delegate.IsAdmin))
                {
                    throw new Exception("Denied");
                }
                return true;
            }
            catch (Exception E) {
                throw E;
            }
        }








    }
}