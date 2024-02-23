using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.Currencies;
using Shared.Models.Suppliers;
using System.Linq.Expressions;

namespace Application.Features.Suppliers.Queries
{
    public record GetAllSupplierQuery() : IRequest<IResult<List<SupplierResponse>>>;

    public class GetAllSupplierQueryHandler : IRequestHandler<GetAllSupplierQuery, IResult<List<SupplierResponse>>>
    {
        private ISupplierRepository Repository { get; set; }
        private CurrentUser CurrentUser { get; set; }
        public GetAllSupplierQueryHandler(ISupplierRepository repository, CurrentUser currentUser)
        {
            Repository = repository;
            CurrentUser = currentUser;
        }

        public async Task<IResult<List<SupplierResponse>>> Handle(GetAllSupplierQuery request, CancellationToken cancellationToken)
        {

            var rows = await Repository.GetSupplierList();
            Expression<Func<Supplier, SupplierResponse>> expression = e => new SupplierResponse
            {
                Id = e.Id,
                Name = e.Name,
                Address = e.Address!,
                ContactEmail = e.ContactEmail!,
                ContactName = e.ContactName!,
                PhoneNumber = e.PhoneNumber!,
                SupplierCurrency = CurrencyEnum.GetType(e.SupplierCurrency),
                TaxCodeLD = e.TaxCodeLD,
                TaxCodeLP = e.TaxCodeLP,
                VendorCode = e.VendorCode,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),

            };
            var result = rows.Select(expression).ToList();
            return Result<List<SupplierResponse>>.Success(result);
        }
    }

}
