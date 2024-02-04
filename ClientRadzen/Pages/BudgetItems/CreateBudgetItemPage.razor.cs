#nullable disable
using Blazored.FluentValidation;
using ClientRadzen.Managers.Brands;
using ClientRadzen.Managers.BudgetItems;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Brands;
using Shared.Models.BudgetItems;

namespace ClientRadzen.Pages.BudgetItems
{
    public partial class CreateBudgetItemPage
    {
        CreateBudgetItemRequest Model { get; set; } = new();

        [Parameter]
        public Guid MWOId { get; set; }

        FluentValidationValidator _fluentValidationValidator = null!;
        [Inject]
        private IBudgetItemService Service { get; set; } = null!;
        [Inject]
        private IBrandService BrandService { get; set; }
        List<BrandResponse> Brands { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var resultSumBudget =await Service.GetSumBudget(MWOId);
            var resultSumPercent = await Service.GetSumEngConPercentage(MWOId);
            Model.SumBudgetItems = resultSumBudget.Data;
            Model.SumPercentage = resultSumPercent.Data;
            Model.MWOId = MWOId;
            var resultbrands = await BrandService.GetAllBrand();
            if(resultbrands.Succeeded)
            {
                Brands=resultbrands.Data;
            }

        }
        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.CreateBudgetItem(Model);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    _NavigationManager.NavigateTo($"/BudgetItemtable/{MWOId}");
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Error Summary",
                        Detail = result.Message,
                        Duration = 4000
                    });
                }
            }

        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/BudgetItemstable");
        }

    }

}


