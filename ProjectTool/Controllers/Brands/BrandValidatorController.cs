using Application.Features.Brands.Queries;
using Application.Features.PurchaseorderValidators.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Brands
{
    [ApiController]
    [Route("[controller]")]
    public class BrandValidatorController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public BrandValidatorController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("ValidateNameExist/{name}")]
        public async Task<IActionResult> ValidateNameExistInPurchaseOrder(string name)
        {
            return Ok(await Mediator.Send(new ValidateBrandNameExistQuery(name)));
        }
        [HttpGet("ValidateNameExist/{BrandId}/{name}")]
        public async Task<IActionResult> ValidateNameExistInPurchaseOrder(Guid BrandId,string name)
        {
            return Ok(await Mediator.Send(new ValidateExistingBrandNameExistQuery(BrandId, name)));
        }
    }
}
