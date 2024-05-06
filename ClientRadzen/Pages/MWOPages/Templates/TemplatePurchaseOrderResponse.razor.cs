using ClientRadzen.Pages.Enums;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.PurchaseOrders.Responses;
#nullable disable
namespace ClientRadzen.Pages.MWOPages.Templates;
public partial class TemplatePurchaseOrderResponse
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    [EditorRequired]
    public PurchaseorderView View { get; set; }
    [Parameter]
    public MWOEBPResponse MWOEBPResponse { get; set; } = new();
    List<NewPurchaseOrderResponse> PurchaseOrders => View == PurchaseorderView.Actual ?
        MWOEBPResponse.Actual : View == PurchaseorderView.Commitment ? MWOEBPResponse.Commitment : MWOEBPResponse.Potential;

    [Inject]
    private IPurchaseOrderService PurchaseorderService { get; set; }


    public double TotalValue =>
        View == PurchaseorderView.Actual ? PurchaseOrders.Sum(x => x.ActualUSD) :
        View == PurchaseorderView.Commitment ? PurchaseOrders.Sum(x => x.CommitmentUSD) :
         PurchaseOrders.Sum(x => x.AssignedUSD);
    string LabelAction = "";
    public void ApprovedPurchaseOrder(Guid PurchaseOrderId)
    {
        _NavigationManager.NavigateTo($"/ApprovePurchaseOrder/{PurchaseOrderId}");
    }
    public void ReceivePurchaseOrder(Guid PurchaseOrderId)
    {
        _NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{PurchaseOrderId}");
    }

    public void EditPurchaseOrder(Guid PurchaseOrderId, bool IsTaxEditable, bool IsCapitalizedSalary, PurchaseOrderStatusEnum PurchaseOrderStatus)
    {
        if (IsTaxEditable)
        {
            _NavigationManager.NavigateTo($"/EditTaxPurchaseOrder/{PurchaseOrderId}");

        }
        else if (IsCapitalizedSalary)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderCapitalizedSalary/{PurchaseOrderId}");
        }
        else if (PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderCreated/{PurchaseOrderId}");
        }
        else if (PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderApproved/{PurchaseOrderId}");
        }
        else if (PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id || PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderClosed/{PurchaseOrderId}");
        }

    }
    public async Task RemovePurchaseorder(Guid PurchaseOrderId, string PurchaseOrderNumber)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {PurchaseOrderNumber}?", "Confirm Delete",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            var result = await PurchaseorderService.DeletePurchaseOrder(PurchaseOrderId);
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
