﻿using Application.Features.Brands.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Brands;

namespace Application.Features.Brands.Command
{
    public record UpdateBrandCommand(UpdateBrandRequest Data) : IRequest<IResult>;
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, IResult>
    {
        private IBrandRepository Repository { get; set; }
        private IAppDbContext Context { get; set; }
        public UpdateBrandCommandHandler(IBrandRepository repository, IAppDbContext context)
        {
            Repository = repository;
            Context = context;

        }

        public async Task<IResult> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateBrandValidator(Repository);
            var validationresult = await validator.ValidateAsync(request.Data);
            if (!validationresult.IsValid)
            {
                return Result.Fail(validationresult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var row = await Repository.GetBrandById(request.Data.Id);
            if (row == null) return Result.Fail($"{request.Data.Name} was not found!");

            row.Name = request.Data.Name;
            await Repository.UpdateBrand(row);
            var result = await Context.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} was updated succesfully!");
            }
            return Result.Success($"{request.Data.Name} was not updated succesfully!");
        }
    }

}
