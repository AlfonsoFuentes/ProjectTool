using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Application.Mappers.PurchaseOrders
{
    public static class PurchaseOrderItemReceivedMapper
    {
       
        public static NewPriorPurchaseOrderReceivedResponse ToPurchaseOrderReceivedResponse(this PurchaseOrderItemReceived purchaseOrderReceived)
        {
            return new()
            {
                CurrencyDate = purchaseOrderReceived.CurrencyDate,
                PurchaseOrderCurrency = purchaseOrderReceived.PurchaseOrderCurrency,
                PurchaseOrderItemId = purchaseOrderReceived.PurchaseOrderItemId,
                PurchaseOrderItemReceivedId = purchaseOrderReceived.Id,
                USDCOP = purchaseOrderReceived.USDCOP,
                USDEUR = purchaseOrderReceived.USDEUR,
                ValueReceivedCurrency = purchaseOrderReceived.ValueReceivedCurrency,
            

            };
        }
    }
}
