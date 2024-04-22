using Client.Infrastructure.Managers.BudgetItems;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWOApprovedWIthBudgetItemsPurchaseOrder
{
    [Inject]
    private IMWOService Service { get; set; }
    [Inject]
    private IPurchaseOrderService PurchaseorderService { get; set; }
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    string nameFilter = string.Empty;
    MWOApprovedWithBudgetItemsResponse Response = null!;
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
    }
    
    IQueryable<NewBudgetItemsWithPurchaseorders> FilteredItems => GetFilteredItems();
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
    void CreatePurchaseOrder(NewBudgetItemsWithPurchaseorders approvedResponse)
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
    NewBudgetItemsWithPurchaseorders seletedRow = null;
    void ShowPurchaseOrders(NewBudgetItemsWithPurchaseorders approvedResponse)
    {
        seletedRow = approvedResponse;

    }
    void HidePurchaseOrders()
    {
        seletedRow = null!;

    }
    void ApprovedPurchaseOrder(NewPurchaseOrderItemResponse order)
    {
        _NavigationManager.NavigateTo($"/ApprovePurchaseOrder/{order.PurchaseOrderId}");
    }
    void ReceivePurchaseOrder(NewPurchaseOrderItemResponse order)
    {
        _NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{order.PurchaseOrderId}");
    }
    void EditPurchaseOrder(NewPurchaseOrderItemResponse order)
    {
        if (order.IsTaxEditable)
        {
            _NavigationManager.NavigateTo($"/EditTaxPurchaseOrder/{order.PurchaseOrderId}");

        }
        else if (order.IsCapitalizedSalary)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderCapitalizedSalary/{order.PurchaseOrderId}");
        }
        else if (order.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderCreated/{order.PurchaseOrderId}");
        }
        else if (order.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderApproved/{order.PurchaseOrderId}");
        }
        else if (order.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id || order.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id)
        {
            _NavigationManager.NavigateTo($"/EditPurchaseOrderClosed/{order.PurchaseOrderId}");
        }

    }
    public void ShowSapAlignmentofMWO()
    {
        _NavigationManager.NavigateTo($"/SapAdjustListByMWO/{Response.Id}");

    }
    async Task RemovePurchaseorder(NewPurchaseOrderItemResponse selectedRow)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {selectedRow.PurchaseOrderNumber}?", "Confirm Delete",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            var result = await PurchaseorderService.DeletePurchaseOrder(selectedRow.PurchaseOrderId);
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
    async Task ExporApprovedToExcel()
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
