using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Server.Logics
{
    public class CrewMemberLogic
    {
        private Persistences.CrewMemberPersistence CrewMemberPersistence;
        public CrewMemberLogic()
        {
            this.CrewMemberPersistence = 
                new Persistences.CrewMemberPersistence();
        }

        public void AddCrewMember(Models.CrewMember NewCrewMember) {

            if (CrewMemberPersistence.FindCrewMemberByEmail(NewCrewMember.Email)!=null) {
                throw new Exception();
            }
            NewCrewMember.CheckAndEncryptPassword();
            Models.CrewMember _savedCrewMember=
                this.CrewMemberPersistence.AddOrUpdateCrewMember(NewCrewMember);
           Thread _workerThread = new Thread( _savedCrewMember.SendConfirmationEmail);
           _workerThread.Start();
           
        }


        public void ConfirmCrewMemberAccount(String ConfirmationId) {
            Models.CrewMember _confirmedCrewMember = 
                this.CrewMemberPersistence.FindCrewMemberByConfirmationId(ConfirmationId);

            if (_confirmedCrewMember == null)
            {
                throw new Exception("No exite");
            }
            
            if (DateTime.Now > _confirmedCrewMember.ConfirmationLimit) {
                throw new Exception("Se paso");
            }
            _confirmedCrewMember.ConfirmAccountId = null;
            _confirmedCrewMember.ConfirmationLimit = null;
            this.CrewMemberPersistence.AddOrUpdateCrewMember(_confirmedCrewMember);

        }

        public void ResetPassword(Models.CrewMember ResetPasswordCrewMember,String ResetPasswordId) {
            Models.CrewMember _resetPasswordCrewMember =
                this.CrewMemberPersistence.FindCrewMemberByResetPassworId(ResetPasswordId);

            if (_resetPasswordCrewMember == null)
            {
                throw new Exception("No exite");
            }

            if (DateTime.Now > _resetPasswordCrewMember.ResetPasswordLimit)
            {
                throw new Exception("Se paso");
            }
            _resetPasswordCrewMember.Password = ResetPasswordCrewMember.Password;
            _resetPasswordCrewMember.ResetPasswordId = null;
            _resetPasswordCrewMember.ResetPasswordLimit = null;
            _resetPasswordCrewMember.CheckAndEncryptPassword();
            this.CrewMemberPersistence.AddOrUpdateCrewMember(_resetPasswordCrewMember);
        }



        public void RequestResetPassword(String Email) {
            Models.CrewMember _requestResetPasswordCrewMember = 
                this.CrewMemberPersistence.FindCrewMemberByEmail(Email);
            if (_requestResetPasswordCrewMember == null)
            {
                throw new Exception("No exite");
            }

            _requestResetPasswordCrewMember.ResetPasswordId = "";
            _requestResetPasswordCrewMember.ResetPasswordLimit = DateTime.Now;
            CrewMemberPersistence.AddOrUpdateCrewMember(_requestResetPasswordCrewMember);
            Thread _workerThread = new Thread(_requestResetPasswordCrewMember.SendResetPasswordEmail);
            _workerThread.Start();

        }




        public void UpdateCrewMember(int id,Models.CrewMember Data) {
            Models.CrewMember _updateCrewMember =
                 this.CrewMemberPersistence.FindById(id);
            if (_updateCrewMember == null)
            {
                throw new Exception("No exite");
            }
            if (Data.Name == null) { 
                throw new Exception("Nombre no puede ser null");
            }

            if (Data.LastName == null) {
                 throw new Exception("Apellido no puede ser null");
            }

            _updateCrewMember.Name = Data.Name;
            _updateCrewMember.LastName = Data.LastName;
            _updateCrewMember.Password = Data.Password;
            _updateCrewMember.CheckAndEncryptPassword();
            this.CrewMemberPersistence.AddOrUpdateCrewMember(_updateCrewMember);
        }


    }
}