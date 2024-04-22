﻿using Client.Infrastructure.Managers.BudgetItems;
using ClientRadzen.Pages.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
#nullable disable
namespace ClientRadzen.Pages.BudgetItems
{
    public partial class ApprovedItemsDataList
    {
        [Inject]
        private IBudgetItemService Service { get; set; }
        [CascadingParameter]
        public App MainApp { get; set; }
        ListApprovedBudgetItemsResponse Response { get; set; } = new();

        string nameFilter = string.Empty;
        Func<BudgetItemApprovedResponse, bool> fiterexpresion => x =>
        x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.Nomenclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.Brand.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)||
        x.PurchaseOrders.Any(x => x.PurchaseorderName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
        x.PurchaseOrders.Any(x => x.PONumber.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
        x.PurchaseOrders.Any(x => x.PurchaseRequisition.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
        x.PurchaseOrders.Any(x => x.SupplierName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)) ||
        x.PurchaseOrders.Any(x => x.SupplierNickName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase))
        ;
        IQueryable<BudgetItemApprovedResponse> FilteredItems => Response.BudgetItems?.Where(fiterexpresion).AsQueryable();
        [Parameter]
        public Guid MWOId { get; set; }

        protected override async Task OnInitializedAsync()
        {


            var result = await Service.GetApprovedBudgetItemsByMWO(MWOId);
            if (result.Succeeded)
            {
                Response = result.Data;
            }
        }

        void CreatePurchaseOrder(BudgetItemApprovedResponse approvedResponse)
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

        BudgetItemApprovedResponse seletedRow = null;
        void ShowPurchaseOrders(BudgetItemApprovedResponse approvedResponse)
        {
            seletedRow = approvedResponse;

        }
        void HidePurchaseOrders()
        {
            seletedRow = null!;

        }
        private void GoToHome()
        {
            _NavigationManager.NavigateTo("/");
        }
        private void GoToMWOPage()
        {
            _NavigationManager.NavigateTo("/MWODataMain");
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
        void ApprovedPurchaseOrder(PurchaseOrderResponse order)
        {
            _NavigationManager.NavigateTo($"/ApprovePurchaseOrder/{order.PurchaseOrderId}");
        }
        void ReceivePurchaseOrder(PurchaseOrderResponse order)
        {
            _NavigationManager.NavigateTo($"/ReceivePurchaseOrder/{order.PurchaseOrderId}");
        }
        async Task ExporNotApprovedToExcel()
        {
            var result = await Service.ExporApprovedToExcel(MWOId);
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
}
