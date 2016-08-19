using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Server.App_Start
{
    public class LogisticsAuthorizationProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
 
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                Persistences.LogisticsDelegatePersistence _persistence
                    = new Persistences.LogisticsDelegatePersistence();

                Models.LogisticsDelegate _loginLogisticsDelegate =
                    _persistence.FindLogisticsDelegateByEmail(context.UserName);

                if (_loginLogisticsDelegate == null)
                {
                    throw new Exception();
                }

                Models.CrewMember _passwordValidator = new Models.CrewMember();
                _passwordValidator.Password = context.Password;
                _passwordValidator.CheckAndEncryptPassword();

                if (_loginLogisticsDelegate.Password == _passwordValidator.Password)
                {
                    if (_loginLogisticsDelegate.ConfirmAccountId == null)
                    {
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim(ClaimTypes.Name, _loginLogisticsDelegate.Id.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "logisticdelegate"));
                        context.Validated(identity);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception E)
            {
                context.SetError("invalid_grant", "The user name or password are incorrect.");
            }

        }
    }
}