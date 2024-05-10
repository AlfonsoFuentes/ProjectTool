using Application.Mappers.BudgetItems;
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
                purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemResponse()).ToList(),




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
                ExpectedDate = purchaseOrder.POExpectedDateDate,

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
                IsAssetProductive = purchaseOrder.MWO.IsAssetProductive,
                PurchaseOrderItems = (purchaseOrder.PurchaseOrderItems == null || purchaseOrder.PurchaseOrderItems.Count == 0) ? new() :
                purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemRequest()).ToList(),




            };
        }
        public static NewPurchaseOrderEditApprovedRequest ToPurchaseOrderEditApprovedRequest(this PurchaseOrder purchaseOrder)
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
                ExpectedDate = purchaseOrder.POExpectedDateDate,

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
                IsAssetProductive = purchaseOrder.MWO.IsAssetProductive,
                PurchaseOrderItems = (purchaseOrder.PurchaseOrderItems == null || purchaseOrder.PurchaseOrderItems.Count == 0) ? new() :
               purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemRequest()).ToList(),




            };
        }
        public static NewPurchaseOrderCreateEditRequest ToPurchaseOrderCreateEditRequest(this PurchaseOrder purchaseOrder, BudgetItem budgetItem)
        {
            return new()
            {

                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                CurrencyDate = purchaseOrder.CurrencyDate,
                MainBudgetItem = budgetItem == null ? null! : budgetItem.ToBudgetItemToCreatePurchaseOrder(),

                PurchaseOrderNumber = purchaseOrder.PONumber,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                QuoteNo = purchaseOrder.QuoteNo,

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

                PurchaseOrderItems = (purchaseOrder.PurchaseOrderItems == null || purchaseOrder.PurchaseOrderItems.Count == 0) ? new() :
                purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemRequest()).ToList(),




            };
        }

        public static NewPurchaseOrderReceiveRequest ToPurchaseOrderReceiveRequest(this PurchaseOrder purchaseOrder)
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
                ExpectedDate = purchaseOrder.POExpectedDateDate,
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
                purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderReceiveItemRequest()).ToList(),




            };
        }

        public static NewPurchaseOrderEditReceiveRequest ToPurchaseOrderEditReceiveRequest(this PurchaseOrder purchaseOrder)
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
                ExpectedDate = purchaseOrder.POExpectedDateDate,
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
                purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderReceiveItemRequest()).ToList(),




            };
        }

        public static NewPurchaseOrderEditSalaryRequest ToPurchaseOrderEditSalaryRequest(this PurchaseOrder purchaseOrder)
        {
            return new()
            {
                
                
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                CurrencyDate = purchaseOrder.CurrencyDate,
                IsAlteration = purchaseOrder.IsAlteration,
              
                IsTaxEditable = purchaseOrder.IsTaxEditable,

                CreatePurchaseOrderNumber=string.IsNullOrEmpty(purchaseOrder.PONumber),
                PurchaseOrderNumber = purchaseOrder.PONumber,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
               
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
               
                SPL = purchaseOrder.SPL,
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
               
                PurchaseOrderItems = (purchaseOrder.PurchaseOrderItems == null || purchaseOrder.PurchaseOrderItems.Count == 0) ? new() :
                purchaseOrder.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderReceiveItemRequest()).ToList(),




            };
        }
    }
}
