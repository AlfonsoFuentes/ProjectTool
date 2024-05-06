using Shared.NewModels.Brands.Request;

namespace Client.Infrastructure.Validators.Brands
{
    public class NewCreateBrandValidator : AbstractValidator<NewBrandCreateRequest>
    {
        private readonly INewBrandValidatorService Service;

        public NewCreateBrandValidator(INewBrandValidatorService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Brand Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await Service.ReviewIfNameExist(name.ToLower());
            return !result;
        }
    }
}
