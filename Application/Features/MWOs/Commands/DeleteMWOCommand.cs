using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record DeleteMWOCommand(MWOResponse data) : IRequest<IResult>;

    public class DeleteMWOCommandHandler:IRequestHandler<DeleteMWOCommand,IResult>
    {
        private IAppDbContext _appDbContext;

        public DeleteMWOCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(DeleteMWOCommand request, CancellationToken cancellationToken)
        {
            var row=await _appDbContext.MWOs.FindAsync(request.data.Id);
            if (row == null)
            {
                return Result.Fail($"{request.data.Name} Not found");

            }
            _appDbContext.MWOs.Remove(row);

            var result=await _appDbContext.SaveChangesAsync(cancellationToken);
            if(result>0)

            {
                return Result.Success($"{request.data.Name}  removed succesfully");
            }
            return Result.Fail($"{request.data.Name} Not found");
        }
    }

}
