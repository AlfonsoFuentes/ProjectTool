namespace Shared.Models.UserAccounts.Registers
{
    public class RegisterUserRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = "RegisterUserPassword123*";
        public string Role {  get; set; }=string.Empty; 
    }
}
