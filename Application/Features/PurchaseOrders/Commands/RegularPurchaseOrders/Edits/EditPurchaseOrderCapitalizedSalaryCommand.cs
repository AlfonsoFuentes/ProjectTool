using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Edits
{
    public record EditPurchaseOrderCapitalizedSalaryCommand(EditCapitalizedSalaryPurchaseOrderRequestDto Data) : IRequest<IResult>;
    internal class EditPurchaseOrderCapitalizedSalaryCommandHandler : IRequestHandler<EditPurchaseOrderCapitalizedSalaryCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public EditPurchaseOrderCapitalizedSalaryCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(EditPurchaseOrderCapitalizedSalaryCommand request, CancellationToken cancellationToken)
        {
           
            var purchaseorder = await Repository.GetPurchaseOrderById(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Purchase order Not found");

            }
          
            purchaseorder.PurchaseorderName = request.Data.PurchaseOrderName;
            purchaseorder.PurchaseRequisition = request.Data.PurchaseorderNumber;
            purchaseorder.PONumber = request.Data.PurchaseorderNumber;
            purchaseorder.QuoteCurrency = -1;
            purchaseorder.IsAlteration = false;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.SPL = "";

            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueUSD = request.Data.SumPOValueUSD;
            purchaseorder.Actual = request.Data.SumPOValueUSD;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.QuoteNo = "";
            purchaseorder.TaxCode = "";

            purchaseorder.AccountAssigment = request.Data.MWOCECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency;
            await Repository.UpdatePurchaseOrder(purchaseorder);
            var purchaseorderitem = await Repository.GetPurchaseOrderItemById(request.Data.PurchaseOrderItem.PurchaseOrderItemId);
            purchaseorderitem.POValueUSD = request.Data.SumPOValueUSD;
            purchaseorderitem.Quantity = request.Data.PurchaseOrderItem.Quantity;
            purchaseorderitem.Actual = request.Data.SumPOValueUSD;
            await Repository.UpdatePurchaseOrderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await MWORepository.UpdateDataForApprovedMWO(purchaseorder.MWOId, cancellationToken);
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
