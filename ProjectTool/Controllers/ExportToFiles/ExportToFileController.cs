using Application.Features.BudgetItems.Queries;
using Application.Features.MWOs.Queries;
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
            var resultExcel = await ExcelService.ExportAsync(result, "Supplier List");
            return Ok(resultExcel);
        }
        [HttpGet("BudgetItemsNotApprovedExcel/{MWOId}")]
        public async Task<IActionResult> GetBudgetItemsNotApproved(Guid MWOId)
        {
            var result = await Mediator.Send(new GetBudgetItemsNotApprovedQuery(MWOId));
            var resultExcel = await ExcelService.ExportAsync(result, "MWO");
            return Ok(resultExcel);
        }
        [HttpGet("BudgetItemsApprovedExcel/{MWOId}")]
        public async Task<IActionResult> GetBudgetItems(Guid MWOId)
        {
            var result = await Mediator.Send(new GetBudgetItemsApprovedQuery(MWOId));
            var resultExcel = await ExcelService.ExportAsync(result, "MWO Approved");
            return Ok(resultExcel);
        }
        [HttpGet("MWOsCreated")]
        public async Task<IActionResult> GetMWOCreated()
        {
            var result = await Mediator.Send(new GetAllMWOCreatedQuery());
            var resultExcel = await ExcelService.ExportAsync(result, "MWOs Created");
            return Ok(resultExcel);
        }
        [HttpGet("MWOsApproved")]
        public async Task<IActionResult> GetMWOsApproved()
        {
            var result = await Mediator.Send(new GetAllMWOApprovedQuery());
            var resultExcel = await ExcelService.ExportAsync(result, "MWOs Approved");
            return Ok(resultExcel);
        }
        [HttpGet("MWOsClosed")]
        public async Task<IActionResult> GetMWOsClosed()
        {
            var result = await Mediator.Send(new GetAllMWOClosedQuery());
            var resultExcel = await ExcelService.ExportAsync(result, "MWOs Closed");
            return Ok(resultExcel);
        }
    }
}
