using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseorderStatus;
using System.Linq.Expressions;

namespace Application.Features.MWOs.Queries
{
    public record GetAllMWOQuery : IRequest<IResult<List<MWOResponse>>>;

    public class GetAllMWOQueryHandler : IRequestHandler<GetAllMWOQuery, IResult<List<MWOResponse>>>
    {
        private IMWORepository Repository { get; set; }
        private CurrentUser CurrentUser { get; set; }
        public GetAllMWOQueryHandler(IMWORepository repository, CurrentUser currentUser)
        {
            Repository = repository;
            CurrentUser = currentUser;
        }


        public async Task<IResult<List<MWOResponse>>> Handle(GetAllMWOQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<MWO, bool>> filteruser = CurrentUser.IsSuperAdmin ? p => p.CreatedBy != null : p => p.CreatedBy == CurrentUser.UserId;

            var result = await Repository.GetMWOList();
            Expression<Func<MWO, MWOResponse>> expression = e => new MWOResponse
            {
                Id = e.Id,
                Name = e.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                CECName = e.Status == MWOStatusEnum.Approved.Id ? $"CEC0000{e.MWONumber}" : "",
                CostCenter = CostCenterEnum.GetName(e.CostCenter),
                BudgetItems=e.BudgetItems.Select(x=>new Shared.Models.BudgetItems.BudgetItemResponse()
                {
                     Id=x.Id,
                     Name=x.Name,
                     Type=BudgetItemTypeEnum.GetType(x.Type),

                }).ToList(),
                MWOType = MWOTypeEnum.GetType(e.Type),
                Status = MWOStatusEnum.GetType(e.Status),
                Capital = Math.Round(e.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget)),
                Expenses = Math.Round(e.BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget)),
                AssignedExpenses = Math.Round(e.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.POValueUSD)),
                ActualExpenses = Math.Round(e.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.Actual)),
                PotencialExpenses = Math.Round(e.PurchaseOrders.Where(x => x.IsAlteration == true && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.Actual)),

                AssignedCapital = Math.Round(e.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.POValueUSD)),
                ActualCapital = Math.Round(e.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.Actual)),
                PotencialCapital = Math.Round(e.PurchaseOrders.Where(x => x.IsAlteration == false && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.Actual)),
            };




            List<MWOResponse> retorno = result.Where(filteruser).Select(expression).ToList();
            retorno = retorno.OrderBy(x => x.Status.Id).ToList();
            return Result<List<MWOResponse>>.Success(retorno);

        }
    }

}
