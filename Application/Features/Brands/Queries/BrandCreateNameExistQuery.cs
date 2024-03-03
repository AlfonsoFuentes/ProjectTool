using Application.Features.MWOs.Queries;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Brands.Queries
{
    public record BrandCreateNameExistQuery(string Name) : IRequest<bool>;
    public class BrandNameExistQueryHandler : IRequestHandler<BrandCreateNameExistQuery, bool>
    {
        private readonly IBrandRepository repository;

        public BrandNameExistQueryHandler(IBrandRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(BrandCreateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNameExist(request.Name);
        }
    }

}
