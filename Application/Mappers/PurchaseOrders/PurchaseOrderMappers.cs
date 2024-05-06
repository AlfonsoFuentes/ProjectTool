using Domain.Entities.Data;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Application.Mappers.PurchaseOrders
{
    public static class PurchaseOrderMappers
    {
        
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
                    
                PurchaseOrderItems = (purchaseOrder.PurchaseOrderItems == null || purchaseOrder.PurchaseOrderItems.Count == 0) ? new() :
                purchaseOrder.PurchaseOrderItems.Select(x => x.ToPurchaseOrderItemResponse()).ToList(),




            };
        }

        public static NewPurchaseOrderApproveRequest ToPurchaseOrderApprovedRequest(this PurchaseOrder purchaseOrder)
        {
            return new()
            {
                AccountAssigment = purchaseOrder.AccountAssigment,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                CurrencyDate = purchaseOrder.CurrencyDate,
                IsAlteration = purchaseOrder.IsAlteration,
                IsCapitalizedSalary = purchaseOrder.IsCapitalizedSalary,
                IsTaxEditable = purchaseOrder.IsTaxEditable,
                MainBudgetItemId = purchaseOrder.MainBudgetItemId,
                MWOId = purchaseOrder.MWOId,
               
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

                PurchaseOrderItems = (purchaseOrder.PurchaseOrderItems == null || purchaseOrder.PurchaseOrderItems.Count == 0) ? new() :
                purchaseOrder.PurchaseOrderItems.Select(x => x.ToPurchaseOrderItemRequest()).ToList(),




            };
        }
    }
}
