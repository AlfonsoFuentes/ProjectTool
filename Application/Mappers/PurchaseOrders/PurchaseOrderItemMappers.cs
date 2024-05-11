using Application.Mappers.BudgetItems;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Base;
using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Application.Mappers.PurchaseOrders
{
    public static class PurchaseOrderItemMappers
    {

        public static NewPriorPurchaseOrderItemResponse ToPurchaseOrderItemResponse(this PurchaseOrderItem purchaseOrderItem)
        {
            return new()
            {
                BudgetItemId = purchaseOrderItem.BudgetItemId,
                ExpectedOn = purchaseOrderItem.ExpectedDateDate,

                IsCapitalizedSalary = purchaseOrderItem.IsCapitalizedSalary,
                IsTaxAlteration = purchaseOrderItem.IsTaxAlteration,
                IsTaxEditable = purchaseOrderItem.IsTaxEditable,
                IsTaxNoProductive = purchaseOrderItem.IsTaxNoProductive,
                Name = purchaseOrderItem.Name,
                PurchaseOrderItemId = purchaseOrderItem.Id,
                POExpectedDate = purchaseOrderItem.POExpectedDate,
                PurchaseOrderCurrency = purchaseOrderItem.PurchaseOrderCurrency,
                PurchaseOrderId = purchaseOrderItem.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrderItem.PurchaseOrderNumber,
                PurchaseOrderStatus = purchaseOrderItem.PurchaseorderStatus,
                PurchaseRequisition = purchaseOrderItem.PurchaseRequisition,
                Quantity = purchaseOrderItem.Quantity,
                QuoteCurrency = purchaseOrderItem.QuoteCurrency,
                Supplier = purchaseOrderItem.Supplier,
                UnitaryValuePurchaseOrderCurrency = purchaseOrderItem.UnitaryValueCurrency,
                USDCOP = purchaseOrderItem.USDCOP,
                USDEUR = purchaseOrderItem.USDEUR,
                PurchaseOrderReceiveds = (purchaseOrderItem.PurchaseOrderReceiveds == null || purchaseOrderItem.PurchaseOrderReceiveds.Count == 0) ? new() :
                purchaseOrderItem.PurchaseOrderReceiveds.Select(x => x.ToPurchaseOrderReceivedResponse()).ToList(),
            };
        }
       
      
        public static NewPurchaseOrderReceiveItemRequest ToPurchaseOrderReceiveItemRequest(this PurchaseOrderItem purchaseOrderItem)
        {
            return new()
            {
                PurchaseOrderItemId = purchaseOrderItem.Id,
                BudgetItem = purchaseOrderItem.BudgetItem == null ? null! : purchaseOrderItem.BudgetItem.ToBudgetItemToCreatePurchaseOrder(),
                IsTaxAlteration = purchaseOrderItem.IsTaxAlteration,
                IsTaxNoProductive = purchaseOrderItem.IsTaxNoProductive,
                Name = purchaseOrderItem.Name,
                PurchaseOrderCurrency = purchaseOrderItem.PurchaseOrderCurrency,
                Quantity = purchaseOrderItem.Quantity,
                QuoteCurrency = purchaseOrderItem.QuoteCurrency,
                UnitaryValueCurrency = purchaseOrderItem.UnitaryValueCurrency,
                USDCOP = purchaseOrderItem.USDCOP,
                USDEUR = purchaseOrderItem.USDEUR,
                CurrencyDate = purchaseOrderItem.PurchaseOrder.CurrencyDate,

                PurchaseOrderStatus = purchaseOrderItem.PurchaseorderStatus,
                Receiveds = (purchaseOrderItem.PurchaseOrderReceiveds == null || purchaseOrderItem.PurchaseOrderReceiveds.Count == 0) ? new() :
                 purchaseOrderItem.PurchaseOrderReceiveds.Select(x => x.ToPurchaseOrderReceivedRequest()).ToList(),



            };
        }
        public static NewPurchaseOrderReceiveItemActualRequest ToPurchaseOrderReceivedRequest(this PurchaseOrderItemReceived purchaseOrderItemReceived)
        {
            return new NewPurchaseOrderReceiveItemActualRequest()
            {
                POItemName = purchaseOrderItemReceived.PurchaseOrderItem.Name,
                ReceivedId = purchaseOrderItemReceived.Id,
                PurchaseOrderCurrency = purchaseOrderItemReceived.PurchaseOrderCurrency,
                ReceivedCurrency = purchaseOrderItemReceived.ValueReceivedCurrency,
                USDCOP = purchaseOrderItemReceived.USDCOP,
                USDEUR = purchaseOrderItemReceived.USDEUR,
                CurrencyDate = purchaseOrderItemReceived.CurrencyDate,
            };
        }
       
       
        public static PurchaseOrderItem ToPurchaseOrderItemFromReceivedRequest(this NewPurchaseOrderReceiveItemRequest request, PurchaseOrderItem item)
        {
            item.Quantity = request.Quantity;
            item.UnitaryValueCurrency = request.UnitaryValueCurrency;
            item.Name = request.Name;


            return item;
        }
    }
}
