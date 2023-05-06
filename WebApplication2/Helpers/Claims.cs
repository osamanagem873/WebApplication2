using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebApplication2.Models;

namespace WebApplication2.Helpers
{
    public class Claims : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    {
        public Claims(UserManager<AppUser> userManager ,
            RoleManager<IdentityRole> roleManager , IOptions<IdentityOptions> options)
            :base(userManager,roleManager,options)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            return identity;
        }
    }
}
