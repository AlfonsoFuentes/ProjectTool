using Microsoft.AspNetCore.Components;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWOApprovedWIthBudgetItemsPurchaseOrder
{
    [Inject]
    private IMWOService Service { get; set; }

    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    string nameFilter = string.Empty;
    MWOApprovedResponse Response = null!;
    protected override async Task OnInitializedAsync()
    {
        var result = await Service.GetMWOApprovedById(MWOId);
        if(result.Succeeded)
        {
            Response = result.Data;
           
        }

    }
    private void GoToHome()
    {
        _NavigationManager.NavigateTo("/");
    }
    private void GoToMWOPage()
    {
        _NavigationManager.NavigateTo("/MWODataMain");
    }
    IQueryable<BudgetItemApprovedResponse> FilteredItems => Response.BudgetItems?.Where(fiterexpresion).AsQueryable();
    Func<BudgetItemApprovedResponse, bool> fiterexpresion => x =>
       x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.Nomenclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.Brand.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.PurchaseOrders.Any(x => x.PurchaseorderName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrders.Any(x => x.PONumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrders.Any(x => x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
       x.PurchaseOrders.Any(x => x.Supplier.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase))
       ;
    void CreatePurchaseOrder(BudgetItemApprovedResponse approvedResponse)
    {
        if (approvedResponse.CreateNormalPurchaseOrder)
        {
            _NavigationManager.NavigateTo($"/CreatePurchaseOrder/{approvedResponse.BudgetItemId}");
        }
        else if (approvedResponse.CreateTaxPurchaseOrder)
        {
            _NavigationManager.NavigateTo($"/CreateTaxPurchaseOrder/{approvedResponse.BudgetItemId}");
        }
        else if (approvedResponse.CreateCapitalizedSalaries)
        {
            _NavigationManager.NavigateTo($"/CreateCapitalizedSalary/{approvedResponse.BudgetItemId}");
        }

    }
    BudgetItemApprovedResponse seletedRow = null;
    void ShowPurchaseOrders(BudgetItemApprovedResponse approvedResponse)
    {
        seletedRow = approvedResponse;

    }
    void HidePurchaseOrders()
    {
        seletedRow = null!;

    }
    void ApprovedPurchaseOrder(PurchaseOrderResponse order)
    {
        _NavigationManager.NavigateTo($"/ApprovePurchaseOrder/{order.PurchaseOrderId}");
    }
    void ReceivePurchaseOrder(PurchaseOrderResponse order)
    {
        _NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{order.PurchaseOrderId}");
    }
    void EditPurchaseOrder(PurchaseOrderResponse order)
    {
        if (order.IsTaxNoProductive)
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
}
