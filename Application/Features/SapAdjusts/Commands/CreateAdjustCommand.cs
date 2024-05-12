

namespace Application.Features.SapAdjusts.Commands
{
    public record CreateSapAdjustCommand(CreateSapAdjustRequest Data) : IRequest<IResult>;
    internal class CreateSapAdjustCommandHandler : IRequestHandler<CreateSapAdjustCommand, IResult>
    {
        private IRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public CreateSapAdjustCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(CreateSapAdjustCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetByIdAsync<MWO>(request.Data.MWOApproved.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.MWOCECName, ResponseType.NotFound, ClassNames.MWO));
            }

            if (mwo.SapAdjusts.Count==0)
            {
                await AddFirstRowToMWO(mwo);
            }
            var sapAdjust = mwo.AddAdjust();
            sapAdjust.BudgetCapital = request.Data.BudgetCapital;
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

            await Repository.AddAsync(sapAdjust);
            var result=await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, $"{Cache.GetSapAdjust}:{request.Data.MWOApproved.MWOId}");
    
            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.SapAdjust)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.SapAdjust));
          
        }
        async Task AddFirstRowToMWO(MWO mWO)
        {
            var sapadjust=mWO.AddAdjust();
            sapadjust.PotencialSap = 0;
            sapadjust.ActualSap = 0;
            sapadjust.CommitmentSap = 0;
            sapadjust.PotencialSoftware = 0;
            sapadjust.CommitmentSoftware = 0;
            sapadjust.ActualSoftware = 0;
            sapadjust.Date = mWO.ApprovedDate;
            sapadjust.BudgetCapital = 0;
            await Repository.AddAsync(sapadjust);
           
        }
    }
}
