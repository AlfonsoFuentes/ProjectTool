namespace Shared.Models.PurchaseorderStatus
{
    public class PurchaseOrderStatusEnum
    {
        public override string ToString()
        {
            return Name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public static PurchaseOrderStatusEnum Create(int id, string name) => new PurchaseOrderStatusEnum() { Id = id, Name = name };

        public static PurchaseOrderStatusEnum None = Create(-1, "NONE");
        public static PurchaseOrderStatusEnum Created = Create(0, "Created");
        public static PurchaseOrderStatusEnum Approved = Create(1, "Approved");
        public static PurchaseOrderStatusEnum Receiving = Create(2, "Receiving");
        public static PurchaseOrderStatusEnum Closed = Create(3, "Closed");
        public static List<PurchaseOrderStatusEnum> List = new List<PurchaseOrderStatusEnum>()
            {
                None,Created, Approved, Receiving, Closed
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;

        public static PurchaseOrderStatusEnum GetType(string type) => List.Exists(x => x.Name == type) ? List.FirstOrDefault(x => x.Name == type)! : None;
        public static PurchaseOrderStatusEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : None;
    }
}
