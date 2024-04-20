using Shared.Models.Roles;

namespace Shared.Models.UserAccounts.Reponses
{
    public class UserResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;


        public bool IsSuperAdmin => Role.Contains("Administrator");
        public bool IsViewer => Role.Contains(RolesEnum.ViewerUser.Name);

        public string Role => Roles == null || Roles.Count == 0 ? string.Empty : Roles.First();
        public bool IsEmailConfirmed { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public IList<string>? Roles { get; set; } = new List<string>();
    }
}
