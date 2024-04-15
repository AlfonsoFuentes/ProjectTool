using MediatR;
using Shared.Commons.Results;
using Shared.Models.MWO;

namespace Application.Features.SapAdjusts.Queries
{
    public record GetCurrentStatusForMWOQuery(Guid MWOId) : IRequest<IResult<MWOResponse>>;
}
