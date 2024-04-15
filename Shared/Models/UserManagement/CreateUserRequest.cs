using System.ComponentModel.DataAnnotations;

namespace Shared.Models.UserManagement
{
    public class CreateUserRequest
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "DefaultPassword123456*";

        public string Role { get; set; } = string.Empty;
    }
}
