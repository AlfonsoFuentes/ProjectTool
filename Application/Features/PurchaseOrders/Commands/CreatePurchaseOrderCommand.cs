using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands
{
    public record CreatePurchaseOrderCommand(CreatePurchaseOrderRequest Data) : IRequest<IResult>;

    internal class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreatePurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreatePurchaseOrderValidator(Repository);
            var validationResult = await validator.ValidateAsync(request.Data, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

            }
            var mwo = await Repository.GetMWOWithBudgetItemsAndPurchaseOrderById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail($"MWO Not found");

            }
            var purchaseorder = mwo.AddPurchaseOrder();
            purchaseorder.PurchaseorderName = request.Data.Name;
            purchaseorder.PurchaseRequisition = request.Data.PurchaseRequisition;
            purchaseorder.QuoteCurrency = request.Data.QuoteCurrency.Id;
            purchaseorder.IsAlteration = request.Data.IsAlteration;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.SPL = request.Data.SPL;
            purchaseorder.SupplierId = request.Data.SupplierId;
            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueUSD = request.Data.ItemsForm.Sum(x => x.TotalValueUSDItem);
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Created.Id;
            purchaseorder.QuoteNo = request.Data.QuoteNo;
            purchaseorder.TaxCode = request.Data.TaxCode;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.AccountAssigment = request.Data.AccountAssigment;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency.Id;
            await Repository.AddPurchaseOrder(purchaseorder);
            foreach (var item in request.Data.ItemsToCreate)
            {
                var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId, item.Name);
                purchaseorderitem.POValueUSD = item.UnitaryCostInUSD;
                purchaseorderitem.Quantity = item.Quantity;
                await Repository.AddPurchaseorderItem(purchaseorderitem);
            }

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
