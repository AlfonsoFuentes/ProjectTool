using System.ComponentModel.DataAnnotations;

namespace Shared.Models.ChangePasswords
{
    public class ChangePasswordRequest
    {

        [EmailAddress]
        public string Email { get; set; } = "";

        public string OldPassword { get; set; } = "DefaultPassword123456*";
        public string Password { get; set; } = "";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
