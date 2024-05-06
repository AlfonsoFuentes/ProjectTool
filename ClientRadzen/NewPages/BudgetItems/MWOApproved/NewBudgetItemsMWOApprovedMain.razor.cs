using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace ClientRadzen.NewPages.BudgetItems.MWOApproved;
#nullable disable
public partial class NewBudgetItemsMWOApprovedMain
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    [Inject]
    private INewBudgetItemService Service { get; set; }
    [Inject]
    private INewPurchaseOrderService PurchaseOrderService { get; set; }
    public NewMWOApprovedReponse Response { get; set; } = new();
    public string nameFilter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await UpdateAll();
    }
    async Task UpdateAll()
    {
        var result = await Service.GetAllMWOApprovedWithItems(MWOId);
        if (result.Succeeded)
        {
            Response = result.Data;
        }
        StateHasChanged();
    }
    Func<NewBudgetItemMWOApprovedResponse, bool> fiterexpresion => x =>
      x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
      x.Nomeclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
      x.BrandName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
      x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.PurchaseOrderItems.Any(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.PurchaseOrderNumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.Supplier.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));
    public List<NewBudgetItemMWOApprovedResponse> FilteredItems => Response.BudgetItems.Count == 0 ? new() :
        Response.BudgetItems?.Where(fiterexpresion).OrderBy(x => x.Nomeclatore).ToList();
    public void ChangeFilter(string arg)
    {
        nameFilter = arg;
    }
    public void ShowSapAlignmentofMWO()
    {
        _NavigationManager.NavigateTo($"/SapAdjustListByMWO/{Response.MWOId}");

    }
    public async Task UnApproveMWO()
    {
        NewMWOUnApproveRequest UnApproveMWORequest = new()
        {
            MWOId = Response.MWOId,
            Name = Response.Name,

        };
        var resultDialog = await DialogService.Confirm($"Are you sure Un Approve {UnApproveMWORequest.Name}?", "Confirm",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {

            var result = await Service.UnApproveMWO(UnApproveMWORequest);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                CancelAsync();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }
        }
    }
    private void CancelAsync()
    {
        Navigation.NavigateBack();

    }
    public async Task ExporApprovedToExcel()
    {
        //var result = await Service.ExporApprovedToExcel(MWOId);
        //if (result.Succeeded)
        //{
        //    var downloadresult = await blazorDownloadFileService.DownloadFile(result.Data.ExportFileName,
        //       result.Data.Data, contentType: result.Data.ContentType);
        //    if (downloadresult.Succeeded)
        //    {
        //        MainApp.NotifyMessage(NotificationSeverity.Success, "Export Excel", new() { "Export excel succesfully" });


        //    }
        //}


    }
    public void CreatePurchaseOrder(NewBudgetItemMWOApprovedResponse approvedResponse)
    {
        //if (approvedResponse.CreateTaxPurchaseOrder)
        //{
        //    _NavigationManager.NavigateTo($"/CreateTaxPurchaseOrder/{approvedResponse.BudgetItemId}");
        //}
        //else if (approvedResponse.CreateCapitalizedSalaries)
        //{
        //    _NavigationManager.NavigateTo($"/CreateCapitalizedSalary/{approvedResponse.BudgetItemId}");
        //}

        //else if (approvedResponse.CreateNormalPurchaseOrder)
        //{
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.Create}/{approvedResponse.BudgetItemId}");
        //}

    }
    public void EditPurchaseOrder(Guid PurchaseOrderId, bool IsTaxEditable, bool IsCapitalizedSalary, PurchaseOrderStatusEnum PurchaseOrderStatus)
    {
        //if (IsTaxEditable)
        //{
        //    _NavigationManager.NavigateTo($"/EditTaxPurchaseOrder/{PurchaseOrderId}");

        //}
        //else if (IsCapitalizedSalary)
        //{
        //    _NavigationManager.NavigateTo($"/EditPurchaseOrderCapitalizedSalary/{PurchaseOrderId}");
        //}
        //else if (PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id)
        //{
        //    _NavigationManager.NavigateTo($"/EditPurchaseOrderCreated/{PurchaseOrderId}");
        //}
        //else if (PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
        //{
        //    _NavigationManager.NavigateTo($"/EditPurchaseOrderApproved/{PurchaseOrderId}");
        //}
        //else if (PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id || PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id)
        //{
        //    _NavigationManager.NavigateTo($"/EditPurchaseOrderClosed/{PurchaseOrderId}");
        //}

    }
    public void ApprovedPurchaseOrder(Guid PurchaseOrderId)
    {
        _NavigationManager.NavigateTo($"/ApprovePurchaseOrder/{PurchaseOrderId}");
    }
    public void ReceivePurchaseOrder(Guid PurchaseOrderId)
    {
        //_NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{PurchaseOrderId}");
    }
    public async Task RemovePurchaseorder(NewPriorPurchaseOrderItemResponse purchaseOrderItemResponse)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {purchaseOrderItemResponse.PurchaseOrderLegendToDelete}?", "Confirm Delete",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            NewPurchaseOrderDeleteRequest request = new NewPurchaseOrderDeleteRequest()
            {
                PurchaseOrderId = purchaseOrderItemResponse.PurchaseOrderId,
                Name = purchaseOrderItemResponse.PurchaseOrderNumber,
            };
            var result = await PurchaseOrderService.DeletePurchaseOrderAsync(request);
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
