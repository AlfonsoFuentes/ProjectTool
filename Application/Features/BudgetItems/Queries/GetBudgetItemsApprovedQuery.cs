using Shared.Enums.BudgetItemTypes;

namespace Application.Features.BudgetItems.Queries
{
    public record GetBudgetItemsApprovedQuery(Guid MWOId) : IRequest<IQueryable<BudgetItemApprovedExportFileResponse>>;
    internal class GetBudgetItemsApprovedQueryHandler
        : IRequestHandler<GetBudgetItemsApprovedQuery, IQueryable<BudgetItemApprovedExportFileResponse>>

    {
        private readonly IBudgetItemRepository Repository;

        public GetBudgetItemsApprovedQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        }

        public async Task<IQueryable<BudgetItemApprovedExportFileResponse>> Handle(GetBudgetItemsApprovedQuery request, CancellationToken cancellationToken)
        {
            var rows = await Repository.GetBudgetItemWithMWOPurchaseOrderList(request.MWOId);

            var result = rows.Select(e => new BudgetItemApprovedExportFileResponse()
            {

                Brand = e.Brand == null ? string.Empty : e.Brand.Name,
                Model = e.Model!,
                Name = e.Name,
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(e.Type)}{e.Order}",
                Type = BudgetItemTypeEnum.GetName(e.Type),
                MWOName = e.MWO.Name,
                AssignedUSD = e.AssignedUSD,
                ActualUSD = e.ActualUSD,
                ApprovedUSD = e.ApprovedUSD,
                BudgetUSD = e.Budget,
                PotentialCommitmentUSD = e.PotentialCommitmentUSD,
            });



            return result;
        }
    }

}
