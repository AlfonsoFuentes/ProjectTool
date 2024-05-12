namespace Application.Mappers.PurchaseOrders
{
    public static class PurchaseOrderResponseMappers
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
                UnitaryValueQuoteCurrency = purchaseOrderItem.UnitaryValueQuoteCurrency,
                USDCOP = purchaseOrderItem.USDCOP,
                USDEUR = purchaseOrderItem.USDEUR,
                PurchaseOrderReceiveds = purchaseOrderItem.PurchaseOrderReceiveds == null || purchaseOrderItem.PurchaseOrderReceiveds.Count == 0 ? new() :
                purchaseOrderItem.PurchaseOrderReceiveds.Select(x => x.ToPurchaseOrderReceivedResponse()).ToList(),
            };
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
        public static NewPriorPurchaseOrderResponse ToPurchaseOrderResponse(this PurchaseOrder purchaseOrder)
        {
            return new()
            {
                AccountAssigment = purchaseOrder.AccountAssigment,
                Currency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                CurrencyDate = purchaseOrder.CurrencyDate,
                IsAlteration = purchaseOrder.IsAlteration,
                IsCapitalizedSalary = purchaseOrder.IsCapitalizedSalary,
                IsTaxEditable = purchaseOrder.IsTaxEditable,
                MainBudgetItemId = purchaseOrder.MainBudgetItemId,
                MWOId = purchaseOrder.MWOId,
                POApprovedDate = purchaseOrder.POApprovedDate,
                POClosedDate = purchaseOrder.POClosedDate,
                POExpectedDateDate = purchaseOrder.POExpectedDateDate,
                PurchaseOrderNumber = purchaseOrder.PONumber,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                QuoteNo = purchaseOrder.QuoteNo,
                SPL = purchaseOrder.SPL,
                Supplier = purchaseOrder.Supplier == null ? null : new()
                {
                    SupplierId = purchaseOrder.Supplier.Id,
                    Name = purchaseOrder.Supplier.Name,
                    NickName = purchaseOrder.Supplier.NickName,
                    SupplierCurrency = CurrencyEnum.GetType(purchaseOrder.Supplier.SupplierCurrency),
                    VendorCode = purchaseOrder.Supplier.VendorCode,

                },
                TaxCode = purchaseOrder.TaxCode,
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CreatedDate = purchaseOrder.CreatedDate,
                CECName = purchaseOrder.MWO.CECName,
                MWOName = purchaseOrder.MWO.Name,

                PurchaseOrderItems = purchaseOrder.PurchaseOrderItems == null || purchaseOrder.PurchaseOrderItems.Count == 0 ? new() :
                purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemResponse()).ToList(),




            };
        }
    }
}
