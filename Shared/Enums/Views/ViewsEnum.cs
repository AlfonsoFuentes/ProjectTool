namespace Shared.Enums.Views
{
    public class MWOViewsEnum
    {
        public override string ToString()
        {
            return Name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public static MWOViewsEnum Create(int id, string name) => new MWOViewsEnum() { Id = id, Name = name };

        public static MWOViewsEnum Table { get; set; } = Create(0, "Table");
        public static MWOViewsEnum DataList { get; set; } = Create(1, "Data List");
        public static List<MWOViewsEnum> List = new List<MWOViewsEnum>()
            {
        Table, DataList
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;
        public static MWOViewsEnum GetType(string type) => List.Exists(x => x.Name == type) ? List.FirstOrDefault(x => x.Name == type)! : Table;
        public static MWOViewsEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : Table;
    }
}
