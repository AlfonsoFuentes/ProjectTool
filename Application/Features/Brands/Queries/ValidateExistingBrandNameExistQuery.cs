using Application.Interfaces;
using MediatR;

namespace Application.Features.Brands.Queries
{
    public record ValidateExistingBrandNameExistQuery(Guid BrandId,string Name) : IRequest<bool>;
    public class ValidateExistingBrandNameExistQueryHandler : IRequestHandler<ValidateExistingBrandNameExistQuery, bool>
    {
        private readonly IBrandRepository repository;

        public ValidateExistingBrandNameExistQueryHandler(IBrandRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateExistingBrandNameExistQuery request, CancellationToken cancellationToken)
        {
            var result= await repository.ReviewIfNameExist(request.BrandId, request.Name);
            return result;
        }
    }

}
