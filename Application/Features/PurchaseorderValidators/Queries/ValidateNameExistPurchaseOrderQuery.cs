using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record ValidateNameExistPurchaseOrderQuery(Guid MWOId,string name) : IRequest<bool>;
    internal class ValidateNameExistPurchaseOrderQueryHandler : IRequestHandler<ValidateNameExistPurchaseOrderQuery, bool>
    {
        private IPurchaseOrderValidatorRepository _repository;

        public ValidateNameExistPurchaseOrderQueryHandler(IPurchaseOrderValidatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ValidateNameExistPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ValidateNameExist(request.MWOId,request.name);
        }
    }

}
