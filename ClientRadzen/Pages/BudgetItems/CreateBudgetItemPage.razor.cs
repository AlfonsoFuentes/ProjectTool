﻿#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Client.Infrastructure.Managers.BudgetItems;
using ClientRadzen.Pages.Brands;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Brands;
using Shared.Models.BudgetItems;

namespace ClientRadzen.Pages.BudgetItems
{
    public partial class CreateBudgetItemPage
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        CreateBudgetItemRequest Model { get; set; } = new();

        [Parameter]
        public Guid MWOId { get; set; }


        [Inject]
        private IBudgetItemService Service { get; set; } = null!;
        [Inject]
        private IBrandService BrandService { get; set; }

        ListBudgetItemResponse Response { get; set; } = new();

        List<BrandResponse> Brands { get; set; } = new();

      

        protected override async Task OnInitializedAsync()
        {
            var resultBudgetItems = await Service.GetAllBudgetItemByMWO(MWOId);
            if (resultBudgetItems.Succeeded)
            {
                Response = resultBudgetItems.Data;
                Model.MWO = Response.MWO;
                Model.Validator += ValidateAsync;
            }
            
            var resultbrands = await BrandService.GetAllBrand();
            if (resultbrands.Succeeded)
            {
                Brands = resultbrands.Data;
            }
           
        }
        private async Task SaveAsync()
        {
          

            var result = await Service.CreateBudgetItem(Model);
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

       
        async Task CreateBrand()
        {
            var result = await DialogService.OpenAsync<CreateBrandDialog>($"Create New Brand",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "500px", Height = "312px", Resizable = true, Draggable = true });
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
    }

}


