using Client.Infrastructure.Managers.BudgetItems;
using Shared.Enums.PurchaseorderStatus;
#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWOApproved
{
    [Inject]
    private IMWOService Service { get; set; }
    [Inject]
    private IPurchaseOrderService PurchaseorderService { get; set; }
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    public string nameFilter = string.Empty;
    MWOApprovedWithBudgetItemsResponse Response = new();
    MWOEBPResponse MWOEBPResponse { get; set; } = new();
    [Inject]
    private IBudgetItemService BudgetItemService { get; set; }
    protected override async Task OnInitializedAsync()
    {

        await UpdateAll();
    }
    async Task UpdateAll()
    {
        var result = await Service.GetMWOApprovedById(MWOId);
        if (result.Succeeded)
        {
            Response = result.Data;

        }
        var resultEbp = await Service.GetMWOEBPReport(MWOId);
        if (resultEbp.Succeeded)
        {
            MWOEBPResponse = resultEbp.Data;
        }
    }

    public IQueryable<NewBudgetItemsWithPurchaseorders> FilteredItems => GetFilteredItems();
    Func<NewBudgetItemsWithPurchaseorders, bool> fiterexpresion => x =>
       x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.Nomenclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.PurchaseOrderItems.Any(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.PurchaseOrderNumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrderItems.Any(x => x.Supplier.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase))
      ;

    IQueryable<NewBudgetItemsWithPurchaseorders> GetFilteredItems()
    {
        var result = Response.BudgetItems.Where(fiterexpresion).AsQueryable();



        return result;
    }
    public void CreatePurchaseOrder(NewBudgetItemsWithPurchaseorders approvedResponse)
    {
        if (approvedResponse.CreateTaxPurchaseOrder)
        {
            _NavigationManager.NavigateTo($"/CreateTaxPurchaseOrder/{approvedResponse.BudgetItemId}");
        }
        else if (approvedResponse.CreateCapitalizedSalaries)
        {
            _NavigationManager.NavigateTo($"/CreateCapitalizedSalary/{approvedResponse.BudgetItemId}");
        }

        else if (approvedResponse.CreateNormalPurchaseOrder)
        {
            _NavigationManager.NavigateTo($"/CreatePurchaseOrder/{approvedResponse.BudgetItemId}");
        }

    }


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
    public void ShowSapAlignmentofMWO()
    {
        _NavigationManager.NavigateTo($"/SapAdjustListByMWO/{Response.Id}");

    }
    public async Task UnApproveMWO()
    {
        UnApproveMWORequest UnApproveMWORequest = new()
        {
            MWOId = Response.Id,
            Name = Response.Name,
            CECName = Response.CECName,
        };
        var resultDialog = await DialogService.Confirm($"Are you sure Un Approve {UnApproveMWORequest.Name}?", "Confirm",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {

            var result = await Service.UnApproveMWO(UnApproveMWORequest);
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
                await UpdateAll();

            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }
    }
    public async Task ExporApprovedToExcel()
    {
        var result = await BudgetItemService.ExporApprovedToExcel(MWOId);
        if (result.Succeeded)
        {
            var downloadresult = await blazorDownloadFileService.DownloadFile(result.Data.ExportFileName,
               result.Data.Data, contentType: result.Data.ContentType);
            if (downloadresult.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Export Excel", new() { "Export excel succesfully" });


            }
        }


    }
}
