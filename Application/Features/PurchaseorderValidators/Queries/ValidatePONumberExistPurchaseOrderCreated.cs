using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record ValidatePONumberExist(Guid PurchaseOrderId, string ponumber) : IRequest<bool>;
    internal class ValidatePONumberExistHandler : IRequestHandler<ValidatePONumberExist, bool>
    {
        private IPurchaseOrderValidatorRepository _repository;

        public ValidatePONumberExistHandler(IPurchaseOrderValidatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ValidatePONumberExist request, CancellationToken cancellationToken)
        {
            return await _repository.ValidatePONumber(request.PurchaseOrderId, request.ponumber);
        }
    }
}
