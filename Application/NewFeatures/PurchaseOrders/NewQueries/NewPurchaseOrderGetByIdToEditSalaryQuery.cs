namespace Application.NewFeatures.PurchaseOrders.NewQueries
{
    public record NewPurchaseOrderGetByIdToEditSalaryQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderEditSalaryRequest>>;
    internal class NewPurchaseOrderGetByIdToEditSalaryQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToEditSalaryQuery, IResult<NewPurchaseOrderEditSalaryRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditSalaryQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderEditSalaryRequest>> Handle(NewPurchaseOrderGetByIdToEditSalaryQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdToReceiveAsync(request.PurchaseOrderId);

            if (row == null)
            {
                return Result<NewPurchaseOrderEditSalaryRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            var MainBudgetItem = await Repository.GetBudgetItemMWOApprovedAsync(row.MainBudgetItemId);
            if (MainBudgetItem == null)
                return Result<NewPurchaseOrderEditSalaryRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));

            var result = row.ToPurchaseOrderEditSalaryRequest(MainBudgetItem);
            foreach (var item in result.PurchaseOrder.PurchaseOrderItems)
            {
                var budgetitem = await Repository.GetBudgetItemMWOApprovedAsync(item.BudgetItemId);
                if (budgetitem != null)
                {
                    item.BudgetItem=budgetitem.ToBudgetItemMWOApproved();
              

                }
            }

            return Result<NewPurchaseOrderEditSalaryRequest>.Success(result!);
        }
    }
}
