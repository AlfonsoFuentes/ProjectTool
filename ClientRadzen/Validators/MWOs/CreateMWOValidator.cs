

using Shared.Models.MWOTypes;

namespace ClientRadzen.Validators.MWOs
{
    public class CreateMWOValidator : AbstractValidator<CreateMWORequest>
    {
        private readonly IMWOService mwoservice;

        public CreateMWOValidator(IMWOService mwoservice)
        {
            this.mwoservice = mwoservice;
            RuleFor(x => x.Name).NotEmpty().WithMessage("MWO Name must be defined!");
            RuleFor(x => x.Type).NotEqual(MWOTypeEnum.None).WithMessage("Type must be defined");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage($"Name Already exist");

        }

        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await mwoservice.ReviewIfNameExist(name);
            return !result;
        }
    }
}
