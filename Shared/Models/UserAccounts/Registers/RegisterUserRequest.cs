using Shared.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.UserAccounts.Registers
{
    public class RegisterUserRequest
    {
       
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = "RegisterUserPassword123*";
        public RolesEnum Role { get; set; } = RolesEnum.None;
       
       

    }
}
