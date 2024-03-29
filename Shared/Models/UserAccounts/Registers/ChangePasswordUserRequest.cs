namespace Shared.Models.UserAccounts.Registers
{
    public class ChangePasswordUserRequest
    {

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = "RegisterUserPassword123*";
        public string OldPassword {  get; set; } = string.Empty;

    }
}
