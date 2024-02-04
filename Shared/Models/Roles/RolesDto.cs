namespace Shared.Models.Roles
{
    public class RolesDto
    {
        public string Name { get; set; } = string.Empty;

        public static RolesDto Create(string name)
        {
            return new RolesDto { Name = name };
        }


        public static RolesDto RegularUser = RolesDto.Create("RegularUser");
        public static RolesDto ViewerUser = RolesDto.Create("ViewerUser");

        public static List<RolesDto> Roles = new List<RolesDto>()
        {
            RegularUser, ViewerUser
        };
    }
}
