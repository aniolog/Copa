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


        /// <summary>
        /// The regular expresion checks if the Email is in a valid format
        /// </summary>
        [Required]
        public String UserEmail { 
            set {
                Regex _Regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (_Regex.Match(value).Success)
                {
                    UserEmail = value;
                }
                else
                {
                    throw new Exceptions.InvalidEmailException();
                }
        
            }
            get { 
                return UserEmail;
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
                Regex _Regex = new Regex(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
                if (_Regex.Match(value).Success) {
                    Password = EncryptPassword(value);
                    this.ConfirmAccountId = null;
                }
                else
                {
                    throw new Exceptions.InvalidPasswordException();
                }
                
           
            }

            get { return Password; }
        }

        /// <summary>
        /// Sets the limit date when the user can reset his password
        /// </summary>
        public DateTime? ResetPasswordLimit{
            set
            {
                
                if (value != null)
                {
                    ResetPasswordLimit = DateTime.Now.AddDays(1);
                }
            }
            get{return ResetPasswordLimit;}
         }

        /// <summary>
        /// Sets the limit date when the user can confirm his account
        /// </summary>
        public DateTime? ConfirmationLimit{
            set{
                
                if (value != null) {
                
                    ConfirmationLimit = DateTime.Now.AddDays(1);
                }
            }
            get { return ConfirmationLimit; }
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
                    this.ResetPasswordId = null;
                }
                else
                {
                    this.ResetPasswordId = System.Guid.NewGuid().ToString();
                }
            }
            get { return ResetPasswordId; }
         }

        /// <summary>
        /// Sets the uid for the user's account confirmation
        /// </summary>
        public String ConfirmAccountId
        {
            set
            {

                if (value == null)
                {
                    this.ConfirmAccountId = null;
                }
                else
                {
                    this.ConfirmAccountId = System.Guid.NewGuid().ToString();
                }
            }
            get { return ResetPasswordId; }
        }

        /// <summary>
        /// Class constructor which sets the confirmation UId
        /// </summary>
        public User()
        {
            this.ConfirmAccountId = "";
        }

        /// <summary>
        /// Encrypts a Password string using Sha 256 algorithm
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static String EncryptPassword(String Password) {
            HashAlgorithm _hash = new SHA256Managed();
            byte[] _plainTextBytes =
                System.Text.Encoding.UTF8.GetBytes(Password);
            byte[] hashBytes =
                _hash.ComputeHash(_plainTextBytes);
            return Convert.ToBase64String(hashBytes);
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
            _mail.To.Add(this.UserEmail);
            _mail.Subject = Subject;
            _mail.IsBodyHtml = true;
            _mail.Body = "";
            _smtpServer.Send(_mail);
         
        }
    }
}