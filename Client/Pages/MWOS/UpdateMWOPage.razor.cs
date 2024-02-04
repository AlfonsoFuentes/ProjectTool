using Blazored.FluentValidation;
using Client.Managers.MWOS;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Models.MWO;

namespace Client.Pages.MWOS
{
    public partial class UpdateMWOPage : IDialogContentComponent<MWOUpdateRequest>
    {
        MWOCreateRequest Model = new();
        [Parameter]
        public MWOUpdateRequest Content { get; set; } = default!;

        [CascadingParameter]
        public FluentDialog? Dialog { get; set; }

        FluentValidationValidator _fluentValidationValidator = null!;
        [Inject]
        private IMWOService Service { get; set; } = null!;
        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.UpdateMWO(Content);
                if (result.Succeeded)
                {
                    MessageService.ShowMessageBar(options =>
                    {
                        options.Title = result.Message;
                        options.Intent = MessageIntent.Success;
                        options.Section = "MESSAGES_TOP";
                        options.Timeout = 4000;
                    });
                    await Dialog.CloseAsync(Content);

                }
            }

        }

        private async Task CancelAsync()
        {
            await Dialog.CancelAsync();
        }

    }
}
