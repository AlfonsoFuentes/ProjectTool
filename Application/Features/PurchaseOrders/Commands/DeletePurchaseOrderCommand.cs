using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseorderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PurchaseOrders.Commands
{
    public record DeletePurchaseOrderCommand(Guid PurchaseOrderId) : IRequest<IResult>;

    internal class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand, IResult>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private IAppDbContext _appDbContext;
        public DeletePurchaseOrderCommandHandler(IPurchaseOrderRepository purchaseOrderRepository, IAppDbContext appDbContext)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderById(request.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return Result.Fail($"Purchase order Not found");
            }
            string Label = purchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ?
                $"Purchase Requisition {purchaseOrder.PurchaseRequisition} removed succesfully" :
              $"Purchase Order {purchaseOrder.PONumber} removed succesfully";
            await _purchaseOrderRepository.RemovePurchaseOrder(purchaseOrder);
            var result = await _appDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success(Label);
            }
            return Result.Fail($"It was not possible to remove purchase order");
        }
    }
}
