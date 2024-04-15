using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PurchaseOrders.Commands.RecalculateData
{
    public record RecalculatePurchaseOrderCommand : IRequest<IResult>;

    internal class RecalculatePurchaseOrderCommandHandler : IRequestHandler<RecalculatePurchaseOrderCommand, IResult>
    {
        private IAppDbContext _appDbContext;

        public RecalculatePurchaseOrderCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(RecalculatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrderItemss = await _appDbContext.PurchaseOrderItems.ToListAsync(cancellationToken);

            foreach (var item in purchaseOrderItemss)
            {
                item.UnitaryValueCurrency = item.UnitaryValueCurrency ;
                _appDbContext.PurchaseOrderItems.Update(item);

            }
            var result = await _appDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        //public async Task<IResult> Handle(RecalculatePurchaseOrderCommand request, CancellationToken cancellationToken)
        //{
        //    var purchaseOrders = await _appDbContext.PurchaseOrders.Include(x => x.PurchaseOrderItems).ToListAsync(cancellationToken);

        //    foreach (var purchaseOrder in purchaseOrders)
        //    {
        //        var trm = purchaseOrder.Currency == CurrencyEnum.COP.Id ? purchaseOrder.USDCOP :
        //            purchaseOrder.Currency == CurrencyEnum.EUR.Id ? purchaseOrder.USDEUR : 1;

        //        //purchaseOrder.POValueCurrency = trm * purchaseOrder.POValueUSD;
        //        //purchaseOrder.ActualCurrency = trm * purchaseOrder.Actual;
        //        _appDbContext.PurchaseOrders.Update(purchaseOrder);
        //        foreach (var purchaseorderitem in purchaseOrder.PurchaseOrderItems)
        //        {
        //            //purchaseorderitem.POValueCurrency = trm * purchaseorderitem.POValueUSD;
        //            //purchaseorderitem.ActualCurrency = trm * purchaseorderitem.Actual;
        //            _appDbContext.PurchaseOrderItems.Update(purchaseorderitem);
        //        }
        //    }
        //    var result = await _appDbContext.SaveChangesAsync(cancellationToken);

        //    return Result.Success();
        //}
    }
}
