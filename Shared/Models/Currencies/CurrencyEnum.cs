namespace Shared.Models.Currencies
{
    public class CurrencyEnum
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public static CurrencyEnum Create(int id, string name) => new CurrencyEnum() { Id = id, Name = name };

        public static CurrencyEnum None = Create(-1, "NONE");
        public static CurrencyEnum USD = Create(0, "USD");
        public static CurrencyEnum COP = Create(1, "COP");
        public static CurrencyEnum EUR = Create(2, "EUR");

        public static List<CurrencyEnum> List = new List<CurrencyEnum>()
            {
          None, USD, COP, EUR
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;

        public static CurrencyEnum GetType(string type) => List.Exists(x => x.Name == type) ? List.FirstOrDefault(x => x.Name == type)! : CurrencyEnum.None;
        public static CurrencyEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : CurrencyEnum.None;
    }
}
