using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.SapAdjust;

namespace Application.Features.SapAdjusts.Commands
{
    public record UpdateSapAdjustCommand(UpdateSapAdjustRequest Data) : IRequest<IResult>;
    internal class UpdateSapAdjustCommandHandler : IRequestHandler<UpdateSapAdjustCommand, IResult>
    {
        private ISapAdjustRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public UpdateSapAdjustCommandHandler(ISapAdjustRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(UpdateSapAdjustCommand request, CancellationToken cancellationToken)
        {



            var sapAdjust = await Repository.GetSapAdAjustsById(request.Data.SapAdjustId);
            if (sapAdjust == null) return Result.Fail("row not found");
            sapAdjust.PotencialSap = request.Data.PotencialSap;
            sapAdjust.ActualSap = request.Data.ActualSap;
            sapAdjust.CommitmentSap = request.Data.CommitmentSap;
            sapAdjust.PotencialSoftware = request.Data.PotencialSoftware;
            sapAdjust.CommitmentSoftware = request.Data.CommitmentSoftware;
            sapAdjust.ActualSoftware = request.Data.ActualSoftware;
            sapAdjust.Date = request.Data.Date.Date;
            sapAdjust.Justification = request.Data.Justification;
            sapAdjust.ImageDataFile = request.Data.ImageData;
            sapAdjust.ImageTitle = request.Data.ImageTitle;

            await Repository.UpdateSapAdjust(sapAdjust);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Sap Adjust was updated succesfully");
            }

            return Result.Fail("Sap adjust was not updated succesfully!");
        }
    }
}
