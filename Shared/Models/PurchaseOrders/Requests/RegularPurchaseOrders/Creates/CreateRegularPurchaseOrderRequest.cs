using Shared.Enums.PurchaseorderStatus;

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
        public override double SumPOValueUSD => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.PurchaseOrderValueUSD), 2);
        public override double SumPOValueCurrency => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.PurchaseOrderValuePurchaseOrderCurrency), 2);
        public override double SumBudget => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.BudgetUSD), 2);
        public override double SumBudgetAssigned => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.NewAssignedUSD), 2);
        public override double SumBudgetPotencial => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.NewPotencialUSD), 2);
        public override double SumPendingUSD => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.PendingToCommitmUSD), 2);
        public bool IsAnyValueNotDefined => PurchaseOrderItemNoBlank.Any(x => x.QuoteCurrencyValue <= 0);
       
        public override double SumPOValueSupplierCurrency => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.PurchaseOrderValuePurchaseOrderCurrency), 2);
    }

}
