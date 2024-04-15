﻿using Blazored.FluentValidation;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Client.Infrastructure.Managers.Suppliers;
using ClientRadzen.Pages.Suppliers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Shared.Models.BudgetItems;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.Suppliers;

#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class EditPurchaseOrderApproved
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Parameter]
        public Guid PurchaseOrderId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }


        [Inject]
        private ISupplierService SupplierService { get; set; }


        EditPurchaseOrderRegularApprovedRequest Model = null!;
        List<BudgetItemApprovedResponse> OriginalBudgetItems { get; set; } = new();
        List<BudgetItemApprovedResponse> BudgetItems => GetBudgetItems();
        List<BudgetItemApprovedResponse> GetBudgetItems()
        {
            List<BudgetItemApprovedResponse> response = new();
            foreach (var row in OriginalBudgetItems)
            {
                if (!Model.PurchaseOrderItems.Any(x => x.BudgetItemId == row.BudgetItemId))
                {
                    response.Add(row);
                }
            }

            return response;
        }
        List<SupplierResponse> Suppliers { get; set; } = new();


        FluentValidationValidator _fluentValidationValidator = null!;

        BudgetItemApprovedResponse ItemToAdd;

        
       protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetPurchaseOrderApprovedToEdit(PurchaseOrderId);
            if (result.Succeeded)
            {
                Model = result.Data;
               
                AddBlankItem();
            }

            var resultSupplier = await SupplierService.GetAllSupplier();
            if (resultSupplier.Succeeded)
            {
                Suppliers = resultSupplier.Data;

            }
            var resultBudgetItems = await Service.GetBudgetItemsToCreatePurchaseOrder(Model.MainBudgetItemId);
            if (resultBudgetItems.Succeeded)
            {

                OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;

            }
          
            //StateHasChanged();
        }

        async Task<bool> ValidateAsync()
        {
            Validated = await _fluentValidationValidator.ValidateAsync();
            return Validated;
        }
        bool Validated = false;

        public async Task SaveAsync()
        {
            var result = await Service.EditPurchaseOrderApproved(Model);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                Cancel();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


            }

        }

        void Cancel()
        {
            Navigation.NavigateBack();
        }
        async Task CreateSupplier()
        {
            var result = await DialogService.OpenAsync<CreateSupplierForPurchaseOrderDialog>($"Create Supplier",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "400px", Height = "450px", Resizable = true, Draggable = true });
            if (result != null)
            {
                await SetSupplier(result as SupplierResponse);
                var resultData = await SupplierService.GetAllSupplier();
                if (resultData.Succeeded)
                {
                    Suppliers = resultData.Data;

                }


            }
        }
        bool UpdateCurrentTRM = false;
        public void ClickUpdateCurrentTRM()
        {
            Model.OldCurrencyDate = Model.CurrencyDate;
            Model.OldTRMUSDCOP = Model.USDCOP;
            Model.OldTRMUSDEUR = Model.USDEUR;

            Model.CurrencyDate = DateTime.UtcNow;
            Model.USDCOP = MainApp.RateList.COP;
            Model.USDEUR = MainApp.RateList.EUR;
            UpdateCurrentTRM = true;
        }
        public void ClickUpdateOldTRM()
        {


            Model.CurrencyDate = Model.OldCurrencyDate;
            Model.USDCOP = Model.OldTRMUSDCOP;
            Model.USDEUR = Model.OldTRMUSDEUR;
            UpdateCurrentTRM = false;
        }
        public async Task ChangePONumber(string ponumber)
        {

            Model.PONumber = ponumber.Trim();
            await ValidateAsync();

        }
        public async Task ChangedExpectedDate(DateTime? expected)
        {
            Model.ExpectedDate = expected;
            await ValidateAsync();
        }
        public async Task ChangeTRMUSDEUR(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdeur = Model.USDEUR;
            if (!double.TryParse(arg, out usdeur))
            {

            }
            Model.USDEUR = usdeur;
            foreach (var item in Model.PurchaseOrderItemNoBlank)
            {
                item.SetUSDEUR(usdeur);
            }
            await ValidateAsync();
        }
        public async Task ChangeTRMUSDCOP(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdcop = Model.USDCOP;
            if (!double.TryParse(arg, out usdcop))
            {

            }
            Model.USDCOP = usdcop;
            foreach (var item in Model.PurchaseOrderItemNoBlank)
            {
                item.SetUSDCOP(usdcop);
            }
            await ValidateAsync();
        }
        public async Task ChangeName(string name)
        {

            Model.PurchaseorderName = name;
            if (Model.PurchaseOrderItemNoBlank.Count == 1)
            {
                Model.PurchaseOrderItems[0].Name = Model.PurchaseorderName;
            }
            await ValidateAsync();
        }
        public async Task ChangeName(PurchaseOrderItemRequest model, string name)
        {

            model.Name = name;

            if (Model.PurchaseOrderItemNoBlank.Count == 1)
            {
                Model.PurchaseorderName = name;
            }
            await ValidateAsync();
        }
        public async Task ChangeQuantity(PurchaseOrderItemRequest item, string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double quantity = item.Quantity;
            if (!double.TryParse(arg, out quantity))
            {

            }
            item.Quantity = quantity;
            await ValidateAsync();
        }
        public async Task ChangeCurrencyValue(PurchaseOrderItemRequest item, string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double currencyvalue = item.Quantity;
            if (!double.TryParse(arg, out currencyvalue))
            {

            }
            item.CurrencyUnitaryValue = currencyvalue;
            await ValidateAsync();
        }
        bool debug = true;
        RadzenDataGrid<PurchaseOrderItemRequest> ordersGrid = null!;
        Density Density = Density.Compact;

        int MaxColumn = 12;
        int MediumColumn = 6;

        async Task EditRow(DataGridRowMouseEventArgs<PurchaseOrderItemRequest> order)
        {

            await ordersGrid.EditRow(order.Data);

        }
        async Task ClickCell(DataGridCellMouseEventArgs<PurchaseOrderItemRequest> order)
        {
            var column = order.Column;
            var row = order.Data.BudgetItemId == Guid.Empty;
            if (order.Column.Property == "BudgetItemName" && order.Data.BudgetItemId == Guid.Empty)
            {

                await ordersGrid.EditRow(order.Data);
                await ValidateAsync();
            }


        }
        async Task EditRowButton(PurchaseOrderItemRequest order)
        {

            await ordersGrid.EditRow(order);
        }
        async Task DeleteRow(PurchaseOrderItemRequest order)
        {


            if (Model.PurchaseOrderItems.Contains(order) && order.BudgetItemId != Model.MainBudgetItemId)
            {

                Model.PurchaseOrderItems.Remove(order);


                if (BudgetItems.Count > 0)
                {
                    AddBlankItem();
                }
                await ordersGrid.Reload();

            }
            else
            {
                ordersGrid.CancelEditRow(order);
                await ordersGrid.Reload();
            }
            await ValidateAsync();
        }
        async Task SaveRow(PurchaseOrderItemRequest order)
        {
            await ValidateAsync();
            await ordersGrid.UpdateRow(order);
        }

        async Task CancelEdit(PurchaseOrderItemRequest order)
        {

            await ValidateAsync();
            ordersGrid.CancelEditRow(order);


        }

        async Task OnKeyDownCurrency(KeyboardEventArgs arg, PurchaseOrderItemRequest order)
        {
            if (arg.Key == "Enter")
            {
                await ordersGrid.UpdateRow(order);
                await ValidateAsync();
            }
        }

        async Task AddNewItem(PurchaseOrderItemRequest order)
        {
            AddBudgetItem(ItemToAdd!);


            if (BudgetItems.Count > 0)
            {
                AddBlankItem();
            }
            await ordersGrid.UpdateRow(order);
            await ordersGrid.Reload();
            await ValidateAsync();
            ItemToAdd = null!;
        }
       
        public void AddBlankItem()
        {
            if (!Model.PurchaseOrderItems.Any(x => x.BudgetItemId == Guid.Empty))
            {
                Model.PurchaseOrderItems.Add(new PurchaseOrderItemRequest());
            }


        }
        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            if (Model.PurchaseOrderItems.Count == 0)
                Model.PurchaseOrderItems.Add(new PurchaseOrderItemRequest());

            Model.PurchaseOrderItems[Model.PurchaseOrderItems.Count - 1].SetBudgetItem(response, Model.USDCOP, Model.USDEUR);


        }
        public async Task SetSupplier(SupplierResponse? _Supplier)
        {

            if (_Supplier == null)
            {
                Model.PurchaseOrderCurrency = CurrencyEnum.COP;
                return;
            }
            Model.Supplier = _Supplier;
            Model.PurchaseOrderCurrency = _Supplier.SupplierCurrency;

            await ValidateAsync();

        }
        public async Task ChangePR(string purchaserequisition)
        {
            Model.PurchaseRequisition = purchaserequisition;
            await ValidateAsync();
        }
        public async Task ChangeQuote(string quoteno)
        {
            Model.QuoteNo = quoteno;
            await ValidateAsync();
        }
    }
}
