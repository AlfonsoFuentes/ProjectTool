using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ChangeUser
{
    public record ChangeDatabaseUserCommand : IRequest<IResult>; 

    public class ChangeDatabaseUserCommandHandler:IRequestHandler<ChangeDatabaseUserCommand,IResult>
    {
        private IAppDbContext _appDbContext;

        public ChangeDatabaseUserCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(ChangeDatabaseUserCommand request, CancellationToken cancellationToken)
        {
           var mwos=await _appDbContext.MWOs
                .Include(x=>x.BudgetItems)
                .ThenInclude(x=>x.TaxesItems)
                .Include(x=>x.PurchaseOrders)
                .ThenInclude(x=>x.PurchaseOrderItems).ToListAsync();
            foreach (var item in mwos)
            {
                item.Name = item.Name;
                _appDbContext.MWOs.Update(item);
                foreach(var budgetitem in item.BudgetItems)
                {
                    budgetitem.Name = budgetitem.Name;
                    _appDbContext.BudgetItems.Update(budgetitem);
                    foreach(var taxesitem in budgetitem.TaxesItems)
                    {
                        taxesitem.CreatedDate = taxesitem.CreatedDate;
                        _appDbContext.TaxesItems.Update(taxesitem);
                    }
                }
                foreach(var purchaseitem in item.PurchaseOrders)
                {
                    purchaseitem.PurchaseorderName=purchaseitem.PurchaseorderName;
                    _appDbContext.PurchaseOrders.Update(purchaseitem);
                    foreach(PurchaseOrderItem purchaseOrderItem in purchaseitem.PurchaseOrderItems)
                    {
                        purchaseOrderItem.POValueUSD=purchaseOrderItem.POValueUSD;
                        _appDbContext.PurchaseOrderItems.Update(purchaseOrderItem);
                    }
                }
            }
            var result=await _appDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
