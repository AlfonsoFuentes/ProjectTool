using Application.Features.PurchaseOrders.Validators;
using Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Creates
{
    public record CreateRegularPurchaseOrderCommand(CreatedRegularPurchaseOrderRequestDto Data) : IRequest<IResult>;

    internal class CreateRegularPurchaseOrderCommandHandler : IRequestHandler<CreateRegularPurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateRegularPurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
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
            purchaseorder.QuoteCurrency = request.Data.QuoteCurrency;
            purchaseorder.IsAlteration = request.Data.IsAlteration;
            
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.SPL = request.Data.SPL;
            purchaseorder.SupplierId = request.Data.SupplierId;
            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueUSD = request.Data.PurchaseOrderItems.Sum(x => x.POValueUSD);
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Created.Id;
            purchaseorder.QuoteNo = request.Data.QuoteNo;
            purchaseorder.TaxCode = request.Data.TaxCode;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.AccountAssigment = request.Data.AccountAssigment;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency;
            await Repository.AddPurchaseOrder(purchaseorder);
            foreach (var item in request.Data.PurchaseOrderItems)
            {
                var purchaseorderItem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId, item.PurchaseorderItemName);
                purchaseorderItem.POValueUSD = item.POValueUSD;
                purchaseorderItem.Quantity = item.Quantity;
                await Repository.AddPurchaseorderItem(purchaseorderItem);
            }

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
