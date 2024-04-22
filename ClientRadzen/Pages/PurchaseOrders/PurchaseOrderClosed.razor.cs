using Microsoft.AspNetCore.Components;
using Radzen;
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

    Func<NewPurchaseOrderClosedResponse, bool> fiterexpresion => x =>
        x.PurchaseOrderStatus.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.PurchaseorderName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.PurchaseOrderNumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.SupplierNickName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.SupplierName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.MWOName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.VendorCode.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.AccountAssigment.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
    IEnumerable<NewPurchaseOrderClosedResponse> FilteredItems => OriginalData.Where(fiterexpresion).AsQueryable();
    IEnumerable<NewPurchaseOrderClosedResponse> OriginalData => MainPO.PurchaseordersClosed == null ? new List<NewPurchaseOrderClosedResponse>() : MainPO.PurchaseordersClosed;


    void EditPurchaseOrder(NewPurchaseOrderClosedResponse selectedRow)
    {
        if (selectedRow.IsTaxEditable)
        {
            _NavigationManager.NavigateTo($"/EditTaxPurchaseOrder/{selectedRow.PurchaseOrderId}");

        }
        else if (selectedRow.IsCapitalizedSalary)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderCapitalizedSalary/{selectedRow.PurchaseOrderId}");
        }
        else if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderClosed/{selectedRow.PurchaseOrderId}");
        }



    }
    async Task RemovePurchaseorder(NewPurchaseOrderClosedResponse selectedRow)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {selectedRow.PurchaseOrderNumber}?", "Confirm Delete",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            var result = await MainPO.Service.DeletePurchaseOrder(selectedRow.PurchaseOrderId);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                await MainPO.UpdateAll();

            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }
    }

}
