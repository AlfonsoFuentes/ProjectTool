namespace Shared.NewModels.PurchaseOrders.Request
{
    //public static class NewPurchaseOrderCreateRequestExtension
    //{
    //    public static void SetBudgetItem(this NewPurchaseOrderCreateRequest request, NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem, double _USDCOP, double _USDEUR)
    //    {

    //        request.MainBudgetItem = _BudgetItem;
    //        request.AddBudgetItem(request.MainBudgetItem);
    //        request.SetTRM(_USDCOP, _USDEUR, DateTime.UtcNow);
    //        if (request.MainBudgetItem.IsCapitalizedSalary) request.SetPurchaseOrderCurrency(CurrencyEnum.USD);
    //        request.SetQuoteCurrency(CurrencyEnum.COP);
    //    }
    //    public static void AddBudgetItem(this NewPurchaseOrderCreateRequest request, NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem)
    //    {
    //        NewPurchaseOrderCreateItemRequest item = new();
    //        item.SetBudgetItem(_BudgetItem);
    //        request.PurchaseOrderItems.Add(item);
    //    }
    //    public static void SetTRM(this NewPurchaseOrderCreateRequest request, double _usdcop, double _usdEUR, DateTime currencydate)
    //    {
    //        request.USDCOP = _usdcop;
    //        request.USDEUR = _usdEUR;
    //        request.CurrencyDate = currencydate;
    //        foreach (var row in request.PurchaseOrderItems)
    //        {
    //            row.USDCOP = _usdcop;
    //            row.USDEUR = _usdEUR;
    //            row.CurrencyDate = request.CurrencyDate;
    //        }
    //    }
    //    public static void SetQuoteCurrency(this NewPurchaseOrderCreateRequest request, CurrencyEnum _QuoteCurrency)
    //    {
    //        request.QuoteCurrency = _QuoteCurrency;
    //        foreach (var row in request.PurchaseOrderItems)
    //        {
    //            row.QuoteCurrency = _QuoteCurrency;
    //        }
    //    }
    //    public static void SetPurchaseOrderCurrency(this NewPurchaseOrderCreateRequest request, CurrencyEnum _QuoteCurrency)
    //    {
    //        request.PurchaseOrderCurrency = _QuoteCurrency;
    //        foreach (var row in request.PurchaseOrderItems)
    //        {
    //            row.PurchaseOrderCurrency = _QuoteCurrency;
    //        }
    //    }

    //    public static void SetSupplier(this NewPurchaseOrderCreateRequest request, NewSupplierResponse newSupplier)
    //    {
    //        request.Supplier = newSupplier;
    //        request.TaxCode = request.MainBudgetItem.IsAlteration ? request.Supplier.TaxCodeLD : request.Supplier.TaxCodeLD;
    //        request.SetPurchaseOrderCurrency(request.Supplier.SupplierCurrency);
    //    }
    //    public static void SetPurchaseOrderName(this NewPurchaseOrderCreateRequest request, string purchaseorderName)
    //    {

    //        request.PurchaseorderName = purchaseorderName;
    //        if (request.PurchaseOrderItems.Count == 1)
    //            request.PurchaseOrderItems[0].Name = purchaseorderName;
    //        //if (IsTaxEditable)
    //        //{
    //        //    PurchaseRequisition = $"Tax for {PurchaseorderName}";
    //        //    QuoteNo = $"Tax for {PurchaseorderName}";
    //        //    TaxCode = $"Tax for {PurchaseorderName}";
    //        //}
    //    }
    //    public static void SetPurchaseOrderItemName(this NewPurchaseOrderCreateRequest request, string purchaseorderName)
    //    {
    //        if (request.PurchaseOrderItems.Count == 1)
    //        {
    //            request.PurchaseorderName = purchaseorderName;

    //            request.PurchaseOrderItems[0].Name = purchaseorderName;
    //        }

    //    }
    //    public static void SetBudgetItem(this NewPurchaseOrderCreateItemRequest request, NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem)
    //    {
    //        request.BudgetItem = _BudgetItem;
    //    }
    //}
}
