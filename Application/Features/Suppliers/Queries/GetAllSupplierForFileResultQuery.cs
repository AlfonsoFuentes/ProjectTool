using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Currencies;
using Shared.Models.Suppliers;
using System.Linq.Expressions;

namespace Application.Features.Suppliers.Queries
{
    public record GetAllSupplierForFileResultQuery():IRequest<IResult<IQueryable<SupplierExportFileResponse>>>;
    public class GetAllSupplierForFileResultQueryHandler : IRequestHandler<GetAllSupplierForFileResultQuery, IResult<IQueryable<SupplierExportFileResponse>>>
    {
        private ISupplierRepository Repository { get; set; }
  
        public GetAllSupplierForFileResultQueryHandler(ISupplierRepository repository)
        {
            Repository = repository;
    
        }

        public async Task<IResult<IQueryable<SupplierExportFileResponse>>> Handle(GetAllSupplierForFileResultQuery request, CancellationToken cancellationToken)
        {

            var rows = await Repository.GetSupplierList();
            Expression<Func<Supplier, SupplierExportFileResponse>> expression = e => new SupplierExportFileResponse
            {
                
                Name = e.Name,
                Address = e.Address!,
                ContactEmail = e.ContactEmail!,
                ContactName = e.ContactName!,
                PhoneNumber = e.PhoneNumber!,
                SupplierCurrency = CurrencyEnum.GetName(e.SupplierCurrency),
                TaxCodeLD = e.TaxCodeLD,
                TaxCodeLP = e.TaxCodeLP,
                VendorCode = e.VendorCode,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToShortDateString(),
                NickName = e.NickName,

            };
            try
            {
                var result = rows!.Select(expression);
                return Result<IQueryable<SupplierExportFileResponse>>.Success(result);
            }
            catch (Exception ex)
            {
                string exm = ex.Message;
            }
            return Result<IQueryable<SupplierExportFileResponse>>.Fail();

        }
    }

}
