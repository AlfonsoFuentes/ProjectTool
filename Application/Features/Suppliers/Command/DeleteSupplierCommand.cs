using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Command
{
    public record DeleteSupplierCommand(SupplierResponse Data):IRequest<IResult>;
    public class DeleteSupplierCommandHandler:IRequestHandler<DeleteSupplierCommand,IResult>
    {
        private IAppDbContext _appDbContext;
        public DeleteSupplierCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var row=await _appDbContext.Suppliers.FindAsync(request.Data.Id);
            if (row==null)
            {
                return Result.Fail($"{request.Data.Name} was not found!");
                
            }

            _appDbContext.Suppliers.Remove(row); 
            var result=await _appDbContext.SaveChangesAsync(cancellationToken); 
            if(result>0)
            {
                return Result.Success($"{request.Data.Name} was removed succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not removed succesfully!");
        }
    }

}
