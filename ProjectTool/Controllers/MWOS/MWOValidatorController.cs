

namespace Server.Controllers.MWOS
{
    [ApiController]
    [Route("[controller]")]
    public class MWOValidatorController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public MWOValidatorController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("ValidateMWONumberExist/{MWOId}/{mwonumber}")]
        public async Task<IActionResult> ValidateMWONumberExist(Guid MWOId, string mwonumber)
        {
            return Ok(await Mediator.Send(new NewMWOValidateNumberExistQuery(MWOId,mwonumber)));
        }
        [HttpGet("ValidateMWONameExist/{mwoname}")]
        public async Task<IActionResult> ValidateMWONameExist(string mwoname)
        {
            return Ok(await Mediator.Send(new NewMWOValidateNameQuery(mwoname)));
        }
        [HttpGet("ValidateMWONameExist/{MWOId}/{mwoname}")]
        public async Task<IActionResult> ValidateMWONameExist(Guid MWOId, string mwoname)
        {
            return Ok(await Mediator.Send(new NewMWOValidateNameExistQuery(MWOId, mwoname)));
        }
    }
}
