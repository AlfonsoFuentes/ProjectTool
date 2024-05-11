using Application.Mappers.BudgetItems;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Base;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.Mappers.PurchaseOrders.NewMappers
{
    public static class PurchaseOrderRequestMappers
    {
        public static void ToPurchaseOrderFromRequest(this NewPurchaseOrderRequest request, PurchaseOrder purchaseOrder)
        {
            purchaseOrder.AccountAssigment = request.AccountAssigment;

            purchaseOrder.CurrencyDate = request.CurrencyDate;
            purchaseOrder.IsAlteration = request.IsAlteration;
            purchaseOrder.IsCapitalizedSalary = request.IsCapitalizedSalary;
            purchaseOrder.MainBudgetItemId = request.MainBudgetItemId;
            purchaseOrder.PurchaseorderName = request.PurchaseOrderName;
            purchaseOrder.PurchaseOrderStatus = request.PurchaseOrderStatus.Id;
            purchaseOrder.PurchaseRequisition = request.PurchaseRequisition;
            purchaseOrder.QuoteCurrency = request.QuoteCurrency.Id;
            purchaseOrder.PurchaseOrderCurrency = request.PurchaseOrderCurrency.Id;
            purchaseOrder.QuoteNo = request.QuoteNo;
            purchaseOrder.USDCOP = request.USDCOP;
            purchaseOrder.USDEUR = request.USDEUR;
            purchaseOrder.SupplierId = request.SupplierId;
            purchaseOrder.TaxCode = request.TaxCode;
            purchaseOrder.SPL = request.SPL;
            purchaseOrder.PONumber = request.PurchaseOrderNumber;
            purchaseOrder.IsTaxEditable = request.IsTaxEditable;
        }
        public static void ToPurchaseOrderApprovedFromRequest(this NewPurchaseOrderRequest request, PurchaseOrder purchaseOrder)
        {
            request.ToPurchaseOrderFromRequest(purchaseOrder);
            purchaseOrder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Approved.Id;
            purchaseOrder.PONumber = request.PurchaseOrderNumber;
            purchaseOrder.POExpectedDateDate = request.ExpectedDate;
            purchaseOrder.USDCOP = request.USDCOP;
            purchaseOrder.USDEUR = request.USDEUR;
        }
        public static void ToPurchaseOrderItemFromRequest(this NewPurchaseOrderItemRequest request, PurchaseOrderItem purchaseOrderItem)
        {
            purchaseOrderItem.Quantity = request.Quantity;
            purchaseOrderItem.UnitaryValueCurrency = request.UnitaryValuePurchaseOrderCurrency;
            purchaseOrderItem.Name = request.Name;
        }

        public static NewPurchaseOrderApproveRequest ToPurchaseOrderApproveRequest(this PurchaseOrder purchaseOrder, BudgetItem mainbudgetitem)
        {
            return new ()
            {
                PurchaseOrder=new ()
                {


                    MainBudgetItem = mainbudgetitem == null ? null! : mainbudgetitem.ToBudgetItemMWOApproved(),
                    CurrencyDate = purchaseOrder.CurrencyDate,
                    Supplier = purchaseOrder.Supplier == null ? null! : purchaseOrder.Supplier.ToResponse(),
                    ApprovedDate = purchaseOrder.POApprovedDate,
                    TaxCode = purchaseOrder.TaxCode,
                    QuoteNo = purchaseOrder.QuoteNo,
                    ClosedDate = purchaseOrder.POClosedDate,
                    ExpectedDate = purchaseOrder.POExpectedDateDate,
                    PurchaseOrderName = purchaseOrder.PurchaseorderName,
                    PurchaseOrderId = purchaseOrder.Id,
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                    PurchaseOrderNumber = purchaseOrder.PONumber,
                    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                    PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    USDCOP = purchaseOrder.USDCOP,
                    USDEUR = purchaseOrder.USDEUR,
                    PurchaseOrderItems = purchaseOrder.PurchaseOrderItems == null ? new () :
                   purchaseOrder.PurchaseOrderItems.Select(x => x.ToPurchaseOrderItemRequest()).ToList(),
                }
};
        }
        public static NewPurchaseOrderReceiveRequest ToPurchaseOrderReceiveRequest(this PurchaseOrder purchaseOrder, BudgetItem mainbudgetitem)
        {
            return new()
            {
                PurchaseOrder = new()
                {


                    MainBudgetItem = mainbudgetitem == null ? null! : mainbudgetitem.ToBudgetItemMWOApproved(),
                    CurrencyDate = purchaseOrder.CurrencyDate,
                    Supplier = purchaseOrder.Supplier == null ? null! : purchaseOrder.Supplier.ToResponse(),
                    ApprovedDate = purchaseOrder.POApprovedDate,
                    TaxCode = purchaseOrder.TaxCode,
                    QuoteNo = purchaseOrder.QuoteNo,
                    ClosedDate = purchaseOrder.POClosedDate,
                    ExpectedDate = purchaseOrder.POExpectedDateDate,
                    PurchaseOrderName = purchaseOrder.PurchaseorderName,
                    PurchaseOrderId = purchaseOrder.Id,
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                    PurchaseOrderNumber = purchaseOrder.PONumber,
                    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                    PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    USDCOP = purchaseOrder.USDCOP,
                    USDEUR = purchaseOrder.USDEUR,
                    PurchaseOrderItems = purchaseOrder.PurchaseOrderItems == null ? new() :
                   purchaseOrder.PurchaseOrderItems.Select(x => x.ToPurchaseOrderItemRequest()).ToList(),
                }
            };
        }
        public static NewPurchaseOrderEditReceiveRequest ToPurchaseOrderEditReceiveRequest(this PurchaseOrder purchaseOrder, BudgetItem mainbudgetitem)
        {
            return new()
            {
                PurchaseOrder = new()
                {


                    MainBudgetItem = mainbudgetitem == null ? null! : mainbudgetitem.ToBudgetItemMWOApproved(),
                    CurrencyDate = purchaseOrder.CurrencyDate,
                    Supplier = purchaseOrder.Supplier == null ? null! : purchaseOrder.Supplier.ToResponse(),
                    ApprovedDate = purchaseOrder.POApprovedDate,
                    TaxCode = purchaseOrder.TaxCode,
                    QuoteNo = purchaseOrder.QuoteNo,
                    ClosedDate = purchaseOrder.POClosedDate,
                    ExpectedDate = purchaseOrder.POExpectedDateDate,
                    PurchaseOrderName = purchaseOrder.PurchaseorderName,
                    PurchaseOrderId = purchaseOrder.Id,
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                    PurchaseOrderNumber = purchaseOrder.PONumber,
                    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                    PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    USDCOP = purchaseOrder.USDCOP,
                    USDEUR = purchaseOrder.USDEUR,
                    PurchaseOrderItems = purchaseOrder.PurchaseOrderItems == null ? new() :
                   purchaseOrder.PurchaseOrderItems.Select(x => x.ToPurchaseOrderItemRequest()).ToList(),
                }
            };
        }

        public static NewPurchaseOrderEditCreateRequest ToPurchaseOrderEditCreatedRequest(this PurchaseOrder purchaseOrder, BudgetItem mainbudgetitem)
        {
            return new()
            {
                PurchaseOrder=new()
                {


                    MainBudgetItem = mainbudgetitem == null ? null! : mainbudgetitem.ToBudgetItemMWOApproved(),
                    CurrencyDate = purchaseOrder.CurrencyDate,
                    Supplier = purchaseOrder.Supplier == null ? null! : purchaseOrder.Supplier.ToResponse(),
                    ApprovedDate = purchaseOrder.POApprovedDate,
                    TaxCode = purchaseOrder.TaxCode,
                    QuoteNo = purchaseOrder.QuoteNo,
                    ClosedDate = purchaseOrder.POClosedDate,
                    ExpectedDate = purchaseOrder.POExpectedDateDate,
                    PurchaseOrderName = purchaseOrder.PurchaseorderName,
                    PurchaseOrderId = purchaseOrder.Id,
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                    PurchaseOrderNumber = purchaseOrder.PONumber,
                    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                    PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    USDCOP = purchaseOrder.USDCOP,
                    USDEUR = purchaseOrder.USDEUR,
                    PurchaseOrderItems = purchaseOrder.PurchaseOrderItems == null ? new() :
                   purchaseOrder.PurchaseOrderItems.Select(x => x.ToPurchaseOrderItemRequest()).ToList(),
                }
            };
        }
        public static NewPurchaseOrderEditApproveRequest ToPurchaseOrderEditApprovedRequest(this PurchaseOrder purchaseOrder, BudgetItem mainbudgetitem)
        {
            return new()
            {
                PurchaseOrder = new()
                {


                    MainBudgetItem = mainbudgetitem == null ? null! : mainbudgetitem.ToBudgetItemMWOApproved(),
                    CurrencyDate = purchaseOrder.CurrencyDate,
                    Supplier = purchaseOrder.Supplier == null ? null! : purchaseOrder.Supplier.ToResponse(),
                    ApprovedDate = purchaseOrder.POApprovedDate,
                    TaxCode = purchaseOrder.TaxCode,
                    QuoteNo = purchaseOrder.QuoteNo,
                    ClosedDate = purchaseOrder.POClosedDate,
                    ExpectedDate = purchaseOrder.POExpectedDateDate,
                    PurchaseOrderName = purchaseOrder.PurchaseorderName,
                    PurchaseOrderId = purchaseOrder.Id,
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                    PurchaseOrderNumber = purchaseOrder.PONumber,
                    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                    PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    USDCOP = purchaseOrder.USDCOP,
                    USDEUR = purchaseOrder.USDEUR,
                    PurchaseOrderItems = purchaseOrder.PurchaseOrderItems == null ? new() :
                   purchaseOrder.PurchaseOrderItems.Select(x => x.ToPurchaseOrderItemRequest()).ToList(),
                }
            };
        }
        
        public static NewPurchaseOrderItemRequest ToPurchaseOrderItemRequest(this PurchaseOrderItem purchaseOrderItem)
        {
            return new()
            {
                PurchaseOrderItemId = purchaseOrderItem.Id,
                BudgetItem = purchaseOrderItem.BudgetItem == null ? null! : purchaseOrderItem.BudgetItem.ToBudgetItemMWOApprovedResponse(),
                CurrencyDate = purchaseOrderItem.CurrencyDate,
                ExpectedDate = purchaseOrderItem.POExpectedDate == null ? string.Empty : purchaseOrderItem.POExpectedDate!.Value.ToString("d"),
                IsCapitalizedSalary = purchaseOrderItem.IsCapitalizedSalary,
                IsTaxAlteration = purchaseOrderItem.IsTaxAlteration,
                IsTaxEditable = purchaseOrderItem.IsTaxEditable,
                IsTaxNoProductive = purchaseOrderItem.IsTaxNoProductive,
                PurchaseOrderCurrency = purchaseOrderItem.PurchaseOrderCurrency,
                PurchaseOrderNumber = purchaseOrderItem.PurchaseOrderNumber,
                PurchaseorderStatus = purchaseOrderItem.PurchaseorderStatus,
                PurchaseOrderUSDCOP = purchaseOrderItem.USDCOP,
                PurchaseOrderUSDEUR = purchaseOrderItem.USDEUR,
                Name = purchaseOrderItem.Name,
                PurchaseRequisition = purchaseOrderItem.PurchaseRequisition,
                Quantity = purchaseOrderItem.Quantity,
                QuoteCurrency = purchaseOrderItem.QuoteCurrency,
                UnitaryValueQuoteCurrency = purchaseOrderItem.UnitaryValueQuoteCurrency,
                PurchaseOrderReceiveds = purchaseOrderItem.PurchaseOrderReceiveds == null ? new() :
                purchaseOrderItem.PurchaseOrderReceiveds.Select(x => x.ToPurchaseOrderItemReceivedRequest())
                .OrderBy(x=>x.ReceivedDate).ThenBy(x=>x.BudgetItemNomclatoreName).ToList(),

            };
        }
        public static NewPurchaseOrderItemReceivedRequest ToPurchaseOrderItemReceivedRequest(this PurchaseOrderItemReceived purchaseOrderItemReceived)
        {
            return new NewPurchaseOrderItemReceivedRequest()
            {
                BudgetItemNomclatoreName=purchaseOrderItemReceived.BudgetItemNomenclatoreName,
                ReceivedDate = purchaseOrderItemReceived.ReceivedDate,
                CurrencyDate = purchaseOrderItemReceived.CurrencyDate,
                PurchaseOrderCurrency = purchaseOrderItemReceived.PurchaseOrderCurrency,
                PurchaseOrderItemReceivedId = purchaseOrderItemReceived.Id,
                USDCOP = purchaseOrderItemReceived.USDCOP,
                USDEUR = purchaseOrderItemReceived.USDEUR,
                ValueReceivedCurrency = purchaseOrderItemReceived.ValueReceivedCurrency,
            };
        }
    }
}
