

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierValidatorController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public SupplierValidatorController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("ValidateNameExist/{Name}")]
        public async Task<IActionResult> ReviewIfNameExist(string Name)
        {
            return Ok(await Mediator.Send(new NewSupplierValidateNameQuery(Name)));
        }
        [HttpGet("ValidateNameExist/{SupplierId}/{Name}")]
        public async Task<IActionResult> ReviewIfNameExist(Guid SupplierId, string Name)
        {
            return Ok(await Mediator.Send(new NewSupplierValidateNameExistQuery(SupplierId,Name)));
        }
        [HttpGet("ValidateVendorCodeExist/{VendorCode}")]
        public async Task<IActionResult> ValidateVendorCodeExist(string VendorCode)
        {
            return Ok(await Mediator.Send(new NewSupplierValidateVendorCodeQuery(VendorCode)));
        }
        [HttpGet("ValidateVendorCodeExist/{SupplierId}/{VendorCode}")]
        public async Task<IActionResult> ValidateVendorCodeExist(Guid SupplierId,string VendorCode)
        {
            return Ok(await Mediator.Send(new NewSupplierValidateVendorCodeExistQuery(SupplierId,VendorCode)));
        }
        [HttpGet("ValidateEmailExist/{Email}")]
        public async Task<IActionResult> ValidateEmailExist(string Email)
        {
            return Ok(await Mediator.Send(new NewSupplierValidateEmailQuery(Email)));
        }
        [HttpGet("ValidateEmailExist/{SupplierId}/{Email}")]
        public async Task<IActionResult> ValidateEmailExist(Guid SupplierId, string Email)
        {
            return Ok(await Mediator.Send(new NewSupplierValidateExistingEmailExistQuery(SupplierId,Email)));
        }
    }
}
