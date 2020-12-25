using System.ComponentModel.DataAnnotations;

namespace StyledByAdeola.Models.ViewModels
{
    public class LoginModel
    {
        public string Name { get; set; }

        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";

        //[StringLength(10, ErrorMessage = "Identity Provider field manupulated")]
        public string IdentityProvider { get; set; }
    }
}