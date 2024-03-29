using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.UserAccounts.Logins
{
    public class LoginUserRequest
    {
        public Func<Task<bool>>? Validator { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public bool PasswordConfirmed { get; set; } = false;

        public LoginUserRequestDto ConvertToDto()
        {
            return new()
            {
                Password = Password,
                Email = Email,
            };
        }
        public async Task ChangeEmail(string email)
        {
            Email = email;
            if (Validator != null) { await Validator(); }
        }
        public async Task ChangePassword(string password)
        {
            Password = password;
            if (Validator != null) { await Validator(); }
        }
        public void ChangeNewPassword(string password)
        {
            Password = password;
            
        }
    }
    public class LoginUserRequestDto
    {

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
