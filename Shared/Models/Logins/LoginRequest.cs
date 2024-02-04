using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Logins
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        public string OldPassword { get; set; } = "DefaultPassword123456*";
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
