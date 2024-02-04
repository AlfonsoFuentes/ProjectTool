using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BudgetItems.Queries
{
    public record GetSumPercentageEngContQuery(Guid MWOId):IRequest<IResult<double>>;
    public class GetSumPercentageEngContQueryHandler:IRequestHandler<GetSumPercentageEngContQuery, IResult<double>>
    {
        private IBudgetItemRepository Repository { get; set; }

        public GetSumPercentageEngContQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<double>> Handle(GetSumPercentageEngContQuery request, CancellationToken cancellationToken)
        {
            var retorno=await Repository.GetSumEngConPercentage(request.MWOId);

            return Result<double>.Success(retorno);
        }
    }
}
