using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{

    public class CreatedRegularPurchaseOrderRequest: PurchaseOrderRequest
    {
        public CreatedRegularPurchaseOrderRequest():base()
        {

        }

        public override PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.Created;


        public double OldTRMUSDCOP { get; set; }
        public double OldTRMUSDEUR { get; set; }
        public DateTime OldCurrencyDate { get; set; }
       
        public DateTime CurrencyDate { get; set; } = DateTime.UtcNow;
        public string CurrencyDateOnly => CurrencyDate.ToShortDateString();
        public override double SumPOValueUSD => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.POValueUSD), 2);
        public override double SumPOValueCurrency => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.TotalValuePurchaseOrderCurrency), 2);
        public override double SumBudget => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.Budget), 2);
        public override double SumBudgetAssigned => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.AssignedUSD), 2);
        public override double SumBudgetPotencial => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.PotencialUSD), 2);
        public override double SumPendingUSD => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.POItemPendingUSD), 2);
        public bool IsAnyValueNotDefined => PurchaseOrderItemNoBlank.Any(x => x.QuoteCurrencyValue <= 0);
       
        public override double SumPOValueSupplierCurrency => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.TotalValuePurchaseOrderCurrency), 2);
    }

}
