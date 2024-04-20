using Application.Features.ChangeUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangeUserController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public ChangeUserController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost("ChangeUser")]
        public async Task<IActionResult> ChangeUser()
        {
            return Ok(await Mediator.Send(new ChangeDatabaseUserCommand()));
        }
        [HttpPost("UpdateDataForMWOs")]
        public async Task<IActionResult> UpdateDataForMWOs()
        {
            return Ok(await Mediator.Send(new UpdataForMWOSCommand()));
        }
        [HttpPost("UpdateTenant")]
        public async Task<IActionResult> UpdateTenat()
        {
            return Ok(await Mediator.Send(new UpdateTenantCommand()));
        }
    }
}
