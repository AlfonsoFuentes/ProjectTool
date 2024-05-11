using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Enums.BudgetItemTypes;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.EBPReport;
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
    [Inject]
    private INewMWOService MWOService { get; set; }
    public NewMWOApprovedReponse Response { get; set; } = new();
    public string nameFilter { get; set; } = string.Empty;

    public NewEBPReportResponse EBPReport { get; set; }=new();
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
            EBPReport = Response.EBPReportResponse;
        }
        var resultEBP=await MWOService.GetMWOEBPReportById(MWOId);
        if(resultEBP.Succeeded)
        {
            //EBPReport=resultEBP.Data;
        }
        StateHasChanged();
    }
    Func<NewBudgetItemMWOApprovedResponse, bool> fiterexpresion => x =>
      x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
      x.Nomenclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
      x.BrandName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
      x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.PurchaseOrderItems.Any(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.PurchaseOrderNumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.Supplier.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));
    public List<NewBudgetItemMWOApprovedResponse> FilteredItems => Response.BudgetItems.Count == 0 ? new() :
        Response.BudgetItems?.Where(fiterexpresion).OrderBy(x => x.Nomenclatore).ToList();
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
        if (approvedResponse.IsCapitalizedSalary)
        {
            _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.CreateSalary}/{approvedResponse.BudgetItemId}");
        }
        else
        {
            _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.Create}/{approvedResponse.BudgetItemId}");
        }


    }
    public void EditPurchaseOrderCreated(NewPriorPurchaseOrderItemResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditCreate}/{selectedRow.PurchaseOrderId}");

    }
    public void EditPurchaseOrderClosed(NewPriorPurchaseOrderItemResponse selectedRow)
    {

        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditReceive}/{selectedRow.PurchaseOrderId}");
    }
    public void EditPurchaseOrderSalary(NewPriorPurchaseOrderItemResponse selectedRow)
    {

        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditSalary}/{selectedRow.PurchaseOrderId}");
    }
    public void EditPurchaseOrderApproved(NewPriorPurchaseOrderItemResponse selectedRow)
    {

        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditApproved}/{selectedRow.PurchaseOrderId}");
    }
    public void EditPurchaseOrderReceiving(NewPriorPurchaseOrderItemResponse selectedRow)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.EditReceive}/{selectedRow.PurchaseOrderId}");

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
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.Approve}/{PurchaseOrderId}");
    }
    public void ReceivePurchaseOrder(Guid PurchaseOrderId)
    {
        _NavigationManager.NavigateTo($"/{PageName.PurchaseOrder.Receive}/{PurchaseOrderId}");
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
