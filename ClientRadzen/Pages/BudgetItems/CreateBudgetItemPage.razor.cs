#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Client.Infrastructure.Managers.BudgetItems;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Commons.Results;
using Shared.Models.Brands;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;

namespace ClientRadzen.Pages.BudgetItems
{
    public partial class CreateBudgetItemPage
    {
        CreateBudgetItemRequest Model { get; set; } = new();

        [Parameter]
        public Guid MWOId { get; set; }

       
        [Inject]
        private IBudgetItemService Service { get; set; } = null!;
        [Inject]
        private IBrandService BrandService { get; set; }
        [Inject]
        private IMWOService MWOService { get; set; }    

        UpdateMWORequest CurrentMWO { get; set; }
        List<BrandResponse> Brands { get; set; } = new();
        List<BudgetItemDto> BudgetItemDtos { get; set; } = new();
        bool DisableCreateButton => Model.Type == BudgetItemTypeEnum.None ? true :
      Model.Type == BudgetItemTypeEnum.Taxes && Model.BudgetItemDtos.Count == 0 ? true : Model.ValidationErrors.Count > 0;
  
        protected override async Task OnInitializedAsync()
        {
            var resultDataForCreateBudget = await Service.GetDataForCreateBudgetItem(MWOId);
            if (resultDataForCreateBudget.Succeeded)
            {
                Model.SumBudgetItems = resultDataForCreateBudget.Data.SumBudget;
                Model.SumPercentage = resultDataForCreateBudget.Data.SumEngContPercentage;
                BudgetItemDtos = resultDataForCreateBudget.Data.BudgetItems;
            }
            else
            {
                Model.ValidationErrors = resultDataForCreateBudget.Messages;
            }
            var resultMWO=await MWOService.GetMWOById(MWOId);
            if (resultMWO.Succeeded)
            {
                CurrentMWO = resultMWO.Data;
                Model.MWOName = CurrentMWO.Name;
                Model.MWOId = MWOId;
            }
            else
            {
                Model.ValidationErrors = resultMWO.Messages;
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

            var result = await Service.CreateBudgetItem(Model);
            if (result.Succeeded)
            {
                NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                _NavigationManager.NavigateTo($"/BudgetItemsDataList/{MWOId}");
            }
            else
            {
                Model.ValidationErrors = result.Messages;
                NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }
        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo($"/BudgetItemsDataList/{MWOId}");
        }

    }

}


