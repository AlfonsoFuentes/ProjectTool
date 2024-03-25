using Application.Features.MWOs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("ValidateMWONumberExist/{mwonumber}")]
        public async Task<IActionResult> ValidateMWONumberExist(string mwonumber)
        {
            return Ok(await Mediator.Send(new ValidateMWONumberExist(mwonumber)));
        }
        [HttpGet("ValidateMWONameExist/{mwoname}")]
        public async Task<IActionResult> ValidateMWONameExist(string mwoname)
        {
            return Ok(await Mediator.Send(new ValidateMWONameExist(mwoname)));
        }
        [HttpGet("ValidateMWONameExist/{MWOId}/{mwoname}")]
        public async Task<IActionResult> ValidateMWONameExist(Guid MWOId, string mwoname)
        {
            return Ok(await Mediator.Send(new ValidateMWOExistingNameExist(MWOId, mwoname)));
        }
    }
}
