using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Brands;

namespace Application.Features.Brands.Command
{
    public record CreateBrandForBudgetItemCommand(CreateBrandRequestDto Data) : IRequest<IResult<BrandResponse>>;
    public class CreateBrandForBudgetItemCommandHandler : IRequestHandler<CreateBrandForBudgetItemCommand, IResult<BrandResponse>>
    {
        private IBrandRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateBrandForBudgetItemCommandHandler(IAppDbContext appDbContext, IBrandRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult<BrandResponse>> Handle(CreateBrandForBudgetItemCommand request, CancellationToken cancellationToken)
        {
           
            var row = Brand.Create(request.Data.Name);
            await Repository.AddBrand(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            BrandResponse response = new()
            {
                Id = row.Id,
                Name = row.Name,
            };
            if (result > 0)
            {
                return Result<BrandResponse>.Success(response, $"{request.Data.Name} created succesfully!");
            }
            return Result<BrandResponse>.Fail($"{request.Data.Name} was not created succesfully!");
        }
    }

}
