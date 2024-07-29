using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Linq;
using System.Threading.Tasks;

namespace HybridMvcNetFramework481
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "cookie",
                CookieName = "HybridMvcNetFramework481",
                ExpireTimeSpan = System.TimeSpan.FromDays(1),
                SlidingExpiration = true,
                CookieSecure = CookieSecureOption.SameAsRequest,
                CookieHttpOnly = true,
                CookieSameSite = Microsoft.Owin.SameSiteMode.None
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "", // client id
                Authority = "", // idp login url, e.g. "https://login.com",
                RedirectUri = "http://localhost:5001/signin-oidc", // redirect uri
                ResponseType = "code id_token",
                Scope = "openid profile email offline_access",

                SignInAsAuthenticationType = "cookie",

                UseTokenLifetime = false,

                RedeemCode = true,
                SaveTokens = true,
                ClientSecret = "", // client secret
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = (context) =>
                    {
                        var identity = context.AuthenticationTicket.Identity;
                        var name = identity.Claims.FirstOrDefault(c => c.Type == identity.NameClaimType)?.Value;

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
