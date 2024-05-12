using ClientRadzen.Enums;
using ClientRadzen.NewPages.BudgetItems.MWOApproved;
using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;
#nullable disable

namespace ClientRadzen.NewPages.EBPReport;
public partial class NewMWOEBPReportPurchaseordersTable
{
    [CascadingParameter]
    private App MainApp { get; set; }
    [Inject]
    private INewPurchaseOrderService PurchaseOrderService { get; set; }
    [Parameter]
    public PurchaseorderView View { get; set; }

    [Parameter]
  
    public List<NewPriorPurchaseOrderResponse> PurchaseOrders { get; set; } = new();

    public void EditPurchaseOrderCreated(NewPriorPurchaseOrderResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditCreate}/{selectedRow.PurchaseOrderId}");

    }
    public void ApprovedPurchaseOrder(NewPriorPurchaseOrderResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.Approve}/{selectedRow.PurchaseOrderId}");
    }
    public void EditPurchaseOrderApproved(NewPriorPurchaseOrderResponse selectedRow)
    {

        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditApproved}/{selectedRow.PurchaseOrderId}");
    }
    public void EditPurchaseOrderReceiving(NewPriorPurchaseOrderResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditReceive}/{selectedRow.PurchaseOrderId}");

    }
    public void ReceivePurchaseOrder(NewPriorPurchaseOrderResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.Receive}/{selectedRow.PurchaseOrderId}");
    }
    public void EditPurchaseOrderClosed(NewPriorPurchaseOrderResponse selectedRow)
    {

        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditReceive}/{selectedRow.PurchaseOrderId}");
    }
    public void EditPurchaseOrderSalary(NewPriorPurchaseOrderResponse selectedRow)
    {

        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditSalary}/{selectedRow.PurchaseOrderId}");
    }
    public async Task RemovePurchaseorder(NewPriorPurchaseOrderResponse purchaseOrderResponse)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {purchaseOrderResponse.PurchaseOrderLegendToDelete}?", "Confirm Delete",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            NewPurchaseOrderDeleteRequest request = new NewPurchaseOrderDeleteRequest()
            {
                PurchaseOrderId = purchaseOrderResponse.PurchaseOrderId,
                Name = purchaseOrderResponse.PurchaseOrderNumber,
            };
            var result = await PurchaseOrderService.DeletePurchaseOrderAsync(request);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);


            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }
    }
}
