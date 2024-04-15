using Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Excels
{
    [ApiController]
    [Route("[controller]")]
    public class ExportListController : ControllerBase
    {
        private IMediator Mediator { get; set; }
        //private IExcelService ExcelService { get; set; }
        public ExportListController(IMediator mediator/*, IExcelService excelService*/)
        {
            Mediator = mediator;
            //ExcelService = excelService;
        }

        private string Now = DateTime.Now.Date.ToShortDateString();

        [HttpGet("suppliers")]
        public async Task<IActionResult> ExportSuppliers()
        {
            var result = await Mediator.Send(new GetAllSupplierQuery());
            //var resultExcel = await ExcelService.ExportAsync(result.Data.AsQueryable(), "Supplier List");
            return Ok(result);
          
        }
    }
}
