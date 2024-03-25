namespace Application.Features.Brands.Validators
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
    {
        private readonly IBrandValidatorService Service;

        public UpdateBrandValidator(IBrandValidatorService service)
        {
            Service = service;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand must be defined!")
                .NotNull().WithMessage("Brand must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage(x => $"{x.Name} already exist");



        }

        async Task<bool> ReviewIfNameExist(UpdateBrandRequest data, string name, CancellationToken cancellationToken)
        {

            var result = await Service.ReviewIfNameExist(data.Id, name.ToLower());
            return !result;
        }
    }
}
