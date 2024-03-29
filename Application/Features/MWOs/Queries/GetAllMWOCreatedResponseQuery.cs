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
    //public record GetAllMWOCreatedResponseQuery : IRequest<IResult<IEnumerable<MWOResponse>>>;
    //public class GetAllMWOCreatedResponseQueryHandler : IRequestHandler<GetAllMWOCreatedResponseQuery, IResult<IEnumerable<MWOResponse>>>
    //{
    //    private IMWORepository Repository { get; set; }
    //    private CurrentUser CurrentUser { get; set; }
    //    public GetAllMWOCreatedResponseQueryHandler(IMWORepository repository, CurrentUser currentUser)
    //    {
    //        Repository = repository;
    //        CurrentUser = currentUser;
    //    }


    //    public async Task<IResult<IEnumerable<MWOResponse>>> Handle(GetAllMWOCreatedResponseQuery request, CancellationToken cancellationToken)
    //    {
    //       var MWOsCreated = await GetCreated();


    //        return Result<IEnumerable<MWOResponse>>.Success(MWOsCreated);

    //    }
    //    async Task<IEnumerable<MWOResponse>> GetCreated()
    //    {
    //        var query = await Repository.GetMWOCreatedList(CurrentUser);
    //        return query.Select(e => new MWOResponse
    //        {
    //            Id = e.Id,
    //            Name = e.Name,
    //            CreatedBy = e.CreatedByUserName,
    //            CreatedOn = e.CreatedDate.ToString(),
    //            CECName = e.Status == MWOStatusEnum.Approved.Id ? $"CEC0000{e.MWONumber}" : "",
    //            CostCenter = CostCenterEnum.GetName(e.CostCenter),
    //            BudgetItems = e.BudgetItems.Select(x => new Shared.Models.BudgetItems.BudgetItemResponse()
    //            {
    //                Id = x.Id,
    //                Name = x.Name,
    //                Type = BudgetItemTypeEnum.GetType(x.Type),
    //                Quantity = x.Quantity,
    //                UnitaryCost = x.UnitaryCost,
    //                Budget = x.Budget,

    //            }).ToList(),
    //            MWOType = MWOTypeEnum.GetType(e.Type),
    //            Status = MWOStatusEnum.GetType(e.Status),


    //        });

    //    }
        
    //}
}
