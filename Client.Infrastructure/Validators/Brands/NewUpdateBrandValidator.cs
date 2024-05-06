using Shared.NewModels.Brands.Request;

namespace Client.Infrastructure.Validators.Brands
{
    public class NewUpdateBrandValidator : AbstractValidator<NewBrandUpdateRequest>
    {
        private readonly INewBrandValidatorService Service;

        public NewUpdateBrandValidator(INewBrandValidatorService service)
        {
            Service = service;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand must be defined!")
                .NotNull().WithMessage("Brand must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage(x => $"{x.Name} already exist");



        }

        async Task<bool> ReviewIfNameExist(NewBrandUpdateRequest data, string name, CancellationToken cancellationToken)
        {

            var result = await Service.ReviewIfNameExist(data.BrandId, name.ToLower());
            return !result;
        }
    }
}

