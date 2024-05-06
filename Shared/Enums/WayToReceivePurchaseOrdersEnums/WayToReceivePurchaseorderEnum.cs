namespace Shared.Enums.WayToReceivePurchaseOrdersEnums
{
    public class WayToReceivePurchaseorderEnum
    {
        public override string ToString()
        {
            return Name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public static WayToReceivePurchaseorderEnum Create(int id, string name) => new WayToReceivePurchaseorderEnum() { Id = id, Name = name };

        public static WayToReceivePurchaseorderEnum None = Create(-1, "NONE");
        public static WayToReceivePurchaseorderEnum CompleteOrder = Create(0, "Receive PO 100%");
        public static WayToReceivePurchaseorderEnum PercentageOrder = Create(1, "Receive PO By Percentage");
        public static WayToReceivePurchaseorderEnum MoneyByItem = Create(2, "Receive By Item");
        public static WayToReceivePurchaseorderEnum PercentageByItem = Create(3, "Receive Percentage By Item");

        public static List<WayToReceivePurchaseorderEnum> List = new List<WayToReceivePurchaseorderEnum>()
            {
          None,  CompleteOrder, PercentageOrder, MoneyByItem, PercentageByItem
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;

        public static WayToReceivePurchaseorderEnum GetType(string type) => List.Exists(x => x.Name == type) ? List.FirstOrDefault(x => x.Name == type)! : None;
        public static WayToReceivePurchaseorderEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : None;
    }
}
