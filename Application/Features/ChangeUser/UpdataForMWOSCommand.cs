using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.MWOStatus;

namespace Application.Features.ChangeUser
{
    public record UpdataForMWOSCommand:IRequest<IResult>;
    public class UpdataForMWOSCommandHandler:IRequestHandler<UpdataForMWOSCommand,IResult>
    {
        private IAppDbContext AppDbContext { get; set; }

        private IMWORepository MWORepository { get; set; }
        public UpdataForMWOSCommandHandler(IAppDbContext appDbContext, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(UpdataForMWOSCommand request, CancellationToken cancellationToken)
        {
            var mwoNotapproved = await AppDbContext.MWOs
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.Status == MWOStatusEnum.Created.Id).ToListAsync();
            //foreach (var mwo in mwoNotapproved)
            //{
            //    await MWORepository.UpdateDataForNotApprovedMWO(mwo.Id,cancellationToken);
            //}
            var mwoapproved = await AppDbContext.MWOs
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.Status == MWOStatusEnum.Approved.Id).ToListAsync();
            //foreach (var mwo in mwoapproved)
            //{
            //    await MWORepository.UpdateDataForNotApprovedMWO(mwo.Id, cancellationToken);
            //    await MWORepository.UpdateDataForApprovedMWO(mwo.Id, cancellationToken);
            //}

            return Result.Success();

        }
    }
}
