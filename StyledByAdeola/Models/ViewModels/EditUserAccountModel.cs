using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StyledByAdeola.Models.ViewModels
{
    public class EditUserAccountModel
    {
        public AppUser AppUser { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

    }
}
