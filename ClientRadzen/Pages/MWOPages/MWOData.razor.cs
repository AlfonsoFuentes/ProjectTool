using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Shared.Models.MWOStatus;
#nullable disable
namespace ClientRadzen.Pages.MWOPages
{
    public partial class MWOData
    {
        [Parameter]
        public MWOResponse Data { get; set; }
        [CascadingParameter]
        private MWODataList MainPage { get; set; }

        public MWOResponse selectedRow { get; set; }
        bool EditRow = false;
      


        void OnDoubleClick(MWOResponse _selectedRow)
        {
            selectedRow = _selectedRow;
            if (selectedRow.Status.Id == MWOStatusEnum.Created.Id)
            {
                EditRow = true;
            }




        }
        async Task OnKeyDown(KeyboardEventArgs arg, MWOResponse order)
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
        async Task SaveAsync(MWOResponse order)
        {
            if (EditRow)
            {
                await UpdateAsync(order);
            }
            
        }
        async Task CancelAsync()
        {
            EditRow = false;
            await MainPage.UpdateAll();
        }
        

        async Task UpdateAsync(MWOResponse order)
        {
            UpdateMWOMinimalRequest Model = new()
            {
                Id = order.Id,
                Name = order.Name,
                Type = order.MWOType,


            };
            var result = await MainPage.Service.UpdateMWOMinimal(Model);
            if (result.Succeeded)
            {
                NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                EditRow = false;
               
                selectedRow = null!;
                await MainPage.UpdateAll();
                
            }
            else
            {
                NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);

            }
        }
        void Edit(MWOResponse Response)
        {
            OnDoubleClick(Response);

        }
        void EditByForm(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/UpdateMWO/{Response.Id}");

        }
        void AddItemToMWO(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/CreateBudgetItem/{Response.Id}");

        }
        void ShowItemsofMWO(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/BudgetItemsDataList/{Response.Id}");

        }
        async Task Delete(MWOResponse response)
        {

            var resultDialog = await DialogService.Confirm($"Are you sure delete {response.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true });
            if (resultDialog.Value)
            {
                var result = await MainPage.Service.Delete(response);
                if (result.Succeeded)
                {
                    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                    await MainPage.UpdateAll();
                }
                else
                {
                    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }

            }


        }
        async Task Approve(MWOResponse Response)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure Approved {Response.Name}?", "Confirm",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                _NavigationManager.NavigateTo($"/ApproveMWO/{Response.Id}");

            }

        }
    }
}
