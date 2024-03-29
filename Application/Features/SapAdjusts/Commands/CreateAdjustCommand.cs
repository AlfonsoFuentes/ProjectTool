using Application.Interfaces;
using Azure.Core;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.SapAdjust;
using System.Threading;

namespace Application.Features.SapAdjusts.Commands
{
    public record CreateSapAdjustCommand(CreateSapAdjustRequestDto Data) : IRequest<IResult>;
    internal class CreateSapAdjustCommandHandler : IRequestHandler<CreateSapAdjustCommand, IResult>
    {
        private ISapAdjustRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public CreateSapAdjustCommandHandler(ISapAdjustRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(CreateSapAdjustCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetMWOById(request.Data.MWOId);
            if(mwo == null) return Result.Fail("MWO not found");

            if(mwo.SapAdjusts.Count==0)
            {
                await AddFirstRowToMWO(mwo);
            }
            var sapAdjust = mwo.AddAdjust();
            sapAdjust.BudgetCapital = request.Data.BudgetCapital;
            sapAdjust.PotencialSap=request.Data.PotencialSap;
            sapAdjust.ActualSap=request.Data.ActualSap;
            sapAdjust.CommitmentSap=request.Data.CommitmentSap;
            sapAdjust.PotencialSoftware=request.Data.PotencialSoftware;
            sapAdjust.CommitmentSoftware=request.Data.CommitmentSoftware;
            sapAdjust.ActualSoftware=request.Data.ActualSoftware;
            sapAdjust.Date=request.Data.Date.Date;
            sapAdjust.Justification=request.Data.Justification;
            sapAdjust.ImageDataFile = request.Data.ImageData;
            sapAdjust.ImageTitle=request.Data.ImageTitle;

            await Repository.AddSapAdAjust(sapAdjust);
            var result=await AppDbContext.SaveChangesAsync(cancellationToken);
            if(result>0)
            {
                return Result.Success($"Sap Adjust for MWO: {mwo.Name} at date: {request.Data.Date.ToShortDateString()} was created succesfully");
            }

            return Result.Fail("Sap adjust was not created succesfully!");
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
            await Repository.AddSapAdAjust(sapadjust);
           
        }
    }
}
