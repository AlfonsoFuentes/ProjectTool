namespace Shared.Models.Roles
{
    public class RolesEnum
    {
        public string Name { get; set; } = string.Empty;

        public static RolesEnum Create(string name)
        {
            return new RolesEnum { Name = name };
        }
        public static RolesEnum None = RolesEnum.Create("None");

        public static RolesEnum RegularUser = RolesEnum.Create("Regular");
        public static RolesEnum ViewerUser = RolesEnum.Create("Viewer");

        public static List<RolesEnum> Roles = new List<RolesEnum>()
        {
            None, RegularUser, ViewerUser
        };
    }
}
