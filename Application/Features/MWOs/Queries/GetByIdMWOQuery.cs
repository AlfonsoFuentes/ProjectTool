using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;
using System.Linq.Expressions;

namespace Application.Features.MWOs.Queries
{
    public record GetByIdMWOQuery(Guid Id) : IRequest<IResult<UpdateMWORequest>>;
    public class GetByIdMWOQueryHandler : IRequestHandler<GetByIdMWOQuery, IResult<UpdateMWORequest>>
    {
        private IMWORepository Repository { get; set; }
 
        public GetByIdMWOQueryHandler(IMWORepository repository)
        {
            Repository = repository;
        
        }


        public async Task<IResult<UpdateMWORequest>> Handle(GetByIdMWOQuery request, CancellationToken cancellationToken)
        {
            var result = await Repository.GetMWOById(request.Id);
            if (result == null) return Result<UpdateMWORequest>.Fail("Not Found");

            UpdateMWORequest retorno = new()
            {
                Id = result.Id,
                Name = result.Name,
                Type = MWOTypeEnum.GetType(result.Type),
            };
            return Result<UpdateMWORequest>.Success(retorno);

        }
    }

}
