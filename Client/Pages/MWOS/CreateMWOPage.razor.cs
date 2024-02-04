using Blazored.FluentValidation;
using Client.Managers.MWOS;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Models.MWO;

namespace Client.Pages.MWOS
{
    public partial class CreateMWOPage 
    {
 
       
         MWOCreateRequest Content { get; set; } = new();

      

        FluentValidationValidator _fluentValidationValidator = null!;
        [Inject]
        private IMWOService Service { get; set; } = null!;
       
        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.CreateMWO(Content);
                if (result.Succeeded)
                {
                    MessageService.ShowMessageBar(options =>
                    {
                        options.Title = result.Message;
                        options.Intent = MessageIntent.Success;
                        options.Section = "MESSAGES_TOP";
                        options.Timeout = 4000;
                    });

                    _NavigationManager.NavigateTo("/mwotable");
                }
            }

        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/mwotable");
        }

    }
}
