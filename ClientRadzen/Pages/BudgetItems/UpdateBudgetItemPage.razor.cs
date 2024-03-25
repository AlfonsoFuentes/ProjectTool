using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Client.Infrastructure.Managers.BudgetItems;
using ClientRadzen.Pages.Brands;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Brands;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
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
        private IBrandService BrandService { get; set; }

        ListBudgetItemResponse Response { get; set; } = new();

        List<BrandResponse> Brands { get; set; } = new();
     
        protected override async Task OnInitializedAsync()
        {
            var resultModel = await Service.GetBudgetItemById(Id);
            if (resultModel.Succeeded)
            {
                Model = resultModel.Data;
                Model.Validator += ValidateAsync;
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
                Brands = resultbrands.Data;
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

        public void Dispose()
        {
            Model.Validator -= ValidateAsync;
        }
        async Task CreateBrand()
        {
            var result = await DialogService.OpenAsync<CreateBrandDialog>($"Create New Brand",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "500px", Height = "512px", Resizable = true, Draggable = true });
            if (result != null && result is BrandResponse)
            {
                Model.Brand = result as BrandResponse;
                var resultData = await BrandService.GetAllBrand();
                if (resultData.Succeeded)
                {
                    Brands = resultData.Data;

                }


            }
        }
    }
}
