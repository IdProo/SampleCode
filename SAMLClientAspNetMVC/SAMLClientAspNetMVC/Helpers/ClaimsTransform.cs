using ITfoxtec.Identity.Saml2.Claims;
using System.Security.Claims;

namespace SAMLClientAspNetMVC.Helpers
{
    public static class ClaimsTransform
    {
        public static ClaimsPrincipal Transform(ClaimsPrincipal incomingPrincipal)
        {
            if (!(incomingPrincipal.Identity?.IsAuthenticated ?? false))
            {
                return incomingPrincipal;
            }

            return CreateClaimsPrincipal(incomingPrincipal);
        }

        private static ClaimsPrincipal CreateClaimsPrincipal(ClaimsPrincipal incomingPrincipal)
        {
            var claims = new List<Claim>();

            // All claims
            claims.AddRange(incomingPrincipal.Claims);

            // Or custom claims
            //claims.AddRange(GetSaml2LogoutClaims(incomingPrincipal));
            //claims.Add(new Claim(ClaimTypes.NameIdentifier, GetClaimValue(incomingPrincipal, ClaimTypes.NameIdentifier)));

            return new ClaimsPrincipal(new ClaimsIdentity(claims, incomingPrincipal.Identity?.AuthenticationType, ClaimTypes.NameIdentifier, ClaimTypes.Role)
            {
                BootstrapContext = ((ClaimsIdentity?)incomingPrincipal.Identity)?.BootstrapContext
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private static IEnumerable<Claim?> GetSaml2LogoutClaims(ClaimsPrincipal principal)
        {
            yield return GetClaim(principal, Saml2ClaimTypes.NameId);
            yield return GetClaim(principal, Saml2ClaimTypes.NameIdFormat);
            yield return GetClaim(principal, Saml2ClaimTypes.NameQualifier);
            yield return GetClaim(principal, Saml2ClaimTypes.SPNameQualifier);
            yield return GetClaim(principal, Saml2ClaimTypes.SessionIndex);
        }

        private static Claim? GetClaim(ClaimsPrincipal principal, string claimType)
        {
            return ((ClaimsIdentity?)principal.Identity)?.Claims.Where(c => c.Type == claimType).FirstOrDefault();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private static string? GetClaimValue(ClaimsPrincipal principal, string claimType)
        {
            var claim = GetClaim(principal, claimType);
            return claim?.Value;
        }
    }
}
