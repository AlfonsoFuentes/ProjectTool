namespace Shared.Models.UserAccounts.Reponses
{
    public class RegisterUserResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = "RegisterUserPassword123*";

    }
}
