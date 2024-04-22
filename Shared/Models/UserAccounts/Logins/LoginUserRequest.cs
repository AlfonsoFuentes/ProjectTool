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
