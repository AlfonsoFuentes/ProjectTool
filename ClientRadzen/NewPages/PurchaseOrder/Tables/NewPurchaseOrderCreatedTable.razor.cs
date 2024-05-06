using Shared.Models.PurchaseOrders.Responses;
using Shared.NewModels.PurchaseOrders.Responses;
#nullable disable
namespace ClientRadzen.NewPages.PurchaseOrder.Tables;
public partial class NewPurchaseOrderCreatedTable
{

    [CascadingParameter]
    public NewPurchaseOrderMain MainPage { get; set; }
    [CascadingParameter]
    public App MainApp { get; set; }

    public List<NewPriorPurchaseOrderResponse> PurchaseOrders => MainPage.Createds;

    List<NewPriorPurchaseOrderResponse> FilteredItems => PurchaseOrders.Count == 0 ? new() : PurchaseOrders.Where(fiterexpresion).ToList();

    Func<NewPriorPurchaseOrderResponse, bool> fiterexpresion => x =>
          x.PurchaseorderName.Contains(MainPage.nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
            x.PurchaseRequisition.Contains(MainPage.nameFilter, StringComparison.CurrentCultureIgnoreCase) ||

           x.SupplierNickName.Contains(MainPage.nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.SupplierName.Contains(MainPage.nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
            x.MWOName.Contains(MainPage.nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
            x.SupplierVendorCode.Contains(MainPage.nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.AccountAssigment.Contains(MainPage.nameFilter, StringComparison.CurrentCultureIgnoreCase);
}
