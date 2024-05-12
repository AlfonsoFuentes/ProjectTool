using Application.Features.SapAdjusts.Commands;
using Application.Features.SapAdjusts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.SapAdjust;

namespace Server.Controllers.SapAdjusts
{
    [ApiController]
    [Route("[controller]")]
    public class SapAdjustController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public SapAdjustController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Create))]
        public async Task<IActionResult> CreateSapAdjust(CreateSapAdjustRequest request)
        {
            return Ok(await Mediator.Send(new CreateSapAdjustCommand(request)));
        }

        [HttpPost(nameof(ClientEndPoint.Actions.Update))]
        public async Task<IActionResult> UpdateSapAdjust(UpdateSapAdjustRequest request)
        {
            return Ok(await Mediator.Send(new UpdateSapAdjustCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Delete))]
        public async Task<IActionResult> DeleteSapAdjust(SapAdjustResponse response)
        {
            return Ok(await Mediator.Send(new DeleteSapAdjustCommand(response)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSapAdjustbyId(Guid Id)
        {
            return Ok(await Mediator.Send(new GetSapAdjustByIdToUpdateQuery(Id)));
        }
        [HttpGet("GetByIdMWOApproved/{MWOId}")]
        public async Task<IActionResult> GetAllByMWO(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetSapAdjustByMWOIdQuery(MWOId)));
        }
        
    }
}
