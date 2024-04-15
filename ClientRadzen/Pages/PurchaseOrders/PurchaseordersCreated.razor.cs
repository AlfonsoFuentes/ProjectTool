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
    public partial class PurchaseordersCreated
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        [CascadingParameter]
        public PurchaseOrderDataMain MainPO { get; set; }


      
        string nameFilter => MainPO.nameFilter;

        Func<PurchaseOrderResponse, bool> fiterexpresion => x =>
            x.PurchaseOrderStatus.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PurchaseorderName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PONumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
            x.SupplierNickName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.SupplierName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.MWOName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.VendorCode.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.AccountAssigment.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
        IEnumerable<PurchaseOrderResponse> FilteredItems => OriginalData.Where(fiterexpresion).AsQueryable();
        IEnumerable<PurchaseOrderResponse> OriginalData => MainPO.PurchaseordersCreated==null?new List<PurchaseOrderResponse>(): MainPO.PurchaseordersCreated;

        
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
        async Task RemovePurchaseorder(PurchaseOrderResponse selectedRow)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure delete {selectedRow.PurchaseRequisition}?", "Confirm Delete",
               new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await MainPO.Service.DeletePurchaseOrder(selectedRow.PurchaseOrderId);
                if (result.Succeeded)
                {
                    MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                    await MainPO.UpdateAll();

                }
                else
                {
                    MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }

            }
        }
    }
}
