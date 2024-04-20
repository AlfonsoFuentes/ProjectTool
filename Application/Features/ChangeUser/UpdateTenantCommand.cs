using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using System.Threading;

namespace Application.Features.ChangeUser
{
    public record UpdateTenantCommand:IRequest<IResult>;
    internal class UpdateTenantCommandHandler:IRequestHandler<UpdateTenantCommand,IResult>
    {
        private IAppDbContext _appDbContext;

        public UpdateTenantCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            await UpdateBudgetItems(cancellationToken);
            await UpdateMWO(cancellationToken);
            await UpdatePurchaseOrders(cancellationToken);
            await UpdatePurchaseOrderItems(cancellationToken);
            await UpdateTaxesItems(cancellationToken);
            await UpdateDownPayments(cancellationToken);
            await UpdateSapAdjusts(cancellationToken);

            var result=await _appDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        async Task UpdateBudgetItems(CancellationToken cancellationToken)
        {
            var items = await _appDbContext.BudgetItems.ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                item.TenantId = item.TenantId;
                _appDbContext.BudgetItems.Update(item);
            }
        }
        async Task UpdateMWO(CancellationToken cancellationToken)
        {
            var items = await _appDbContext.MWOs.ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                item.TenantId = item.TenantId;
                _appDbContext.MWOs.Update(item);
            }
        }
        async Task UpdatePurchaseOrders(CancellationToken cancellationToken)
        {
            var items = await _appDbContext.PurchaseOrders.ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                item.TenantId = item.TenantId;
                _appDbContext.PurchaseOrders.Update(item);
            }
        }
        async Task UpdatePurchaseOrderItems(CancellationToken cancellationToken)
        {
            var items = await _appDbContext.PurchaseOrderItems.ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                item.TenantId = item.TenantId;
                _appDbContext.PurchaseOrderItems.Update(item);
            }
        }
        async Task UpdateTaxesItems(CancellationToken cancellationToken)
        {
            var items = await _appDbContext.TaxesItems.ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                item.TenantId = item.TenantId;
                _appDbContext.TaxesItems.Update(item);
            }
        }
        async Task UpdateDownPayments(CancellationToken cancellationToken)
        {
            var items = await _appDbContext.DownPayments.ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                item.TenantId = item.TenantId;
                _appDbContext.DownPayments.Update(item);
            }
        }
        async Task UpdateSapAdjusts(CancellationToken cancellationToken)
        {
            var items = await _appDbContext.SapAdjusts.ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                item.TenantId = item.TenantId;
                _appDbContext.SapAdjusts.Update(item);
            }
        }
    }
}
