using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.SapAdjust;

namespace Application.Features.SapAdjusts.Queries
{
    public record GetSapAdjustByIdToUpdateQuery(Guid SapAdjustId) : IRequest<IResult<UpdateSapAdjustRequest>>;
    internal class GetSapAdjustByIdToUpdateQueryHandler : IRequestHandler<GetSapAdjustByIdToUpdateQuery, IResult<UpdateSapAdjustRequest>>
    {
        private ISapAdjustRepository Repository { get; set; }

        public GetSapAdjustByIdToUpdateQueryHandler(ISapAdjustRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UpdateSapAdjustRequest>> Handle(GetSapAdjustByIdToUpdateQuery request, CancellationToken cancellationToken)
        {
            var query = await Repository.GetSapAdAjustsById(request.SapAdjustId);

            UpdateSapAdjustRequest response = new()
            {
                ActualSap = query.ActualSap,
                CommitmentSap = query.CommitmentSap,
                Date = query.Date,
                ImageData = query.ImageDataFile,
                ImageTitle = query.ImageTitle,
                Justification = query.Justification,
                PotencialSap = query.PotencialSap,
                SapAdjustId = query.Id,
                MWOId = query.MWOId,
                ActualSoftware = query.ActualSoftware,
                CommitmentSoftware = query.CommitmentSoftware,
                MWOName = query.MWO.Name,
                CECMWOName=$"CEC0000{query.MWO.MWONumber}",
                PotencialSoftware = query.PotencialSoftware,
                BudgetCapital = query.BudgetCapital,


            };


            return Result<UpdateSapAdjustRequest>.Success(response);
        }
    }
}
