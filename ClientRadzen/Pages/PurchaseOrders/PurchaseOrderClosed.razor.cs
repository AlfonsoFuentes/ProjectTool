using Microsoft.AspNetCore.Components;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders;
public partial class PurchaseOrderClosed
{
    [CascadingParameter]
    private App MainApp { get; set; }
    [CascadingParameter]
    public PurchaseOrderDataMain MainPO { get; set; }


    string nameFilter => MainPO.nameFilter;

    Func<PurchaseOrderResponse, bool> fiterexpresion => x =>
        x.PurchaseOrderStatus.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.PurchaseorderName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.PONumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.Supplier.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.MWOName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.VendorCode.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.AccountAssigment.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
    IEnumerable<PurchaseOrderResponse> FilteredItems => OriginalData.Where(fiterexpresion).AsQueryable();
    IEnumerable<PurchaseOrderResponse> OriginalData => MainPO.PurchaseordersClosed == null ? new List<PurchaseOrderResponse>() : MainPO.PurchaseordersClosed;


    void EditPurchaseOrder(PurchaseOrderResponse selectedRow)
    {
        if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderClosed/{selectedRow.PurchaseOrderId}");
        }



    }


}
