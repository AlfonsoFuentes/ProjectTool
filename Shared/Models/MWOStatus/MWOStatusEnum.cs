namespace Shared.Models.MWOStatus
{
    public class MWOStatusEnum
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public static MWOStatusEnum Create(int id, string name) => new MWOStatusEnum() { Id = id, Name = name };
        static MWOStatusEnum Created = Create(0, "Created");
        static MWOStatusEnum Approved = Create(1, "Approved");
        static MWOStatusEnum Closed = Create(2, "Closed");
        public static List<MWOStatusEnum> List = new List<MWOStatusEnum>()
            {
            Created,Approved, Closed
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;
    }
}
