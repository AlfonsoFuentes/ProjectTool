#nullable disable
using Client.Managers.MWOS;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;

namespace Client.Pages.MWOS
{
    public partial class MWOTable
    {
        [Inject]
        private IMWOService Service { get; set; }
        List<MWOResponse> OriginalData { get; set; } = new();
        private bool _trapFocus = true;
        private bool _modal = true;
        protected override async Task OnInitializedAsync()
        {
            var user = CurrentUser.UserId;

            await UpdateAll();
        }
        async Task UpdateAll()
        {
            var result = await Service.GetAllMWO();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
        }
        private void AddNewMWO()
        {
            _NavigationManager.NavigateTo("/CreateMWO");
        }
      
        async Task Edit(MWOResponse mWOResponse)
        {
            MWOUpdateRequest model = new MWOUpdateRequest()
            {
                Id = mWOResponse.Id,
                Name = mWOResponse.Name,
                Type = MWOTypeEnum.GetType(mWOResponse.Type),
            };
            DialogParameters parameters = new()
            {
                Title = $"Edit MWO: {mWOResponse.Name}",
                PrimaryAction = "Yes",
                PrimaryActionEnabled = false,
                SecondaryAction = "No",
                Width = "500px",
                TrapFocus = _trapFocus,
                Modal = _modal,
                PreventScroll = true
            };

            IDialogReference dialog = await DialogService.ShowDialogAsync<UpdateMWOPage>(model, parameters);
            DialogResult result = await dialog.Result;


            if (result.Data is not null)
            {
                //SimplePerson? simplePerson = result.Data as result;
                await UpdateAll();
            }
            else
            {
               
            }
        }
        async Task Delete(MWOResponse mWOResponse)
        {

        }
    }
}
