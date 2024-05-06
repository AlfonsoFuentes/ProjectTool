using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.BudgetItems;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;


namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits
{
    public class EditPurchaseOrderRegularClosedRequest : EditPurchaseOrderRegularApprovedRequest
    {
        public EditPurchaseOrderRegularClosedRequest()
        {

        }
        public double POValueCurrencyOriginal { get; set; }
        public double POValueUSDOriginal => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            POValueCurrencyOriginal : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
            POValueCurrencyOriginal / USDCOP : POValueCurrencyOriginal / USDEUR;
        public override PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.Closed;
    }
}
