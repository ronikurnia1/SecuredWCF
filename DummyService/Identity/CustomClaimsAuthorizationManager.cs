using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DummyService.Identity
{
    public class CustomClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        private const string ACTION_PREFIX = "http://tempuri.org/IMyService/";
        private const string ROLE_TYPE = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public override bool CheckAccess(AuthorizationContext context)
        {
            // Authentication check
            if (context.Principal.Identity.IsAuthenticated)
            {
                // Authorization check (role)
                var roles = GetRoles(context.Principal.Claims);
                return roles.Contains(context.Action[0].Value);
            }
            return false;
        }


        private string[] GetRoles(IEnumerable<Claim> claims)
        {
            var roleClaims = claims.Where(c => c.Type == ROLE_TYPE);
            if (roleClaims.Any())
            {
                var roles = string.Join(";", roleClaims.Select(c => c.Value)).Split(';');
                return roles.Select(r => $"{ACTION_PREFIX}{r}").ToArray();
            }
            return new List<string>().ToArray();
        }
    }
}