using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.SapAdjust;

namespace Application.Features.SapAdjusts.Queries
{
    public record GetSapAdjustByIdQuery(Guid SapAdjustId) : IRequest<IResult<SapAdjustResponse>>;
    internal class GetSapAdjustByIdQueryHandler : IRequestHandler<GetSapAdjustByIdQuery, IResult<SapAdjustResponse>>
    {
        private ISapAdjustRepository Repository { get; set; }

        public GetSapAdjustByIdQueryHandler(ISapAdjustRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<SapAdjustResponse>> Handle(GetSapAdjustByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await Repository.GetSapAdAjustsById(request.SapAdjustId);

            SapAdjustResponse response = new()
            {
                ActualSap = query.ActualSap,
                ActualSoftware = query.ActualSoftware,
                CommitmentSap = query.CommitmentSap,
                CommitmentSoftware = query.CommitmentSoftware,
                Date = query.Date,
                ImageData = query.ImageDataFile,
                ImageTitle = query.ImageTitle,
                Justification = query.Justification,
                PotencialSap = query.PotencialSap,
                PotencialSoftware = query.PotencialSoftware,
                SapAdjustId = query.Id,
                
                
            };
            

            return Result<SapAdjustResponse>.Success(response);
        }
    }
}
