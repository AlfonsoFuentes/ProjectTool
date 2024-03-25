using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Currencies;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Queries
{
    public record GetByIdSupplierQuery(Guid Id) : IRequest<IResult<UpdateSupplierRequest>>;

    public class GetByIdSupplierQueryHandler : IRequestHandler<GetByIdSupplierQuery, IResult<UpdateSupplierRequest>>
    {
        private ISupplierRepository Repository { get; set; }

        public GetByIdSupplierQueryHandler(ISupplierRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UpdateSupplierRequest>> Handle(GetByIdSupplierQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetSupplierById(request.Id);

            if (row == null)
            {

                return Result<UpdateSupplierRequest>.Fail("Not found!");
            }
            UpdateSupplierRequest response = new()
            {
                Id = row.Id,
                Name = row.Name,
                Address = row.Address!,
                ContactEmail = row.ContactEmail!,
                ContactName = row.ContactName!,
                PhoneNumber = row.PhoneNumber!,
                SupplierCurrency = CurrencyEnum.GetType(row.SupplierCurrency),
                TaxCodeLD = row.TaxCodeLD,
                TaxCodeLP = row.TaxCodeLP,
                VendorCode= row.VendorCode!,
                NickName = row.NickName,
            };
            return Result<UpdateSupplierRequest>.Success(response);
        }
    }

}
