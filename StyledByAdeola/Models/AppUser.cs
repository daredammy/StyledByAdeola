using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StyledByAdeola.Models
{
    public enum Cities
    {
        None, London, Paris, Chicago
    }

    public enum QualificationLevels
    {
        None, Basic, Advanced
    }

    // an class that extends the IdentityUser class
    // IdentityUser class provides access to basic information about a user: the user’s name, e-mail, phone number, password hash, role memberships, and so on.
    public class AppUser : IdentityUser
    {
        public Cities City_ { get; set; }
        public QualificationLevels Qualifications { get; set; }

        public AppUser(string userName): base(userName)
        {

        }

        public AppUser()
        {

        }
        // no additional members are required                
        // for basic Identity installation       
    }
}
