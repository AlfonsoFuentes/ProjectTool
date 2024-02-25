using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands
{
    public record CreatePurchaseOrderCapitalizedSalaryCommand(CreateCapitalizedSalaryPurchaseOrderRequest Data) : IRequest<IResult>;
    internal class CreatePurchaseOrderCapitalizedSalaryCommandHandler : IRequestHandler<CreatePurchaseOrderCapitalizedSalaryCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreatePurchaseOrderCapitalizedSalaryCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(CreatePurchaseOrderCapitalizedSalaryCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateCapitalizedSalaryPurchaseOrderValidator(Repository);
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
            purchaseorder.PurchaseorderName = request.Data.PurchaseOrderName;
            purchaseorder.PurchaseRequisition = request.Data.PurchaseorderNumber;
            purchaseorder.PONumber = request.Data.PurchaseorderNumber;
            purchaseorder.QuoteCurrency = -1;
            purchaseorder.IsAlteration = false;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.SPL = "";
            
            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueUSD = request.Data.SumPOValueUSD;
            purchaseorder.Actual = request.Data.SumPOValueUSD;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.QuoteNo = "";
            purchaseorder.TaxCode = "";
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.AccountAssigment = request.Data.MWOCECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency.Id;
            await Repository.AddPurchaseOrder(purchaseorder);
            var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(request.Data.PurchaseOrderItem.BudgetItemId, request.Data.PurchaseOrderItem.Name);
            purchaseorderitem.POValueUSD = request.Data.PurchaseOrderItem.UnitaryCostInUSD;
            purchaseorderitem.Quantity = request.Data.PurchaseOrderItem.Quantity;
            purchaseorderitem.Actual = request.Data.SumPOValueUSD;
            await Repository.AddPurchaseorderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
