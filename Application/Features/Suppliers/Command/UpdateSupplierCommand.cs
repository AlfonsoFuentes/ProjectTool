using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Command
{
    public record UpdateSupplierCommand(UpdateSupplierRequest Data) : IRequest<IResult>;
    public class UpdateSupplierCommandHandler:IRequestHandler<UpdateSupplierCommand,IResult>
    {
        private ISupplierRepository Repository { get; set; }
        private IAppDbContext Context { get; set; }
        public UpdateSupplierCommandHandler(ISupplierRepository repository,IAppDbContext context)
        {
            Repository = repository;
            Context = context;
            
        }

        public async Task<IResult> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
           
            var row = await Repository.GetSupplierById(request.Data.Id);
            if (row == null) return Result.Fail($"{request.Data.Name} was not found!");

            row.Address=request.Data.Address;
            row.PhoneNumber=request.Data.PhoneNumber;
            row.ContactEmail=request.Data.ContactEmail;
            row.ContactName=request.Data.ContactName;
            row.Name=request.Data.Name;
            row.VendorCode=request.Data.VendorCode;
            row.TaxCodeLD=request.Data.TaxCodeLD;
            row.TaxCodeLP=request.Data.TaxCodeLP;
            row.SupplierCurrency=request.Data.SupplierCurrency.Id;

            row.NickName = request.Data.NickName;
            await Repository.UpdateSupplier(row);
            var result=await Context.SaveChangesAsync(cancellationToken);
            if(result>0)
            {
                return Result.Success($"{request.Data.Name} was updated succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not updated succesfully!");
        }
    }

}
