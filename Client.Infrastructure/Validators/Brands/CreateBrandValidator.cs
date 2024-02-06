

namespace Client.Infrastructure.Validators.Brands
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandRequest>
    {
        private readonly IBrandService Service;

        public CreateBrandValidator(IBrandService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("MWO Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage($"Name Already exist");

        }

        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await Service.ReviewIfNameExist(name);
            return !result;
        }
    }
}
