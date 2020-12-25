using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StyledByAdeola.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "damilola";
        private const string adminRole = "Admins";
        private const string UserRole = "Users";

        public static async Task EnsurePopulated(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, string email, string adminPassword)
        {
            AppUser user = await userManager.FindByIdAsync(adminUser);
            IdentityRole role = await roleManager.FindByNameAsync(adminRole);
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = adminUser,
                    Email = email,
                };
                IdentityResult result = await userManager.CreateAsync(user, adminPassword);
                //if (result.Succeeded) 
                //{
                //    if (role == null)
                //    {
                //        role = new IdentityRole(adminRole);
                //        await roleManager.CreateAsync(role);
                //        await userManager.AddToRoleAsync(user, adminRole);
                //    }
                //}
            }

        }
    }
}
