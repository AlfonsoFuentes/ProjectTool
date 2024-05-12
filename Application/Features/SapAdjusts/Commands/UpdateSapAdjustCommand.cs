using Azure;

namespace Application.Features.SapAdjusts.Commands
{
    public record UpdateSapAdjustCommand(UpdateSapAdjustRequest Data) : IRequest<IResult>;
    internal class UpdateSapAdjustCommandHandler : IRequestHandler<UpdateSapAdjustCommand, IResult>
    {
        private IRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public UpdateSapAdjustCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(UpdateSapAdjustCommand request, CancellationToken cancellationToken)
        {



            var sapAdjust = await Repository.GetSapAdAjustsById(request.Data.SapAdjustId);
            if (sapAdjust == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.SapAdjust));
            }
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

            await Repository.UpdateAsync(sapAdjust);
            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, $"{Cache.GetSapAdjust}:{request.Data.MWOId}");
            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Delete, ClassNames.SapAdjust)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.SapAdjust));
        }
    }
}
