using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Command
{
    public record CreateSupplierCommand(CreateSupplierRequestDto Data ):IRequest<IResult>;

    public class CreateSupplierCommandHandler:IRequestHandler<CreateSupplierCommand,IResult>
    {
        private ISupplierRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateSupplierCommandHandler(IAppDbContext appDbContext, ISupplierRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {

           
            var row = Supplier.Create(request.Data.Name, request.Data.VendorCode,request.Data.TaxCodeLD,
                request.Data.TaxCodeLP,request.Data.SupplierCurrency);
            row.NickName=request.Data.NickName;

              await Repository.AddSupplier(row);
            var result=await AppDbContext.SaveChangesAsync(cancellationToken);
            if(result>0)
            {
                return Result.Success($"{request.Data.Name} created succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not created succesfully!");
        }
    }

}
