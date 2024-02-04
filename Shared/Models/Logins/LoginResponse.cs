namespace Shared.Models.Logins
{
    public class LoginResponse
    {
        public string Email { get; set; } = "Alfonso";
        public string Password { get; set; } = "1506";
        public Dictionary<string, string> Claims { get; set; } = [];
    }
}
