using IdentityModel.Client;

using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

using Owin;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Claims;

[assembly: OwinStartup(typeof(AspNetWebForms.Startup))]

namespace AspNetWebForms
{
    public class Startup
    {
        // These values are stored in Web.config. Make sure you update them!
        private readonly string _clientId = ConfigurationManager.AppSettings["IdAMan:ClientId"]; 
        private readonly string _redirectUri = ConfigurationManager.AppSettings["IdAMan:RedirectUri"];
        private readonly string _authority = ConfigurationManager.AppSettings["IdAMan:Authority"];
        private readonly string _clientSecret = ConfigurationManager.AppSettings["IdAMan:ClientSecret"];

        public void Configuration(IAppBuilder app)
        { 
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            { 
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                Authority = _authority,
                RedirectUri = _redirectUri,
                ResponseType = OpenIdConnectResponseType.CodeIdToken,
                Scope = "profile openid email api.auth user.role user.read user.readAll user.add user.edit application.read application.readAll",
                TokenValidationParameters = new TokenValidationParameters { NameClaimType = "name" }, 
                Notifications = new OpenIdConnectAuthenticationNotifications
                { 
                    AuthorizationCodeReceived = async n =>
                    {
                        // Exchange code for access and ID tokens
                        var tokenClient = new TokenClient($"{_authority}/connect/token", _clientId, _clientSecret);

                        var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(n.Code, _redirectUri);
                        if (tokenResponse.IsError)
                        {
                            throw new Exception(tokenResponse.Error);
                        }

                        var userInfoClient = new UserInfoClient($"{_authority}/connect/userinfo");
                        var userInfoResponse = await userInfoClient.GetAsync(tokenResponse.AccessToken);

                        var claims = new List<Claim>(userInfoResponse.Claims)
                        {
                            new Claim("id_token", tokenResponse.IdentityToken),
                            new Claim("access_token", tokenResponse.AccessToken)
                        };

                        n.AuthenticationTicket.Identity.AddClaims(claims);
                    },
                },
            });
        }
    }
}