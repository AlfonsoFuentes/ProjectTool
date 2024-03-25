using Azure;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class PurchaseOrdersDataList
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; } = null!;

        PurchaseOrdersListResponse Response = new();
        string nameFilter = string.Empty;

        Func<PurchaseOrderResponse, bool> fiterexpresion => x =>
            x.PurchaseOrderStatus.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PurchaseorderName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PONumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.Supplier.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.MWOName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.VendorCode.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.AccountAssigment.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
        IEnumerable<PurchaseOrderResponse> FilteredItems => OriginalData.Where(fiterexpresion).AsQueryable();
        List<PurchaseOrderResponse> OriginalData => Response.Purchaseorders;

        protected override async Task OnInitializedAsync()
        {

            await UpdateAll();
        }

        async Task UpdateAll()
        {
            var result = await Service.GetAllPurchaseOrders();
            if (result.Succeeded)
            {
                Response = result.Data;

            }
        }
        void EditPurchaseOrder(PurchaseOrderResponse selectedRow)
        {
            if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id)
            {
                _NavigationManager.NavigateTo($"/EditPurchaseOrderCreated/{selectedRow.PurchaseOrderId}");
            }
            else if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
            {
                _NavigationManager.NavigateTo($"/EditPurchaseOrderApproved/{selectedRow.PurchaseOrderId}");
            }
            else if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id)
            {
                _NavigationManager.NavigateTo($"/EditPurchaseOrderClosed/{selectedRow.PurchaseOrderId}");
            }


        }
        void ApprovePurchaseOrder(PurchaseOrderResponse selectedRow)
        {

            _NavigationManager.NavigateTo($"/ApprovePurchaseOrder/{selectedRow.PurchaseOrderId}");
        }
        void ReceivePurchaseorder(PurchaseOrderResponse selectedRow)
        {
            _NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{selectedRow.PurchaseOrderId}");
        }
        private void AddBlankPurchaseorder()
        {
            _NavigationManager.NavigateTo($"/CreateBlankPurchaseOrder");
        }
        private void GoToHome()
        {
            _NavigationManager.NavigateTo("/");
        }
        private void GoToMWOPage()
        {
            _NavigationManager.NavigateTo("/MWODataMain");
        }
        bool EditRow = false;
        PurchaseOrderResponse selectedRow;
        void OnDoubleClick(PurchaseOrderResponse _selectedRow)
        {
            EditRow = true;
            selectedRow = _selectedRow;


        }
        async Task OnKeyDown(KeyboardEventArgs arg, PurchaseOrderResponse order)
        {
            if (arg.Key == "Enter")
            {
                await SaveAsync(order);


            }
            else if (arg.Key == "Escape")
            {
                EditRow = false;
            }
        }
        async Task SaveAsync(PurchaseOrderResponse order)
        {
            if (EditRow)
            {
                await UpdateAsync(order);

            }
        }
        async Task UpdateAsync(PurchaseOrderResponse order)
        {
            //UpdatePurchaseOrderMinimalRequest Model = new()
            //{
            //    PurchaseOrderId=order.PurchaseorderId,
            //    PurchaseOrderName=order.PurchaseorderName,


            //};
            //var result = await Service.UpdatePurchaseOrderMinimal(Model);
            //if (result.Succeeded)
            //{
            //    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
            //    await UpdateAll();
            //    EditRow = false;
            //    selectedRow = null!;
            //}
            //else
            //{
            //    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);

            //}
        }
    }
}
