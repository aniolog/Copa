using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin.WebSocket.Extensions;


[assembly: OwinStartup(typeof(Server.App_Start.Startup))]
namespace Server.App_Start
{
   
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token/crewmember"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
                Provider = new CrewMemberAuthorization()
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            
            OAuthAuthorizationServerOptions _logisticsOAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token/logisticdelegate"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
                Provider = new LogisticsAuthorizationProvider()
            };
            app.UseOAuthAuthorizationServer(_logisticsOAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            



            app.MapWebSocketPattern<WebSockets.Sockets.ProviderWebSocket>("/provider/(?<Id>.+)");
            app.MapWebSocketPattern<WebSockets.Sockets.LogisticsWebSocket>("/logistics/(?<Id>.+)");
            Threads.UserConfirmationThread _thread = new Threads.UserConfirmationThread();

        }

    }
}