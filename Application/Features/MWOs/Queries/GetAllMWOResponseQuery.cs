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
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Diagnostics;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Features.MWOs.Queries
{
    public record GetAllMWOResponseQuery : IRequest<IResult<MWOResponseList>>;
    public class GetAllMWOResponseQueryHandler : IRequestHandler<GetAllMWOResponseQuery, IResult<MWOResponseList>>
    {
        private IMWORepository Repository { get; set; }
        private CurrentUser CurrentUser { get; set; }
        public GetAllMWOResponseQueryHandler(IMWORepository repository, CurrentUser currentUser)
        {
            Repository = repository;
            CurrentUser = currentUser;
        }


        public async Task<IResult<MWOResponseList>> Handle(GetAllMWOResponseQuery request, CancellationToken cancellationToken)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var mwoList = await Repository.GetMWOList(CurrentUser);
            sw.Stop();
            var elapse1 = sw.ElapsedMilliseconds;

            sw= Stopwatch.StartNew();
            MWOResponseList response = new();
            response.MWOsCreated = await GetCreated(mwoList);
            response.MWOsApproved = await GetApproved(mwoList);
            response.MWOsClosed = await GetClosed(mwoList);
            sw.Stop();
            var elapse2 = sw.ElapsedMilliseconds;
            return Result<MWOResponseList>.Success(response);

        }
        Task<IEnumerable<MWOResponse>> GetCreated(IEnumerable<MWO> query)
        {
            query = query.Where(x=>x.Status==MWOStatusEnum.Created.Id);
            var result=query.Select(e => new MWOResponse
            {
                Id = e.Id,
                Name = e.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                CECName = e.Status == MWOStatusEnum.Approved.Id ? $"CEC0000{e.MWONumber}" : "",
                CostCenter = CostCenterEnum.GetName(e.CostCenter),
                
                Capital = e.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                Expenses = e.BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                IsAssetProductive = e.IsAssetProductive,
               
                
                MWOType = MWOTypeEnum.GetType(e.Type),
                Status = MWOStatusEnum.GetType(e.Status),


            });
            return Task.FromResult(result);
        }
        Task<IEnumerable<MWOResponse>> GetApproved(IEnumerable<MWO> query)
        {
            query = query.Where(x => x.Status == MWOStatusEnum.Approved.Id);
            var result= query.Select(e => new MWOResponse
            {
                Id = e.Id,
                Name = e.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                CECName = e.Status == MWOStatusEnum.Approved.Id ? $"CEC0000{e.MWONumber}" : "",
                CostCenter = CostCenterEnum.GetName(e.CostCenter),
                Capital = e.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                Expenses = e.BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),

                PotencialCapital = e.PurchaseOrders.Where(x => x.IsAlteration == false 
                && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                PotencialExpenses = e.PurchaseOrders.Where(x => x.IsAlteration == true
                && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                
                ActualCapital = e.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.ActualUSD),
                ActualExpenses = e.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.ActualUSD),
                
                POValueCapital = e.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.POValueUSD),
                POValueExpenses = e.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.POValueUSD),
                
                MWOType = MWOTypeEnum.GetType(e.Type),
                Status = MWOStatusEnum.GetType(e.Status),
                

            });
            return Task.FromResult(result);
        }
        Task<IEnumerable<MWOResponse>> GetClosed(IEnumerable<MWO> query)
        {
            query = query.Where(x => x.Status == MWOStatusEnum.Approved.Id);
            var result=query.Select(e => new MWOResponse
            {
                Id = e.Id,
                Name = e.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                CECName = e.Status == MWOStatusEnum.Approved.Id ? $"CEC0000{e.MWONumber}" : "",
                CostCenter = CostCenterEnum.GetName(e.CostCenter),
                Capital = e.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                Expenses = e.BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),

                PotencialCapital = e.PurchaseOrders.Where(x => x.IsAlteration == false && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                PotencialExpenses = e.PurchaseOrders.Where(x => x.IsAlteration == true && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                ActualCapital = e.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.ActualUSD),
                ActualExpenses = e.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.ActualUSD),
                POValueCapital = e.PurchaseOrders.Where(x => x.IsAlteration == false).Sum(x => x.POValueUSD),
                POValueExpenses = e.PurchaseOrders.Where(x => x.IsAlteration == true).Sum(x => x.POValueUSD),
                MWOType = MWOTypeEnum.GetType(e.Type),
                Status = MWOStatusEnum.GetType(e.Status),
                

            });
            return Task.FromResult(result);
        }
    }
}
