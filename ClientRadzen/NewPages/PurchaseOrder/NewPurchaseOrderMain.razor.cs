using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace ClientRadzen.NewPages.PurchaseOrder;
#nullable disable
public partial class NewPurchaseOrderMain
{

    [CascadingParameter]
    public App MainApp { get; set; }
    void ChangeIndex(int index)
    {
        MainApp.TabIndexPurchaseOrder = index;
    }
    [Inject]
    public INewPurchaseOrderService Service { get; set; } = null!;

    public string nameFilter { get; set; } = string.Empty;
    public List<NewPriorPurchaseOrderResponse> Createds { get; set; } = new();
    public List<NewPriorPurchaseOrderResponse> Approveds { get; set; } = new();
    public List<NewPriorPurchaseOrderResponse> Closeds { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await UpdateAll();
    }
    async Task UpdateAll()
    {
        var resultCreated = await Service.GetAllCreated();
        if (resultCreated.Succeeded)
        {
            Createds = resultCreated.Data.PurchaseOrders;
        }
        var resultApproved = await Service.GetAllApproved();
        if (resultApproved.Succeeded)
        {
            Approveds = resultApproved.Data.PurchaseOrders;
        }
        var resultClosed = await Service.GetAllClosed();
        if (resultClosed.Succeeded)
        {
            Closeds = resultClosed.Data.PurchaseOrders;
        }
        StateHasChanged();
    }
    public void EditPurchaseOrderCreated(NewPriorPurchaseOrderResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditCreate}/{selectedRow.PurchaseOrderId}");
    }
 
    public void ApprovePurchaseOrder(NewPriorPurchaseOrderResponse selectedRow)
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
    public void EditPurchaseOrderSalary(NewPriorPurchaseOrderResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditSalary}/{selectedRow.PurchaseOrderId}");
    }
    public void ReceivePurchaseorder(NewPriorPurchaseOrderResponse selectedRow)
    {

        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.Receive}/{selectedRow.PurchaseOrderId}");
    }
    public async Task UnApprovePurchaseorder(NewPriorPurchaseOrderResponse selectedRow)
    {

        var resultDialog = await DialogService.Confirm($"Are you sure you want un approve {selectedRow.PurchaseOrderNumber}?", "Confirm Un Approve",
          new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            NewPurchaseOrderUnApproveRequest request = new()
            {
                PurchaseOrderNumber = selectedRow.PurchaseOrderNumber,
                PurchaseOrderId = selectedRow.PurchaseOrderId,
                PurchaseorderName = selectedRow.PurchaseorderName,
            };
            var resultUnApprove = await Service.UnApprovePurchaseOrderAsync(request);
            if (resultUnApprove.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", resultUnApprove.Messages);

                await UpdateAll();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", resultUnApprove.Messages);
            }
        }
    }
    public async Task ReOpenPurchaseOrder(NewPriorPurchaseOrderResponse selectedRow)
    {

        var resultDialog = await DialogService.Confirm($"Are you sure reopen {selectedRow.PurchaseRequisition}?", "Confirm Reopen",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            NewPurchaseOrderReOpenRequest request = new()
            {
                PurchaseorderName = selectedRow.PurchaseorderName,
                PurchaseOrderId = selectedRow.PurchaseOrderId,
                PurchaseOrderNumber = selectedRow.PurchaseOrderNumber,

            };
            var result = await Service.ReOpenPurchaseOrderAsync(request);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                await UpdateAll();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }
    }
    public async Task RemovePurchaseorder(NewPriorPurchaseOrderResponse selectedRow)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {selectedRow.PurchaseRequisition}?", "Confirm Delete",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            NewPurchaseOrderDeleteRequest request = new() { Name = selectedRow.PurchaseorderName, PurchaseOrderId = selectedRow.PurchaseOrderId };
            var result = await Service.DeletePurchaseOrderAsync(request);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                await UpdateAll();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }
    }
}
