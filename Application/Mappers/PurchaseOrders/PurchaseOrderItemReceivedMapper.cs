using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Application.Mappers.PurchaseOrders
{
    public static class PurchaseOrderItemReceivedMapper
    {
        public static PurchaseOrderItemReceived ToPurchaseOrderItemReceived(this NewPurchaseOrderCreateItemRequest request, PurchaseOrderItemReceived received)
        {
            received.USDEUR = request.USDEUR;
            received.USDCOP = request.USDCOP;
            received.CurrencyDate = request.CurrencyDate;
            received.ValueReceivedCurrency = request.ItemQuoteValueCurrency;

            return received;
        }
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
