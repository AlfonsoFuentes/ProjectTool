using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record NewPurchaseOrderValidatePRNumberQuery(string purchaserequisition) : IRequest<bool>;
    internal class NewPurchaseOrderValidatePRNumberQueryHandler : IRequestHandler<NewPurchaseOrderValidatePRNumberQuery, bool>
    {
       
        private IQueryValidationsRepository QueryRepository;
        public NewPurchaseOrderValidatePRNumberQueryHandler(IQueryValidationsRepository repository)
        {
            QueryRepository = repository;
        }

        public async Task<bool> Handle(NewPurchaseOrderValidatePRNumberQuery request, CancellationToken cancellationToken)
        {
            return await QueryRepository.ValidatePurchaseRequisitionExist(request.purchaserequisition);
        }
    }

}
