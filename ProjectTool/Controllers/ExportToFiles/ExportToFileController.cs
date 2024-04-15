using Application.Features.Suppliers.Queries;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.ExportToFiles
{
    [ApiController]
    [Route("[controller]")]
    public class ExportToFileController : ControllerBase
    {
        private IMediator Mediator { get; set; }
        private IExcelService ExcelService { get; set; }
        public ExportToFileController(IMediator mediator, IExcelService excelService)
        {
            Mediator = mediator;
            ExcelService = excelService;
        }
        [HttpGet("SupplierExcel")]
        public async Task<IActionResult> GetSuppliers()
        {
            var result = await Mediator.Send(new GetAllSupplierForFileResultQuery());
            var resultExcel = await ExcelService.ExportAsync(result.Data, "Supplier List");
            return Ok(resultExcel);
        }
    }
}
