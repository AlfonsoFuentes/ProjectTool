﻿using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using Radzen;
using Shared.Models.BudgetItems;
#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class PurchaseOrderReceive
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        [CascadingParameter]
        public PurchaseOrderDataMain MainPO { get; set; }



        string nameFilter => MainPO.nameFilter;

        Func<NewPurchaseOrderApprovedResponse, bool> fiterexpresion => x =>
            x.PurchaseOrderStatus.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PurchaseorderName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.PurchaseOrderNumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
            x.SupplierNickName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
         x.SupplierName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.MWOName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
             x.VendorCode.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.AccountAssigment.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
        IEnumerable<NewPurchaseOrderApprovedResponse> FilteredItems => OriginalData.Where(fiterexpresion).AsQueryable();
        IEnumerable<NewPurchaseOrderApprovedResponse> OriginalData => MainPO.PurchaseordersToReceive == null ? new List<NewPurchaseOrderApprovedResponse>() : MainPO.PurchaseordersToReceive;


        void EditPurchaseOrder(NewPurchaseOrderApprovedResponse selectedRow)
        {
            if (selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id)
            {
                _NavigationManager.NavigateTo($"/EditPurchaseOrderApproved/{selectedRow.PurchaseOrderId}");
            }
            else if(selectedRow.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id)
            {
                _NavigationManager.NavigateTo($"/EditPurchaseOrderClosed/{selectedRow.PurchaseOrderId}");
            }



        }

        void ReceivePurchaseorder(NewPurchaseOrderApprovedResponse selectedRow)
        {
            _NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{selectedRow.PurchaseOrderId}");
        }

        async Task RemovePurchaseorder(NewPurchaseOrderApprovedResponse selectedRow)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure delete {selectedRow.PurchaseOrderNumber}?", "Confirm Delete",
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
