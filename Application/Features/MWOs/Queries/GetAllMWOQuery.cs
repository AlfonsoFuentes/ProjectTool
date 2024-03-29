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
using System.Linq.Expressions;

namespace Application.Features.MWOs.Queries
{
    //public record GetAllMWOQuery : IRequest<IResult<List<MWOResponse>>>;

    //public class GetAllMWOQueryHandler : IRequestHandler<GetAllMWOQuery, IResult<List<MWOResponse>>>
    //{
    //    private IMWORepository Repository { get; set; }
    //    private CurrentUser CurrentUser { get; set; }
    //    public GetAllMWOQueryHandler(IMWORepository repository, CurrentUser currentUser)
    //    {
    //        Repository = repository;
    //        CurrentUser = currentUser;
    //    }


    //    public async Task<IResult<List<MWOResponse>>> Handle(GetAllMWOQuery request, CancellationToken cancellationToken)
    //    {
    //        Expression<Func<MWO, bool>> filteruser = CurrentUser.IsSuperAdmin ? p => p.CreatedBy != null : p => p.CreatedBy == CurrentUser.UserId;

    //        var result = await Repository.GetMWOList();
    //        Expression<Func<MWO, MWOResponse>> expression = e => new MWOResponse
    //        {
    //            Id = e.Id,
    //            Name = e.Name,
    //            CreatedBy = e.CreatedByUserName,
    //            CreatedOn = e.CreatedDate.ToString(),
    //            CECName = e.Status == MWOStatusEnum.Approved.Id ? $"CEC0000{e.MWONumber}" : "",
    //            CostCenter = CostCenterEnum.GetName(e.CostCenter),
    //            BudgetItems=e.BudgetItems.Select(x=>new Shared.Models.BudgetItems.BudgetItemResponse()
    //            {
    //                 Id=x.Id,
    //                 Name=x.Name,
    //                 Type=BudgetItemTypeEnum.GetType(x.Type),

    //            }).ToList(),
    //            MWOType = MWOTypeEnum.GetType(e.Type),
    //            Status = MWOStatusEnum.GetType(e.Status),
               
    //            PurchaseOrders=e.PurchaseOrders.Select(x=>new PurchaseOrderResponse()
    //            {
    //                MWOId = x.MWOId,
    //                MWOName = e.Name,
    //                PONumber = x.PONumber,
    //                AccountAssigment = x.AccountAssigment,
    //                CreatedBy = x.CreatedBy,
    //                CreatedOn = x.CreatedDate.ToShortDateString(),
    //                ExpectedOn = x.POExpectedDateDate == null ? string.Empty : x.POExpectedDateDate!.Value.ToShortDateString(),
    //                IsAlteration = x.IsAlteration,
    //                IsCapitalizedSalary = x.IsCapitalizedSalary,
    //                IsTaxEditable = x.IsTaxEditable,
    //                PurchaseOrderId = e.Id,
    //                PurchaseorderName = x.PurchaseorderName,
    //                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(x.PurchaseOrderStatus),
                   
    //                PurchaseOrderItems = x.PurchaseOrderItems.Select(y => new PurchaseorderItemsResponse
    //                {
    //                    BudgetItemId = y.BudgetItemId,
    //                    Actual = y.Actual,
    //                    POValueUSD = x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ? 0 : y.POValueUSD,
    //                    PurchaseorderItemId = y.Id,
    //                    Potencial = x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ? y.POValueUSD : 0,
    //                }).ToList(),



    //            }).ToList(),

    //        };




    //        List<MWOResponse> retorno = result.Where(filteruser).Select(expression).ToList();
    //        retorno = retorno.OrderBy(x => x.Status.Id).ToList();
    //        return Result<List<MWOResponse>>.Success(retorno);

    //    }
    //}

}
