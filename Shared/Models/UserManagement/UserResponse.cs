using Shared.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.UserManagement
{
    public class UserResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public bool IsSuperAdmin => Role.Contains("SuperAdmin");
        public bool IsViewer => Role.Contains(RolesEnum.ViewerUser.Name);
        public bool IsRegularUser => Role.Contains(RolesEnum.RegularUser.Name) || Role.Contains("SuperAdmin");
    }
    
}
