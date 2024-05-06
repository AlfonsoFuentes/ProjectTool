

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("[controller]")]
    public class NewSupplierController : ControllerBase
    {
        private IMediator Mediator { get; set; }
        private IExcelService ExcelService { get; set; }
        public NewSupplierController(IMediator mediator, IExcelService excelService)
        {
            Mediator = mediator;
            ExcelService = excelService;
        }

        [HttpPost(nameof(ClientEndPoint.Actions.Create))]
        public async Task<IActionResult> Create(NewSupplierCreateRequest request)
        {
            return Ok(await Mediator.Send(new NewSupplierCreateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Update))]
        public async Task<IActionResult> Update(NewSupplierUpdateRequest request)
        {
            return Ok(await Mediator.Send(new NewSupplierUpdateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Delete))]
        public async Task<IActionResult> Delete(NewSupplierResponse response)
        {
            return Ok(await Mediator.Send(new NewSupplierDeleteCommand(response)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.CreateAndReponse))]
        public async Task<IActionResult> CreateAndReponse(NewSupplierCreateBasicRequest request)
        {
            return Ok(await Mediator.Send(new NewSupplierCreateAndResponseCommand(request)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await Mediator.Send(new NewSupplierGetByIdToUpdateQuery(Id)));
        }

        [HttpGet(nameof(ClientEndPoint.Actions.GetAll))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new NewSupplierGetAllQuery()));
        }
        [HttpGet(nameof(ClientEndPoint.Actions.ExportToExcel))]
        public async Task<IActionResult> GetSuppliers()
        {
            var result = await Mediator.Send(new NewSupplierExportFileQuery());
            if(result.Succeeded)
            {
                var resultExcel = await ExcelService.ExportAsync(result.Data.Suppliers, "Supplier List");
                return Ok(resultExcel);
            }

            return Ok(Result<FileResult>.Fail(result.Message));
        }
    }
}
