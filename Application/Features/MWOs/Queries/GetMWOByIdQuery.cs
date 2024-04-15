using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOByIdQuery(Guid Id) : IRequest<IResult<MWOResponse>>;
    public class GetMWOByIdQueryHandler : IRequestHandler<GetMWOByIdQuery, IResult<MWOResponse>>
    {
        private IMWORepository Repository { get; set; }

        public GetMWOByIdQueryHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IResult<MWOResponse>> Handle(GetMWOByIdQuery request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetMWOWithItemsById(request.Id);
            if (mwo == null) return Result<MWOResponse>.Fail("Not Found");

            MWOResponse retorno = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageEngineering,
                MWOType = MWOTypeEnum.GetType(mwo.Type),
                Capital = mwo.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                Expenses = mwo.BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                PotencialCapital = mwo.PurchaseOrders.Where(x => x.IsAlteration == false && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                PotencialExpenses = mwo.PurchaseOrders.Where(x => x.IsAlteration == true && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                ActualCapital = mwo.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.ActualUSD),
                ActualExpenses = mwo.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.ActualUSD),
                POValueCapital = mwo.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.POValueUSD),
                POValueExpenses = mwo.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.POValueUSD),
                Status = MWOStatusEnum.GetType(mwo.Status),

            };

            return Result<MWOResponse>.Success(retorno);

        }
    }
}
