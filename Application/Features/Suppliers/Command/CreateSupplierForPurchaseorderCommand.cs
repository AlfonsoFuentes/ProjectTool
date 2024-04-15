using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Currencies;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Command
{
    public record CreateSupplierForPurchaseorderCommand(CreateSupplierRequest Data) : IRequest<IResult<SupplierResponse>>;
    public class CreateSupplierForPurchaseorderCommandHandler : IRequestHandler<CreateSupplierForPurchaseorderCommand, IResult<SupplierResponse>>
    {
        private ISupplierRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateSupplierForPurchaseorderCommandHandler(IAppDbContext appDbContext, ISupplierRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult<SupplierResponse>> Handle(CreateSupplierForPurchaseorderCommand request, CancellationToken cancellationToken)
        {

            var row = Supplier.Create(request.Data.Name, request.Data.VendorCode, request.Data.TaxCodeLD,
                request.Data.TaxCodeLP, request.Data.SupplierCurrency.Id);

            row.NickName = request.Data.NickName;
            await Repository.AddSupplier(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            SupplierResponse response = new SupplierResponse()
            {
                Id = row.Id,
                Name = row.Name,
                NickName = row.NickName,
                VendorCode = row.VendorCode,
                TaxCodeLD = row.TaxCodeLD,
                TaxCodeLP = row.TaxCodeLP,
                SupplierCurrency = CurrencyEnum.GetType(row.SupplierCurrency),

            };

            if (result > 0)
            {
                return Result<SupplierResponse>.Success(response);
            }
            return Result<SupplierResponse>.Fail($"{request.Data.Name} was not created succesfully!");
        }
    }
}
