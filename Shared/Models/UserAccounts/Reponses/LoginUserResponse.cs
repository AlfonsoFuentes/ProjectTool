namespace Shared.Models.UserAccounts.Reponses
{
    public class LoginUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
