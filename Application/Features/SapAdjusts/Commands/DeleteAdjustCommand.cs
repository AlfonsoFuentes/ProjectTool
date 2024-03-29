using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.SapAdjust;

namespace Application.Features.SapAdjusts.Commands
{
    public record DeleteSapAdjustCommand(SapAdjustResponse Data) : IRequest<IResult>;
    internal class DeleteSapAdjustCommandHandler : IRequestHandler<DeleteSapAdjustCommand, IResult>
    {
        private ISapAdjustRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public DeleteSapAdjustCommandHandler(ISapAdjustRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(DeleteSapAdjustCommand request, CancellationToken cancellationToken)
        {

            await Repository.DeleteSapAdAjust(request.Data.SapAdjustId);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Sap Adjust was deleted succesfully");
            }

            return Result.Fail("Sap adjust was not deleted succesfully!");
        }
    }
}
