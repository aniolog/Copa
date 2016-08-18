using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Server.Logics
{
    public class LogisticsDelegateLogic
    {
        private Persistences.LogisticsDelegatePersistence LogisticDelegatePersistence;

        public LogisticsDelegateLogic()
        {
            this.LogisticDelegatePersistence = new 
                Persistences.LogisticsDelegatePersistence();
        }

        public void AddLogisticDelegate(Models.LogisticsDelegate NewLogisticsDelegate)
        {

            if (LogisticDelegatePersistence
                .FindLogisticsDelegateByEmail(NewLogisticsDelegate.Email) != null)
            {
                throw new Exception();
            }
            NewLogisticsDelegate.CheckAndEncryptPassword();
            Models.LogisticsDelegate _savedLogisticDelegate =
                this.LogisticDelegatePersistence.AddOrUpdateLogisticsDelegate(NewLogisticsDelegate);
            Thread _workerThread = new Thread(_savedLogisticDelegate.SendConfirmationEmail);
            _workerThread.Start();

        }

        public void ConfirmLogisticDelegateAccount(String ConfirmationId)
        {
            Models.LogisticsDelegate _confirmedLogisticDelegate =
                this.LogisticDelegatePersistence
                .FindLogisticsDelegateByConfirmationId(ConfirmationId);

            if (_confirmedLogisticDelegate == null)
            {
                throw new Exception("No exite");
            }

            if (DateTime.Now > _confirmedLogisticDelegate.ConfirmationLimit)
            {
                throw new Exception("Se paso");
            }
            _confirmedLogisticDelegate.ConfirmAccountId = null;
            _confirmedLogisticDelegate.ConfirmationLimit = null;
            this.LogisticDelegatePersistence.AddOrUpdateLogisticsDelegate(_confirmedLogisticDelegate);

        }

        public void RequestResetPassword(String Email)
        {
            Models.LogisticsDelegate _requestResetPasswordLogisticDelegate =
                this.LogisticDelegatePersistence.FindLogisticsDelegateByEmail(Email);
            if (_requestResetPasswordLogisticDelegate == null)
            {
                throw new Exception("No exite");
            }

            _requestResetPasswordLogisticDelegate.ResetPasswordId = "";
            _requestResetPasswordLogisticDelegate.ResetPasswordLimit = DateTime.Now;
            this.LogisticDelegatePersistence.
                AddOrUpdateLogisticsDelegate(_requestResetPasswordLogisticDelegate);
            Thread _workerThread = new Thread(_requestResetPasswordLogisticDelegate.SendResetPasswordEmail);
            _workerThread.Start();

        }


        public void ResetPassword(Models.LogisticsDelegate ResetPasswordLogisticDelegate, String ResetPasswordId)
        {
            Models.LogisticsDelegate _resetPasswordDelegate =
                this.LogisticDelegatePersistence.FindLogisticsDelegateByResetPassworId(ResetPasswordId);

            if (_resetPasswordDelegate == null)
            {
                throw new Exception("No exite");
            }

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


        public void UpdateLogisticDelegate(int id, Models.LogisticsDelegate Data)
        {
            Models.LogisticsDelegate _updateLogisticDelegate =
                 this.LogisticDelegatePersistence.FindById(id);
            if (_updateLogisticDelegate == null)
            {
                throw new Exception("No exite");
            }
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









    }
}