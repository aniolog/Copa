using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Server.Logics
{
    public class CrewMemberLogic:BaseLogic
    {
        private Persistences.CrewMemberPersistence CrewMemberPersistence;

        public CrewMemberLogic():base()
        {
            this.CrewMemberPersistence = 
                new Persistences.CrewMemberPersistence(this.CurrentContext);
        }

        public void AddCrewMember(Models.CrewMember NewCrewMember) {
            try
            {
                if (CrewMemberPersistence.FindCrewMemberByEmail(NewCrewMember.Email) != null)
                {
                    throw new Exceptions.EmailAlreadyInUse();
                }
                NewCrewMember.CheckAndEncryptPassword();
                NewCrewMember.CheckEmail();
                Models.CrewMember _savedCrewMember =
                    this.CrewMemberPersistence.AddOrUpdateCrewMember(NewCrewMember);
                Thread _workerThread = new Thread(_savedCrewMember.SendConfirmationEmail);
                _workerThread.Start();
            }
            catch (Exception E)
            {
                throw E;
            }
           
        }


        public void ConfirmCrewMemberAccount(String ConfirmationId) {
            try
            {
                Models.CrewMember _confirmedCrewMember =
                    this.CrewMemberPersistence.FindCrewMemberByConfirmationId(ConfirmationId);
                if (DateTime.Now > _confirmedCrewMember.ConfirmationLimit)
                {
                    throw new Exception("Se paso");
                }
                _confirmedCrewMember.ConfirmAccountId = null;
                _confirmedCrewMember.ConfirmationLimit = null;
                this.CrewMemberPersistence.AddOrUpdateCrewMember(_confirmedCrewMember);
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        public void ResetPassword(Models.CrewMember ResetPasswordCrewMember,String ResetPasswordId) {
            try
            {
                Models.CrewMember _resetPasswordCrewMember =
                    this.CrewMemberPersistence.FindCrewMemberByResetPassworId(ResetPasswordId);
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
            catch (Exception E)
            {
                throw E;
            }
        }



        public void RequestResetPassword(String Email) {
            try
            {
                Models.CrewMember _requestResetPasswordCrewMember =
                    this.CrewMemberPersistence.FindCrewMemberByEmail(Email);
                if (_requestResetPasswordCrewMember == null)
                {
                    throw new Exceptions.CrewMemberNotFoundException();
                }

                _requestResetPasswordCrewMember.ResetPasswordId = "";
                _requestResetPasswordCrewMember.ResetPasswordLimit = DateTime.Now;
                CrewMemberPersistence.AddOrUpdateCrewMember(_requestResetPasswordCrewMember);
                Thread _workerThread = new Thread(_requestResetPasswordCrewMember.SendResetPasswordEmail);
                _workerThread.Start();
            }
            catch (Exception E)
            {
                throw E;
            }

        }




        public void UpdateCrewMember(int id,Models.CrewMember Data) {
            try
            {
                Models.CrewMember _updateCrewMember =
                     this.CrewMemberPersistence.FindById(id);
                if (Data.Name == null)
                {
                    throw new Exception("Nombre no puede ser null");
                }

                if (Data.LastName == null)
                {
                    throw new Exception("Apellido no puede ser null");
                }

                _updateCrewMember.Name = Data.Name;
                _updateCrewMember.LastName = Data.LastName;
                _updateCrewMember.Password = Data.Password;
                _updateCrewMember.CheckAndEncryptPassword();
                this.CrewMemberPersistence.AddOrUpdateCrewMember(_updateCrewMember);
            }
            catch (Exception E)
            {
                throw E;
            }
        }


    }
}