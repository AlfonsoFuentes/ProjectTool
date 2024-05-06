using Application.NewFeatures.SoftwareVersions.Commands;
using Shared.Models.UserManagements;
using Shared.NewModels.SoftwareVersion;

namespace Server.Controllers.SoftwareVersions
{
    [ApiController]
    [Route("[controller]")]
    public class SoftwareVersionController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public SoftwareVersionController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet(nameof(ClientEndPoint.Actions.GetAll))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new NewSoftwareVersionGetAllQuery()));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Create))]
        public async Task<IActionResult> Create(NewSoftwareVersionCreateRequest request)
        {
            return Ok(await Mediator.Send(new NewSoftwareVersionCreateCommand(request)));
        }
        [HttpPost("CheckVersionForUser")]
        public async Task<IActionResult> CheckVersionForUser(AuthResponseDto request)
        {
            return Ok(await Mediator.Send(new NewCheckVersionForUserCommand(request.UserId)));
        }
    }
}
