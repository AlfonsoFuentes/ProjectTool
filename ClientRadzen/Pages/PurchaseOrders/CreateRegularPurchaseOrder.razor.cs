using Client.Infrastructure.Managers.BudgetItems;
using Shared.Enums.Currencies;

#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class CreateRegularPurchaseOrder
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Parameter]
        public Guid BudgetItemId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }

        [Inject]
        private INewSupplierService SupplierService { get; set; }
        [Inject]
        private IBudgetItemService BudgetItemService { get; set; }

        CreatedRegularPurchaseOrderRequest Model = null;
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
        List<NewSupplierResponse> Suppliers { get; set; } = new();

        FluentValidationValidator _fluentValidationValidator = null!;


        BudgetItemApprovedResponse ItemToAdd;


        protected override async Task OnInitializedAsync()
        {
            var resultSupplier = await SupplierService.GetAllSupplier();
            if (resultSupplier.Succeeded)
            {
                Suppliers = resultSupplier.Data.Suppliers;

            }
            Model = new();
            Model.USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
            Model.USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
            var result = await BudgetItemService.GetApprovedBudgetItemsById(BudgetItemId);
            if (result.Succeeded)
            {
               
                SetMainBudgetItem(result.Data);

            }

            var resultBudgetItems = await Service.GetBudgetItemsToCreatePurchaseOrder(Model.MainBudgetItemId);
            if (resultBudgetItems.Succeeded)
            {

                OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;
            }
            StateHasChanged();
        }


        async Task<bool> ValidateAsync()
        {
            Validated = await _fluentValidationValidator.ValidateAsync();
            return Validated;
        }
        bool Validated = false;
        public async Task SaveAsync()
        {
            var result = await Service.CreateRegularPurchaseOrder(Model);
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
            var result = await DialogService.OpenAsync<NewCreateSupplierDialog>($"Create Supplier",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "400px", Height = "450px", Resizable = true, Draggable = true });
            if (result != null)
            {
                await SetSupplier((result as NewSupplierResponse));
                var resultData = await SupplierService.GetAllSupplier();
                if (resultData.Succeeded)
                {
                    Suppliers = resultData.Data.Suppliers;

                }


            }
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
                item.TRMUSDEUR = usdeur;
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
                item.TRMUSDCOP = usdcop;
            }
            await ValidateAsync();
        }
        public void SetMainBudgetItem(BudgetItemApprovedResponse budgetItem)
        {
            Model.MWOId = budgetItem.MWOId;
            Model.MWOName = budgetItem.MWOName;
            Model.CostCenter = budgetItem.CostCenter;
            Model.MWOCECName = budgetItem.MWOCECName;
            Model.IsAssetProductive = budgetItem.IsMWOAssetProductive;
            Model.MainBudgetItem = budgetItem;
            Model.QuoteCurrency = CurrencyEnum.COP;
            AddBudgetItem(budgetItem);
            AddBlankItem();
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
        public async Task ChangeQuoteCurrency(CurrencyEnum currencyEnum)
        {

            foreach (var item in Model.PurchaseOrderItems)
            {
                item.ChangeCurrency(currencyEnum);
            }
            await ValidateAsync();
        }

        public async Task SetSupplier(NewSupplierResponse? _Supplier)
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

        public void AddMWOBudgetItem(BudgetItemApprovedResponse budgetItem)
        {
            AddBudgetItem(budgetItem);
            AddBlankItem();
        }

        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            if (Model.PurchaseOrderItems.Count == 0)
                Model.PurchaseOrderItems.Add(new PurchaseOrderItemRequest());

            Model.PurchaseOrderItems[Model.PurchaseOrderItems.Count - 1].SetBudgetItem(response, Model.USDCOP, Model.USDEUR);

            Model.PurchaseOrderItems[Model.PurchaseOrderItems.Count - 1].PurchaseOrderCurrency = 
                Model.Supplier == null ? CurrencyEnum.COP : Model.Supplier.SupplierCurrency;
        }
        public void AddBlankItem()
        {
            if (!Model.PurchaseOrderItems.Any(x => x.BudgetItemId == Guid.Empty))
            {
                Model.PurchaseOrderItems.Add(new PurchaseOrderItemRequest());
            }


        }
        public async Task ChangeCurrency(PurchaseOrderItemRequest item, CurrencyEnum newCurrency)
        {

            double originalValueInUsd = item.UnitaryValueFromQuoteUSD;
            if (newCurrency.Id == CurrencyEnum.COP.Id)
            {
                item.QuoteCurrencyValue = originalValueInUsd * Model.USDCOP;
            }
            else if (newCurrency.Id == CurrencyEnum.EUR.Id)
            {
                item.QuoteCurrencyValue = originalValueInUsd * Model.USDEUR;
            }
            else if (newCurrency.Id == CurrencyEnum.USD.Id)
            {
                item.QuoteCurrencyValue = originalValueInUsd;
            }
            Model.QuoteCurrency = newCurrency;
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
            item.QuoteCurrencyValue = currencyvalue;
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
                var datatoadd = OriginalBudgetItems.Single(x => x.BudgetItemId == order.BudgetItemId);
                BudgetItems.Add(datatoadd);
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
        }
        async Task SaveRow(PurchaseOrderItemRequest order)
        {
            await ordersGrid.UpdateRow(order);
        }

        void CancelEdit(PurchaseOrderItemRequest order)
        {


            ordersGrid.CancelEditRow(order);


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
            ItemToAdd = null!;
        }
        async Task OnKeyDownCurrency(KeyboardEventArgs arg, PurchaseOrderItemRequest order)
        {
            if (arg.Key == "Enter")
            {
                await ordersGrid.UpdateRow(order);
            }
        }
    }
}
