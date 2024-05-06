using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Client.Infrastructure.Managers.BudgetItems;
using ClientRadzen.NewPages.Brands;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.NewModels.Brands.Reponses;
#nullable disable
namespace ClientRadzen.Pages.BudgetItems
{
    public partial class UpdateBudgetItemPage
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        UpdateBudgetItemRequest Model { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        [Inject]
        private IBudgetItemService Service { get; set; }
        [Inject]
        private INewBrandService BrandService { get; set; }

        ListBudgetItemResponse Response { get; set; } = new();

        List<NewBrandResponse> Brands { get; set; } = new();
     
        protected override async Task OnInitializedAsync()
        {
            var resultModel = await Service.GetBudgetItemById(Id);
            if (resultModel.Succeeded)
            {
                Model = resultModel.Data;
               
            }
            var resultDataForCreateBudget = await Service.GetDataForCreateBudgetItem(Model.MWOId);
            if (resultDataForCreateBudget.Succeeded)
            {
                Model.SumBudgetItems = resultDataForCreateBudget.Data.SumBudget;
                Model.SumPercentage = resultDataForCreateBudget.Data.SumEngContPercentage;
                Model.BudgetItemDtos = resultDataForCreateBudget.Data.BudgetItems;
            }
            var resultBudgetItems = await Service.GetAllBudgetItemByMWO(Model.MWOId);
            if (resultBudgetItems.Succeeded)
            {
                Response = resultBudgetItems.Data;
               
            }


            var resultbrands = await BrandService.GetAllBrand();
            if (resultbrands.Succeeded)
            {
                Brands = resultbrands.Data.Brands;
            }
           
        }
        private async Task SaveAsync()
        {
           

            var result = await Service.UpdateBudgetItem(Model);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                CancelAsync();
            }
            else
            {

                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }
        }

        private void CancelAsync()
        {
            Navigation.NavigateBack();

        }
        FluentValidationValidator _fluentValidationValidator = null!;
        async Task<bool> ValidateAsync()
        {
            try
            {
                NotValidated = !(await _fluentValidationValidator.ValidateAsync());
                return NotValidated;
            }
            catch (Exception ex)
            {
                string exm = ex.Message;
            }
            return false;

        }
        bool NotValidated = true;

        
        async Task CreateBrand()
        {
            var result = await DialogService.OpenAsync<NewCreateBrandDialog>($"Create New Brand",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "500px", Height = "512px", Resizable = true, Draggable = true });
            if (result != null && result is NewBrandResponse)
            {
                Model.Brand = result as NewBrandResponse;
                var resultData = await BrandService.GetAllBrand();
                if (resultData.Succeeded)
                {
                    Brands = resultData.Data.Brands;

                }


            }
        }
        public async Task ChangeQuantity(string stringquantity)
        {

            double quantity = 0;
            if (!double.TryParse(stringquantity, out quantity))
            { }

            if (Model.IsRegularData || Model.IsEquipmentData || Model.IsAlteration)
            {
                Model.Quantity = quantity;
                Model.Budget = Model.Quantity * Model.UnitaryCost;
            }
            await ValidateAsync();
        }
        public async Task ChangeUnitaryCost(string stringunitaryCost)
        {

            double unitarycost = 0;
            if (!double.TryParse(stringunitaryCost, out unitarycost))
            { }


            if (Model.IsRegularData || Model.IsEquipmentData || Model.IsAlteration)
            {
                Model.UnitaryCost = unitarycost;
                Model.Budget = Model.Quantity * Model.UnitaryCost;
            }
            await ValidateAsync();
        }
        public async Task ChangeTaxesItemList(object objeto)
        {

            Model.SelectedBudgetItemDtos.Clear();
            foreach (var item in Model.SelectedIdBudgetItemDtos)
            {
                Model.SelectedBudgetItemDtos.Add(Model.BudgetItemDtos.Single(x => x.BudgetItemId == item));
            }
            Model.Budget = Math.Round(Model.SumBudgetTaxes * Model.Percentage / 100.0, 2);
            await ValidateAsync();
        }

        public async Task ChangePercentage(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            { }
            if (Model.IsEngContData)
            {
                Model.SumPercentage -= Model.Percentage;
                Model.Percentage = percentage;
                Model.SumPercentage += Model.Percentage;
                Model.Budget = Math.Round(Model.SumBudgetItems * Model.Percentage / (100 - Model.SumPercentage), 2);
            }
            if (Model.IsTaxesData)
            {
                Model.Percentage = percentage;
                Model.Budget = Math.Round(Model.SumBudgetTaxes * Model.Percentage / 100, 2);
            }
            await ValidateAsync();
        }
        public async Task ChangeBudget(string stringunitarycost)
        {

            double Unitarycost = 0;
            if (!double.TryParse(stringunitarycost, out Unitarycost))
            { }
            if (Model.IsEngineeringData)
            {
                Model.Percentage = 0;
                Model.UnitaryCost = Unitarycost;
                Model.Quantity = 1;
                Model.Budget = Model.UnitaryCost * Model.Quantity;
            }
            await ValidateAsync();


        }
       
        public async Task ChangeName(string name)
        {
            Model.Name = name;
            await ValidateAsync();

        }
    }
}
