using Shared.Models.Roles;
using System.Linq;

namespace Shared.Commons.UserManagement
{
    public class CurrentUser
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; }=string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();

        public  bool IsSuperAdmin => Roles.Contains("SuperAdmin");
        public bool IsViewer => Roles.Contains(RolesDto.ViewerUser.Name);
        public bool IsRegularUser => Roles.Contains(RolesDto.RegularUser.Name)|| Roles.Contains("SuperAdmin");
    }
}