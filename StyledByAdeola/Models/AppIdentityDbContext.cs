
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StyledByAdeola.Models;

namespace StyledByAdeola.Models
{
    // AppIdentityDbContext class is derived from IdentityDbContext, which provides Identity-specific features for Entity Framework Core.
    //I used the IdentityUser class, which is the built-in class used to represent users.
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options) {}
    }
}