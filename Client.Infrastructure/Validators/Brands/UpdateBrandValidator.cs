namespace Client.Infrastructure.Validators.Brands
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
    {
        private readonly IBrandService Service;

        public UpdateBrandValidator(IBrandService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("MWO Name must be defined!");
            RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage("MWO Name Exist");



        }

        async Task<bool> ReviewIfNameExist(UpdateBrandRequest name, CancellationToken cancellationToken)
        {

            var result = await Service.ReviewIfNameExist(name);
            return !result;
        }
    }
}
