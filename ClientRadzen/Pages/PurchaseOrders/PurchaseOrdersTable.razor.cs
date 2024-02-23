#nullable disable
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;
using Shared.Models.PurchaseOrders.Responses;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class PurchaseOrdersTable
    {
        [Inject]
        private IPurchaseOrderService Service { get; set; } = null!;

        PurchaseOrdersListResponse Model = new();
        IList<PurchaseOrderResponse> selectedPurchaseOrders;
        List<PurchaseOrderResponse> OriginalData => Model.Purchaseorders;
        IQueryable<PurchaseOrderResponse> FilteredItems { get; set; } = null!;

        RadzenDataGrid<PurchaseOrderResponse> grid;
        protected override async Task OnInitializedAsync()
        {
            await ShowLoading();
            await UpdateAll();
        }
        bool isLoading = false;

        async Task ShowLoading()
        {
            isLoading = true;

            await Task.Yield();

            isLoading = false;
        }
        async Task UpdateAll()
        {
            var result = await Service.GetAllPurchaseOrders();
            if (result.Succeeded)
            {
                Model = result.Data;
                FilteredItems = OriginalData.AsQueryable();
            }
        }
        PurchaseOrderResponse selectedRow = null!;
        void OnRowClick(DataGridRowMouseEventArgs<PurchaseOrderResponse> args)
        {

            selectedRow = args.Data == null ? null! : args.Data == selectedRow ? null! : args.Data;
        }
        bool DisableButtonPurchaseorderToApprove => selectedRow == null ? true :
            selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? false : true;

        bool DisableButtonPurchaseorderToReceive => selectedRow == null ? true :
           selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? true :
            selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id ? true : false;
        void EditPurchaseOrder()
        {
            if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id)
            {
                _NavigationManager.NavigateTo($"/EditPurchaseOrderCreated/{selectedRow.PurchaseorderId}");
            }
            else if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
            {
                _NavigationManager.NavigateTo($"/EditPurchaseOrderApproved/{selectedRow.PurchaseorderId}");
            }


        }
        void ApprovePurchaseOrder()
        {

            _NavigationManager.NavigateTo($"/ApprovePurchaseOrder/{selectedRow.PurchaseorderId}");
        }
        void ReceivePurchaseorder()
        {
            _NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{selectedRow.PurchaseorderId}");
        }
        void OnSearch(string search)
        {
            _search = search;

            if (_search == string.Empty)
            {
                FilteredItems = OriginalData.AsQueryable();

            }
            else
            {
                Func<PurchaseOrderResponse, bool> Criteria = x =>
                x.PurchaseOrderStatusName.ToLower().Contains(_search.ToLower()) ||
                x.AccountAssigment.ToLower().Contains(_search.ToLower()) ||
                x.PurchaseRequisition.ToLower().Contains(_search.ToLower()) ||
                x.PONumber.ToLower().Contains(_search.ToLower()) ||
                x.PurchaseorderName.ToLower().Contains(_search.ToLower()) ||
                x.Supplier.ToLower().Contains(_search.ToLower()) ||
                x.VendorCode.ToLower().Contains(_search.ToLower()) ||
                x.MWOName.ToLower().Contains(_search.ToLower());
                FilteredItems = OriginalData.Where(Criteria).AsQueryable();

            }

        }
    }
}
