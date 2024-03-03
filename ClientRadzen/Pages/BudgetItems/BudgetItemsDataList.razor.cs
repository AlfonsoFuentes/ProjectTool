using Client.Infrastructure.Managers.BudgetItems;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.PurchaseOrders.Responses;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Models.Suppliers;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;
#nullable disable
namespace ClientRadzen.Pages.BudgetItems
{
    public partial class BudgetItemsDataList
    {
        [Inject]
        private IBudgetItemService Service { get; set; }


        ListBudgetItemResponse Response { get; set; } = new();
        string nameFilter = string.Empty;

        [Parameter]
        public Guid MWOId { get; set; }
        Func<BudgetItemResponse, bool> fiterexpresion => x =>
        x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) || 
        x.Nomenclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)||
        x.Brand.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
        IQueryable<BudgetItemResponse> FilteredItems => Response.BudgetItems?.Where(fiterexpresion).AsQueryable();
        protected override async Task OnInitializedAsync()
        {


            var user = CurrentUser.UserId;

            await UpdateAll();
        }

        async Task UpdateAll()
        {
            var result = await Service.GetAllBudgetItemByMWO(MWOId);

            if (result.Succeeded)
            {
                Response = result.Data;


            }
        }
        private void AddNewBudgetItem()
        {
            _NavigationManager.NavigateTo($"/CreateBudgetItem/{MWOId}");
        }

        void Edit(BudgetItemResponse Response)
        {
            OnDoubleClick(Response);

        }
        void GotoMWOTable()
        {
            _NavigationManager.NavigateTo($"/mwotable");
        }
        void GoToCreatePurchaseOrder(BudgetItemResponse BudgetItemResponse)
        {
            if (BudgetItemResponse.Type.Id == BudgetItemTypeEnum.Taxes.Id)
            {
                _NavigationManager.NavigateTo($"/CreateTaxPurchaseOrder/{BudgetItemResponse.Id}");
            }
            else if (BudgetItemResponse.Type.Id == BudgetItemTypeEnum.Engineering.Id)
            {
                _NavigationManager.NavigateTo($"/CreateCapitalizedSalary/{BudgetItemResponse.Id}");
            }
            else
            {
                _NavigationManager.NavigateTo($"/CreatePurchaseOrder/{BudgetItemResponse.Id}");
            }

        }

        async Task Delete(BudgetItemResponse BudgetItemResponse)
        {
            if (BudgetItemResponse.IsNotAbleToEditDelete)
            {
                await DialogService.Alert($"Can not remove {BudgetItemResponse.Name}", "Project Tool", new AlertOptions() { OkButtonText = "Yes" });

                return;
            }
            var resultDialog = await DialogService.Confirm($"Are you sure delete {BudgetItemResponse.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(BudgetItemResponse);
                if (result.Succeeded)
                {
                    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                    await UpdateAll();
                    EditRow = false;
                    selectedRow = null!;
                }
                else
                {
                    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }

            }

        }

        BudgetItemResponse selectedRow = null!;

        private void GoToHome()
        {
            _NavigationManager.NavigateTo("/");
        }
        private void GoToMWOPage()
        {
            _NavigationManager.NavigateTo("/MWODataList");
        }
        bool EditRow = false;

        void OnDoubleClick(BudgetItemResponse _selectedRow)
        {
            EditRow = true;
            selectedRow = _selectedRow;


        }
        async Task OnKeyDown(KeyboardEventArgs arg, BudgetItemResponse order)
        {
            if (arg.Key == "Enter")
            {
                await SaveAsync(order);


            }
            else if (arg.Key == "Escape")
            {
                EditRow = false;
            }
        }
        async Task SaveAsync(BudgetItemResponse order)
        {
            if (EditRow)
            {
                await UpdateAsync(order);

            }
        }
        async Task UpdateAsync(BudgetItemResponse order)
        {
            UpdateBudgetItemMinimalRequest Model = new()
            {
                Id = order.Id,
                Name = order.Name,
                UnitaryCost = order.UnitaryCost,
                MWOId = order.MWOId,
                MWOName = order.MWOName,
                Quantity = order.Quantity,
                Type = order.Type,
                Percentage = order.Percentage,
                Budget = order.Budget,


            };
            var result = await Service.UpdateBudgetItemMinimal(Model);
            if (result.Succeeded)
            {
                NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                await UpdateAll();
                EditRow = false;
                selectedRow = null!;
            }
            else
            {
                NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);

            }
        }
        async Task CancelAsync()
        {
            await UpdateAll();
        }
        void EditByForm(BudgetItemResponse Response)
        {
            _NavigationManager.NavigateTo($"/UpdateBudgetItem/{Response.Id}");

        }
    }
}
