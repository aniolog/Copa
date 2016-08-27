using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Server.App_Start
{
    public class CrewMemberAuthorization : OAuthAuthorizationServerProvider
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

                Models.Context _currentContext = new Models.Context();
                Persistences.CrewMemberPersistence _persistence
                    = new Persistences.CrewMemberPersistence(_currentContext);

                Models.CrewMember _loginCrewMember = _persistence.FindCrewMemberByEmail(context.UserName);
                Models.CrewMember _passwordValidator = new Models.CrewMember();
                _passwordValidator.Password = context.Password;
                _passwordValidator.CheckAndEncryptPassword();

                if (_loginCrewMember.Password == _passwordValidator.Password)
                {
                    if (_loginCrewMember.ConfirmAccountId == null)
                    {
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim(ClaimTypes.Name, _loginCrewMember.Id.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "crewmember"));
                        context.Validated(identity);
                    }
                    else {
                        throw new Exception();
                    }
                }
                else {
                    throw new Exception();
                }
            }
            catch (Exception E) {
                context.SetError("invalid_grant", "The user name or password are incorrect.");
            }
        }
    }
}