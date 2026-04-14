using System.Security.Claims;

namespace LogiPulse.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    private const string OidClaimKey = "http://schemas.microsoft.com/identity/claims/objectidentifier";
    
    extension(ClaimsPrincipal user)
    {
        public Guid GetObjectId()
        {
            var oid = user.FindFirst(OidClaimKey)?.Value;
            return Guid.TryParse(oid, out var guid) ? guid : Guid.Empty;
        }

        public string GetEmail()
        {
            return user.Identity?.Name ?? string.Empty;
        }
    }
}