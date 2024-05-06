#nullable disable




using Shared.Enums.Currencies;
using Shared.Enums.WayToReceivePurchaseOrdersEnums;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class ReceivePurchaseOrder
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Parameter]
        public Guid PurchaseOrderId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }


        [Inject]
        private INewSupplierService SupplierService { get; set; }


        ReceiveRegularPurchaseOrderRequest Model { get; set; } = new();
        List<BudgetItemApprovedResponse> OriginalBudgetItems { get; set; } = new();
        List<BudgetItemApprovedResponse> BudgetItems => GetBudgetItems();
        List<BudgetItemApprovedResponse> GetBudgetItems()
        {
            List<BudgetItemApprovedResponse> response = new();
            foreach (var row in OriginalBudgetItems)
            {
                if (!Model.PurchaseOrderItemsToReceive.Any(x => x.BudgetItemId == row.BudgetItemId))
                {
                    response.Add(row);
                }
            }

            return response;
        }



        FluentValidationValidator _fluentValidationValidator = null!;

       

        
        protected override async Task OnInitializedAsync()
        {
        
            var result = await Service.GetPurchaseOrderToReceiveById(PurchaseOrderId);
            if (result.Succeeded)
            {
                Model = result.Data;
               
               
            }
            Model.TRMUSDCOP = MainApp.RateList == null ? Model.TRMUSDCOP : Math.Round(MainApp.RateList.COP, 2);
            Model.TRMUSDEUR = MainApp.RateList == null ? Model.TRMUSDEUR : Math.Round(MainApp.RateList.EUR, 2);
            var resultSupplier = await SupplierService.GetAllSupplier();
            if (resultSupplier.Succeeded)
            {
                Suppliers = resultSupplier.Data.Suppliers;

            }
            var resultBudgetItems = await Service.GetBudgetItemsToCreatePurchaseOrder(Model.MainBudgetItemId);
            if (resultBudgetItems.Succeeded)
            {

                OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;

            }
            await ValidateAsync();
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
            var result = await Service.ReceivePurchaseOrder(Model);
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
        List<NewSupplierResponse> Suppliers { get; set; } = new();
        async Task CreateSupplier()
        {
            var result = await DialogService.OpenAsync<NewCreateSupplierDialog>($"Create Supplier",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "400px", Height = "450px", Resizable = true, Draggable = true });
            if (result != null)
            {
                await SetSupplier(result as NewSupplierResponse);
                var resultData = await SupplierService.GetAllSupplier();
                if (resultData.Succeeded)
                {
                    Suppliers = resultData.Data.Suppliers;

                }


            }
        }
        bool UpdateCurrentTRM = false;
        public void ClickUpdateCurrentTRM()
        {
            Model.OldCurrencyDate = Model.CurrencyDate;
            Model.OldTRMUSDCOP = Model.TRMUSDCOP;
            Model.OldTRMUSDEUR = Model.TRMUSDEUR;

            Model.CurrencyDate = DateTime.UtcNow;
            Model.TRMUSDCOP = MainApp.RateList.COP;
            Model.TRMUSDEUR = MainApp.RateList.EUR;
            UpdateCurrentTRM = true;
        }
        public void ClickUpdateOldTRM()
        {


            Model.CurrencyDate = Model.OldCurrencyDate;
            Model.TRMUSDCOP = Model.OldTRMUSDCOP;
            Model.TRMUSDEUR = Model.OldTRMUSDEUR;
            UpdateCurrentTRM = false;
        }
        public async Task OnChangeWayToReceivePurchaseOrder(WayToReceivePurchaseorderEnum wayToReceivePurchaseOrder)
        {



            if (Model.WayToReceivePurchaseOrder.Id != wayToReceivePurchaseOrder.Id)
            {
                Model.WayToReceivePurchaseOrder = wayToReceivePurchaseOrder;
                ClearValues();
            }
            if (Model.WayToReceivePurchaseOrder.Id == WayToReceivePurchaseorderEnum.CompleteOrder.Id)
            {
                Model.PercentageToReceive = Math.Round(Model.SumPOPendingCurrency / Model.SumPOValueCurrency * 100, 2);
                foreach (var row in Model.PurchaseOrderItemsToReceive)
                {
                    row.ReceivingCurrency = row.POPendingCurrency;


                    row.ReceivePercentagePurchaseOrder = Math.Round(row.ReceivingCurrency / row.POValueCurrency * 100, 2);
                }
            }
            await ValidateAsync();

        }
        void ClearValues()
        {
            foreach (var row in Model.PurchaseOrderItemsToReceive)
            {
                row.ReceivingCurrency = 0;

                row.ReceivePercentagePurchaseOrder = 0;
            }
        }
        public async Task SetSupplier(NewSupplierResponse? _Supplier)
        {

            if (_Supplier == null)
            {
                Model.PurchaseOrderCurrency = CurrencyEnum.COP;
                return;
            }
            Model.Supplier = _Supplier;


            await ValidateAsync();
            foreach (var row in Model.PurchaseOrderItemsToReceive)
            {
                row.PurchaseOrderCurrency = _Supplier.SupplierCurrency;

            }

            Model.PurchaseOrderCurrency = _Supplier.SupplierCurrency;
        }
        public async Task ChangeTRMUSDEUR(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdeur = Model.OldTRMUSDEUR;
            if (!double.TryParse(arg, out usdeur))
            {

            }
            Model.TRMUSDEUR = usdeur;
            await ValidateAsync();
        }
        public async Task OnChangeReceivePercentagePurchaseOrder(string percentage)
        {

            double newpercentage = Model.PercentageToReceive;
            if (!double.TryParse(percentage, out newpercentage)) { };

            if (!(newpercentage < 0 || newpercentage > 100))
            {
                Model.PercentageToReceive = newpercentage;
                if (newpercentage > Model.MaxPercentageToReceive)
                {
                    Model.PercentageToReceive = Model.MaxPercentageToReceive;
                }

                foreach (var row in Model.PurchaseOrderItemsToReceive)
                {
                    row.ReceivePercentagePurchaseOrder = Model.PercentageToReceive;
                }
            }


            await ValidateAsync();
        }
        public async Task OnChangeReceivingItem(ReceivePurchaseorderItemRequest item, string receivingvalue)
        {
            double newreceivingitem = item.ReceivingCurrency;
            if (!double.TryParse(receivingvalue, out newreceivingitem)) { };
            item.ReceivingCurrency = newreceivingitem;

            await ValidateAsync();
        }
        public async Task OnChangePercentageReceivingItem(ReceivePurchaseorderItemRequest item, string receivingpercentage)
        {
            double newpercentage = item.ReceivePercentagePurchaseOrder;
            if (!double.TryParse(receivingpercentage, out newpercentage)) { };
            if (!(newpercentage < 0 || newpercentage > 100))
            {

                if (newpercentage > item.MaxPercentageToReceive)
                {
                    newpercentage = item.MaxPercentageToReceive;
                }
                item.ReceivePercentagePurchaseOrder = newpercentage;

            }


            await ValidateAsync();
        }
        public async Task ChangeTRMUSDCOP(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdcop = Model.OldTRMUSDCOP;
            if (!double.TryParse(arg, out usdcop))
            {

            }
            Model.TRMUSDCOP = usdcop;
            await ValidateAsync();
        }

    }
}
