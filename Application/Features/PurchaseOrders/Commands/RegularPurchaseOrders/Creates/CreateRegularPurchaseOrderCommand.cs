using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Creates
{
    public record CreateRegularPurchaseOrderCommand(CreatedRegularPurchaseOrderRequest Data) : IRequest<IResult>;

    internal class CreateRegularPurchaseOrderCommandHandler : IRequestHandler<CreateRegularPurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public CreateRegularPurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(CreateRegularPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
           
            var mwo = await Repository.GetMWOWithBudgetItemsAndPurchaseOrderById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail($"MWO Not found");

            }
            var purchaseorder = mwo.AddPurchaseOrder();
            purchaseorder.PurchaseorderName = request.Data.PurchaseorderName;
            purchaseorder.PurchaseRequisition = request.Data.PurchaseRequisition;
            purchaseorder.QuoteCurrency = request.Data.QuoteCurrency.Id;
            purchaseorder.IsAlteration = request.Data.IsAlteration;
            
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.SPL = request.Data.SPL;
            purchaseorder.SupplierId = request.Data.SupplierId;
            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueCurrency = request.Data.SumPOValueCurrency;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Created.Id;
            purchaseorder.QuoteNo = request.Data.QuoteNo;
            purchaseorder.TaxCode = request.Data.TaxCode;
           
            purchaseorder.AccountAssigment = request.Data.AccountAssigment;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency.Id;
           
            await Repository.AddPurchaseOrder(purchaseorder);
            foreach (var item in request.Data.PurchaseOrderItemNoBlank)
            {
                var purchaseorderItem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId, item.Name);
                purchaseorderItem.UnitaryValueCurrency = item.CurrencyUnitaryValue;
                purchaseorderItem.Quantity = item.Quantity;
            
                await Repository.AddPurchaseorderItem(purchaseorderItem);
            }

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            //await MWORepository.UpdateDataForApprovedMWO(purchaseorder.MWOId, cancellationToken);
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
