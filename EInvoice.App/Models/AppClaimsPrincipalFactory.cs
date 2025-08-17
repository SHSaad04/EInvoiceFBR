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

            bool isAssociated = user.OrganizationId != null;
            identity.AddClaim(new Claim("IsOrganizationAssociated", isAssociated.ToString().ToLower()));

            if (isAssociated)
            {
                identity.AddClaim(new Claim("OrganizationId", user.OrganizationId.ToString()));
            }

            return identity;
        }

    }
}