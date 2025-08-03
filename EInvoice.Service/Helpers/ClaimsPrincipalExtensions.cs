using System.Security.Claims;

namespace EInvoice.Service.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static long? GetOrganizationId(this ClaimsPrincipal user)
        {
            var orgIdClaim = user.FindFirstValue("OrganizationId");
            return long.TryParse(orgIdClaim, out var orgId) ? orgId : null;
        }
    }
}
