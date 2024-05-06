using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record NewPurchaseOrderValidateNameQuery(Guid MWOId,string name) : IRequest<bool>;
    internal class NewPurchaseOrderValidateNameQueryHandler : IRequestHandler<NewPurchaseOrderValidateNameQuery, bool>
    {
     
        private IQueryValidationsRepository QueryRepository;
        public NewPurchaseOrderValidateNameQueryHandler(IQueryValidationsRepository queryRepository)
        {
           
            QueryRepository = queryRepository;
        }

        public async Task<bool> Handle(NewPurchaseOrderValidateNameQuery request, CancellationToken cancellationToken)
        {
            return await QueryRepository.ValidatePurchaseOrderNameExist(request.MWOId,request.name);
        }
    }

}
