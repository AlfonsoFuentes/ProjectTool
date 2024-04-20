using Application.Interfaces;
using MediatR;

namespace Application.Features.Brands.Queries
{
    public record ValidateBrandNameExistQuery(string Name) : IRequest<bool>;
    public class ValidateBrandNameExistQueryHandler : IRequestHandler<ValidateBrandNameExistQuery, bool>
    {
        private readonly IBrandRepository repository;

        public ValidateBrandNameExistQueryHandler(IBrandRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateBrandNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNameExist(request.Name);
        }
    }

}
