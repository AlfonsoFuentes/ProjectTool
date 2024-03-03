using Application.Interfaces;
using FluentValidation;
using Shared.Models.Brands;

namespace Application.Features.Brands.Validators
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
    {
        private readonly IBrandRepository Repository;

        public UpdateBrandValidator(IBrandRepository service)
        {
            Repository = service;
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("MWO Name must be defined!");
            RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage(x => $"{x.Name} Already exist");



        }

        async Task<bool> ReviewIfNameExist(UpdateBrandRequest data, CancellationToken cancellationToken)
        {

            var result = await Repository.ReviewIfNameExist(data.Id, data.Name);
            return !result;
        }
    }
}
