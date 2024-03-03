using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Client.Infrastructure.Managers.BudgetItems;
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
        UpdateBudgetItemRequest Model { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        [Inject]
        private IBudgetItemService Service { get; set; }
        [Inject]
        private IBrandService BrandService { get; set; }

        FluentValidationValidator _fluentValidationValidator = null!;
       
        List<BrandResponse> Brands { get; set; } = new();
        bool DisableCreateButton => Model.Type == BudgetItemTypeEnum.None ? true :
     Model.Type == BudgetItemTypeEnum.Taxes && Model.SelectedBudgetItemDtos.Count == 0 ? true :
            Model.ValidationErrors.Count > 0;
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
            else
            {
                Model.ValidationErrors = resultDataForCreateBudget.Messages;
            }
            var resultbrands = await BrandService.GetAllBrand();
            if (resultbrands.Succeeded)
            {
                Brands = resultbrands.Data;
            }
            else
            {
                Model.ValidationErrors = resultbrands.Messages;
            }
        }
        private async Task SaveAsync()
        {
            Model.ValidationErrors.Clear();

            var result = await Service.UpdateBudgetItem(Model);
            if (result.Succeeded)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = result.Message,
                    Duration = 4000
                });

                _NavigationManager.NavigateTo($"/BudgetItemsDataList/{Model.MWOId}");
            }
            else
            {
                Model.ValidationErrors = result.Messages;
            }
        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo($"/BudgetItemsDataList/{Model.MWOId}");
        }
    }
}
