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
        public Func<Task<bool>>? Validator { get; set; } = null!;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = "RegisterUserPassword123*";
        public RolesEnum Role { get; set; } = RolesEnum.None;
        public async Task ChangeRole()
        {
            if (Validator != null) await Validator();
        }
        public async Task ChangeEmail(string email)
        {
            Email = email;
            if (Validator != null) await Validator();
        }
        public RegisterUserRequestDto ConvertToDto()
        {
            return new()
            {
                Email = Email,
                Password = Password,
                UserName = UserName,
                Role = Role.Name,

            };
        }

    }
}
