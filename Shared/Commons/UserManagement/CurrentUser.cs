using Shared.Models.Roles;
using System.Linq;

namespace Shared.Commons.UserManagement
{
    public class CurrentUser
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public bool IsSuperAdmin => Role.Contains("SuperAdmin");
        public bool IsViewer => Role.Contains(RolesDto.ViewerUser.Name);
        public bool IsRegularUser => Role.Contains(RolesDto.RegularUser.Name) || Role.Contains("SuperAdmin");
    }
}