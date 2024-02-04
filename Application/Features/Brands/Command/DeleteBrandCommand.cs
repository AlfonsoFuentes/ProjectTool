using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Brands;

namespace Application.Features.Brands.Command
{
    public record DeleteBrandCommand(BrandResponse Data):IRequest<IResult>;
    public class DeleteBrandCommandHandler:IRequestHandler<DeleteBrandCommand,IResult>
    {
        private IAppDbContext _appDbContext;
        public DeleteBrandCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var row=await _appDbContext.Brands.FindAsync(request.Data.Id);
            if (row==null)
            {
                return Result.Fail($"{request.Data.Name} was not found!");
                
            }

            _appDbContext.Brands.Remove(row); 
            var result=await _appDbContext.SaveChangesAsync(cancellationToken); 
            if(result>0)
            {
                return Result.Success($"{request.Data.Name} was removed succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not removed succesfully!");
        }
    }

}
