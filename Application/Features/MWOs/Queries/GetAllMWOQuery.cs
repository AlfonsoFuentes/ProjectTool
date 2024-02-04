using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            Expression<Func<MWO, bool>> filteruser = CurrentUser.IsSuperAdmin? p => p.CreatedBy !=null: p => p.CreatedBy == CurrentUser.UserId;

            var result = await Repository.GetMWOList();
            Expression<Func<MWO, MWOResponse>> expression = e => new MWOResponse
            {
                Id = e.Id,
                Name = e.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                Type = MWOTypeEnum.GetName(e.Type),
                Status= MWOStatusEnum.GetName(e.Status),
                

            };




            List<MWOResponse> retorno = result.Where(filteruser).Select(expression).ToList();
            return Result<List<MWOResponse>>.Success(retorno);

        }
    }

}
