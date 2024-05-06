namespace Shared.Enums.Focuses
{
    public class FocusEnum
    {
        public override string ToString()
        {
            return Name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public static FocusEnum Create(int id, string name,string shortname) => new FocusEnum() { Id = id, Name = name,ShortName=shortname };

        public static FocusEnum None = Create(-1, "NONE","none");
        public static FocusEnum PCP = Create(0, "Personal Care","PCP");
        public static FocusEnum HC = Create(1, "Home Care", "HCL");
        public static FocusEnum OC = Create(2, "Oral Care", "OC");
        public static FocusEnum AxionPaste = Create(3, "Hand dish paste", "HDP");

        public static List<FocusEnum> List = new List<FocusEnum>()
            {
          None,  PCP, HC, OC, AxionPaste
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;

        public static FocusEnum GetType(string type) => List.Exists(x => x.Name == type) ? List.FirstOrDefault(x => x.Name == type)! : None;
        public static FocusEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : None;
    }
}
