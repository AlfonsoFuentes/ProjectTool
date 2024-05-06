using Shared.Enums.Currencies;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Data
{
    public class PurchaseOrderItemReceived : BaseEntity, ITenantEntity
    {

        public static PurchaseOrderItemReceived Create()
        {
            return new()
            {
                Id = Guid.NewGuid(),
               
            };
        }
        public string TenantId { get; set; } = string.Empty;
        public PurchaseOrderItem PurchaseOrderItem { get; set; } = null!;
        public Guid PurchaseOrderItemId { get; set; }
        public double ValueReceivedCurrency { get; set; }
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        [NotMapped]
        public CurrencyEnum PurchaseOrderCurrency => PurchaseOrderItem == null ? CurrencyEnum.None : PurchaseOrderItem.PurchaseOrderCurrency;

        [NotMapped]
        public double ValueReceivedUSD =>
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ValueReceivedCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ValueReceivedCurrency / USDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? ValueReceivedCurrency / USDEUR :
             0;

    }
}
