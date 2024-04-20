using Shared.Models.Roles;

namespace Shared.Commons.Results
{
    //public interface ICurrentUser
    //{
    //    string UserId { get; set; }
    //    string UserName { get; set; }
    //    string Role { get; set; }
    //    bool IsSuperAdmin { get; }
    //    bool IsViewer { get; }
    //    bool IsRegularUser { get; }
    //}
    public class CurrentUser /*: ICurrentUser*/
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public bool IsSuperAdmin => Role.Contains("Administrator");
        public bool IsViewer => Role.Contains(RolesEnum.ViewerUser.Name);
        public bool IsRegularUser => Role.Contains(RolesEnum.RegularUser.Name) || Role.Contains("SuperAdmin");

    }
}
