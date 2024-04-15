using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.UserAccounts.Logins
{
    public class LoginUserRequest
    {
       
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public bool PasswordConfirmed { get; set; } = false;

       
       
    }
    
}
