using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Globalization;
using System.Linq.Expressions;

namespace Application.Features.BudgetItems.Queries
{

    public record GetAllBudgetItemQuery(Guid MWOId) : IRequest<IResult<ListBudgetItemResponse>>;

    public class GetAllBudgetItemQueryHandler : IRequestHandler<GetAllBudgetItemQuery, IResult<ListBudgetItemResponse>>
    {
        private IBudgetItemRepository Repository { get; set; }
        private CurrentUser CurrentUser { get; set; }
        public GetAllBudgetItemQueryHandler(IBudgetItemRepository repository, CurrentUser currentUser)
        {
            Repository = repository;
            CurrentUser = currentUser;
        }

        public async Task<IResult<ListBudgetItemResponse>> Handle(GetAllBudgetItemQuery request, CancellationToken cancellationToken)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ListBudgetItemResponse response = new ListBudgetItemResponse();
            var mwo = await Repository.GetMWOById(request.MWOId);
            if (mwo == null) Result<ListBudgetItemResponse>.Fail("MWO Not found");

            response.MWO = new MWOResponse() { Id = mwo!.Id, Name = mwo.Name, Status = MWOStatusEnum.GetType(mwo.Status) };

            var purchaseorderitems = await Repository.GetPurchaseOrderItemsByMWOId(request.MWOId);
            var rows = await Repository.GetBudgetItemList(request.MWOId);
           
         
            Expression<Func<BudgetItem, BudgetItemResponse>> expression = e => new BudgetItemResponse
            {
                MWOId = e.MWOId,
                Id = e.Id,
                Name = e.Name,
                MWOApproved = mwo.Status == MWOStatusEnum.Approved.Id,
                Type = BudgetItemTypeEnum.GetType(e.Type),
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(e.Type)}{e.Order}",
                Budget = e.Budget,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                IsNotAbleToEditDelete = e.IsNotAbleToEditDelete,
                IsMainItemTaxesNoProductive = e.IsMainItemTaxesNoProductive,
                Quantity = e.Quantity,
                UnitaryCost = e.UnitaryCost,
                MWOName = mwo.Name,
                Percentage = e.Percentage,
              
               
                Brand = e.Brand == null ? string.Empty : e.Brand.Name,
                //Parent=response,

            };
            var result = rows.Select(expression).ToList();
            

            result = result.OrderBy(x => x.Nomenclatore).ToList();
            response.BudgetItems = result;
          
            
            return Result<ListBudgetItemResponse>.Success(response);
        }

    }

}



