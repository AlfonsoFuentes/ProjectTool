



using Application.NewFeatures.SoftwareVersions.Validators;

namespace Server.Controllers.Brands
{
    [ApiController]
    [Route("[controller]")]
    public class SoftwareVersionValidatorController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public SoftwareVersionValidatorController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("ValidateNameExist/{name}")]
        public async Task<IActionResult> ValidateName(string name)
        {
            return Ok(await Mediator.Send(new NewSoftwareVersionValidateNameQuery(name)));
        }
        [HttpGet("ValidateNameExist/{SoftwareVersionId}/{name}")]
        public async Task<IActionResult> ValidateName(Guid SoftwareVersionId,string name)
        {
            return Ok(await Mediator.Send(new NewSoftwareVersionValidateNameExistQuery(SoftwareVersionId, name)));
        }
    }
}
