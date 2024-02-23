using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Registers
{
    public class RegisterRequest
    {

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "DefaultPassword123456*";

        public string Role { get; set; } = string.Empty;
    }
}
