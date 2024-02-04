using Client.Managers.MWOS;
using FluentValidation;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;

namespace Client.Validators.MWOs
{
    public class UpdateMWOValidator : AbstractValidator<MWOUpdateRequest>
    {
        private readonly IMWOService mwoservice;
        private Guid Id;
        public UpdateMWOValidator(IMWOService mwoservice)
        {
            this.mwoservice = mwoservice;
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("MWO Name must be defined!");
            RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage("MWO Name Exist");

            
           
        }

        async Task<bool> ReviewIfNameExist(MWOUpdateRequest name, CancellationToken cancellationToken)
        {
          
            var result = await mwoservice.ReviewIfNameExist(name);
            return !result;
        }
    }
}
