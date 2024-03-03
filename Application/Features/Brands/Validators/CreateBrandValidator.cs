using Application.Interfaces;
using FluentValidation;
using Shared.Models.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Validators
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandRequest>
    {
        private readonly IBrandRepository Repository;

        public CreateBrandValidator(IBrandRepository service)
        {
            Repository = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Brand Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage(x=>$"{x.Name} Already exist");

        }

        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await Repository.ReviewIfNameExist(name);
            return !result;
        }
    }
}
