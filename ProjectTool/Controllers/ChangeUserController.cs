using Application.Features.Brands.Command;
using Application.Features.ChangeUser;
using Azure.Core;
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
    }
}
