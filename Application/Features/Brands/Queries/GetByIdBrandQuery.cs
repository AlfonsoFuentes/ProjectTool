using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Brands;

namespace Application.Features.Brands.Queries
{
    public record GetByIdBrandQuery(Guid Id) : IRequest<IResult<BrandResponse>>;

    public class GetByIdBrandQueryHandler : IRequestHandler<GetByIdBrandQuery, IResult<BrandResponse>>
    {
        private IBrandRepository Repository { get; set; }

        public GetByIdBrandQueryHandler(IBrandRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<BrandResponse>> Handle(GetByIdBrandQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetBrandById(request.Id);

            if (row == null)
            {

                return Result<BrandResponse>.Fail("Not found!");
            }
            BrandResponse response = new()
            {
                Id = row.Id,
                Name = row.Name,
            };
            return Result<BrandResponse>.Success(response);
        }
    }

}
