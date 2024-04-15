using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record ValidateNewPONumberExist(string ponumber) : IRequest<bool>;
    internal class ValidateNewPONumberExistHandler : IRequestHandler<ValidateNewPONumberExist, bool>
    {
        private IPurchaseOrderValidatorRepository _repository;

        public ValidateNewPONumberExistHandler(IPurchaseOrderValidatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ValidateNewPONumberExist request, CancellationToken cancellationToken)
        {
            return await _repository.ValidatePONumber(request.ponumber);
        }
    }
}
