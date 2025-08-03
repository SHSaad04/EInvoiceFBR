using EInvoice.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Linq; // Add this using directive
using System.Threading.Tasks;

namespace EInvoice.App.Models
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Get all claims first
            var claims = await UserManager.GetClaimsAsync(user);

            // Check for organization association
            var isAssociated = claims.Any(c => c.Type == "IsOrganizationAssociated");

            identity.AddClaim(new Claim("IsOrganizationAssociated",
                isAssociated.ToString().ToLower()));

            // Add organization ID if exists
            var orgIdClaim = claims.FirstOrDefault(c => c.Type == "OrganizationId");
            if (orgIdClaim != null)
            {
                identity.AddClaim(orgIdClaim);
            }

            return identity;
        }
    }
}