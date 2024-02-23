using Shared.Models.MWOTypes;

namespace Client.Infrastructure.Validators.MWOs
{
    public class UpdateMWOValidator : AbstractValidator<UpdateMWORequest>
    {
        private readonly IMWOService mwoservice;

        public UpdateMWOValidator(IMWOService mwoservice)
        {
            this.mwoservice = mwoservice;
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("MWO Name must be defined!");
            RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage("MWO Name Exist");
            RuleFor(x => x.Type).NotEqual(MWOTypeEnum.None).WithMessage("Type must be defined");
            RuleFor(x => x.PercentageAssetNoProductive).GreaterThan(0).When(x => x.IsAssetProductive == false).WithMessage("Taxes No Productive must be defined!");


        }

        async Task<bool> ReviewIfNameExist(UpdateMWORequest name, CancellationToken cancellationToken)
        {

            var result = await mwoservice.ReviewIfNameExist(name);
            return !result;
        }
    }
}
