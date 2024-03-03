using Application.Interfaces;
using MediatR;
using Shared.Models.Brands;

namespace Application.Features.Brands.Queries
{
    public record BrandUpdateNameExistQuery(UpdateBrandRequest Data) : IRequest<bool>;
    public class BrandUpdateNameExistQueryHandler : IRequestHandler<BrandUpdateNameExistQuery, bool>
    {
        private readonly IBrandRepository repository;

        public BrandUpdateNameExistQueryHandler(IBrandRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(BrandUpdateNameExistQuery request, CancellationToken cancellationToken)
        {
            var result= await repository.ReviewIfNameExist(request.Data.Id, request.Data.Name);
            return result;
        }
    }

}
