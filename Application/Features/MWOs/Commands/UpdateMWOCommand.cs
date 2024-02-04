using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record UpdateMWOCommand(UpdateMWORequest Data) : IRequest<IResult>;
    public class UpdateMWOCommandHandler : IRequestHandler<UpdateMWOCommand, IResult>
    {
        private readonly IMWORepository Repository;

        private IAppDbContext AppDbContext;
        public UpdateMWOCommandHandler(IMWORepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;

            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(UpdateMWOCommand request, CancellationToken cancellationToken)
        {
            var row = await AppDbContext.MWOs.SingleOrDefaultAsync(x => x.Id == request.Data.Id);
            if (row == null)
            {
                return Result.Fail($"{request.Data.Name} was not found.");
            }
            row.Name = request.Data.Name;
            row.Type = request.Data.Type.Id;
            await Repository.UpdateMWO(row!);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not updated succesfully");
        }
    }
}
