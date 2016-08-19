using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace Server.Models
{
    public  class User
    {
        private String UserMail{set;get;}

        private String UserPassword { set; get; }

        private DateTime? UserResetPasswordLimit { set; get; }

        private DateTime? UserConfirmationLimit { set; get; }

        private String UserResetPasswordId { set; get; }

        private String UserConfirmAccountId { set; get; }
        

        /// <summary>
        /// The regular expresion checks if the Email is in a valid format
        /// </summary>
        [Required]
        public String Email { 
            set {
                Regex _Regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (_Regex.Match(value).Success)
                {
                    this.UserMail = value;
                }
                else
                {
                    throw new Exceptions.InvalidEmailException();
                }
        
            }
            get {
                return this.UserMail;
            }
        }

        /// <summary>
        /// The Regular expresion checks if the password contains:
        /// At least one upper case english letter
        /// At least one lower case english letter
        /// At least one digit
        /// At least one special character
        /// Minimum 8 in length
        /// If the password fullfill all the conditions then it will be encrypted using sha256
        /// </summary>
        [Required]
        public String Password { 
            set {
                this.UserPassword = value;
            }

            get { return this.UserPassword; }
        }

        /// <summary>
        /// Sets the limit date when the user can reset his password
        /// </summary>
        public DateTime? ResetPasswordLimit{
            set
            {
                
                if (value != null)
                {
                    this.UserResetPasswordLimit = DateTime.Now.AddDays(1);
                }
                else
                {
                    this.UserResetPasswordLimit = null;
                }
            }
            get { return this.UserResetPasswordLimit; }
         }

        /// <summary>
        /// Sets the limit date when the user can confirm his account
        /// </summary>
        public DateTime? ConfirmationLimit{
            set{

                if (value != null)
                {

                    this.UserConfirmationLimit = DateTime.Now.AddDays(1);
                }
                else {
                    this.UserConfirmationLimit = null;
                }
            }
            get { return this.UserConfirmationLimit; }
        }

        /// <summary>
        /// Sets the uid for the user's password restore
        /// </summary>
        public String ResetPasswordId
        {
            set
            {

                if (value == null)
                {
                    this.UserResetPasswordId = null;
                }
                else
                {
                    this.UserResetPasswordId = System.Guid.NewGuid().ToString();
                }
            }
            get { return this.UserResetPasswordId; }
         }

        /// <summary>
        /// This attribute represents the code for the user to confirm his account
        /// </summary>
        public String ConfirmAccountId {
            set {
                if (value != null)
                {
                    this.UserConfirmAccountId = System.Guid.NewGuid().ToString();
                }
                else
                {
                    this.UserConfirmAccountId = null;
                }
            }
            get {
                return this.UserConfirmAccountId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Device> Devices { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public User()
        {
            this.ConfirmAccountId = "";
            this.ConfirmationLimit = DateTime.Now;
            this.Devices = new HashSet<Device>();
        }


        /// <summary>
        /// Encrypts a Password string using Sha 256 algorithm
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public void CheckAndEncryptPassword() {

            Regex _Regex = new Regex(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if (_Regex.Match(this.Password).Success)
            {
                HashAlgorithm _hash = new SHA256Managed();
                byte[] _plainTextBytes =
                    System.Text.Encoding.UTF8.GetBytes(this.Password);
                byte[] hashBytes =
                    _hash.ComputeHash(_plainTextBytes);
                this.UserPassword=Convert.ToBase64String(hashBytes);
            }
            else
            {
                throw new Exceptions.InvalidPasswordException();
            }
            
       }

        /// <summary>
        /// 
        /// </summary>
        public void SendConfirmationEmail()
        {
            
            String _confirmAccountBody = 
                String.Format(ModelResources.ConfirmationAccountBody,this.ConfirmAccountId);
            this.SendEmail(ModelResources.ConfirmationAccountSubject,_confirmAccountBody);
        
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendResetPasswordEmail()
        {
            String _resetPasswordBody =
                String.Format(ModelResources.ResetPasswordBody, this.ResetPasswordId);
            this.SendEmail(ModelResources.ConfirmationAccountSubject, _resetPasswordBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NewPassword"></param>
        public void RestorePassword(String NewPassword)
        {

            if (DateTime.Now > this.ResetPasswordLimit)
            {
                this.Password = NewPassword;
                this.ResetPasswordId = null;
                this.ResetPasswordLimit = null;
            }
            else {
                this.ResetPasswordId = null;
                this.ResetPasswordLimit = null;
                throw new Exceptions.RestorePasswordRequstHasExpiredException();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        private void SendEmail(String Subject, String Body)
        {
            MailMessage _mail = new MailMessage();
            SmtpClient _smtpServer = new SmtpClient("smtp.gmail.com");
            _smtpServer.Port = 587;
            _smtpServer.Credentials = new System.Net.NetworkCredential(ModelResources.Email,
                ModelResources.Password);
            _smtpServer.EnableSsl = true;
            _mail.From = new MailAddress("enfermeriacsap@gmail.com");
            _mail.To.Add(this.UserMail);
            _mail.Subject = Subject;
            _mail.IsBodyHtml = true;
            _mail.Body = Body;
            _smtpServer.Send(_mail);
         
        }
    }
}