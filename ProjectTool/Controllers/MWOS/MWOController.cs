using Application.Features.MWOs.Commands;
using Application.Features.MWOs.Queries;
using Shared.Models.MWO;

namespace Server.Controllers.MWOS
{
    [ApiController]
    [Route("[controller]")]
    public class MWOController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public MWOController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost("createMWO")]
        public async Task<IActionResult> CreateMWO(CreateMWORequest request)
        {

            return Ok(await Mediator.Send(new CreateMWOCommand(request)));
        }


        [HttpPost("updateMWO")]
        public async Task<IActionResult> UpdateMWO(UpdateMWORequest request)
        {

            return Ok(await Mediator.Send(new UpdateMWOCommand(request)));
        }
        [HttpPost("approveMWO")]
        public async Task<IActionResult> ApproveMWO(ApproveMWORequest request)
        {

            return Ok(await Mediator.Send(new ApproveMWOCommand(request)));
        }
        [HttpPost("UnapproveMWO")]
        public async Task<IActionResult> UnApproveMWO(UnApproveMWORequest request)
        {

            return Ok(await Mediator.Send(new UnApproveMWOCommand(request)));
        }

        [HttpGet("GetMWOCreated/{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await Mediator.Send(new GetMWOCreatedByIdQuery(Id)));
        }
        [HttpGet("GetMWOToUpdate/{Id}")]
        public async Task<IActionResult> GetMWOToUpdateById(Guid Id)
        {
            return Ok(await Mediator.Send(new GetMWOToUpdateByIdQuery(Id)));
        }
        [HttpGet("GetMWOEBPReport/{Id}")]
        public async Task<IActionResult> GetMWOEBPReport(Guid Id)
        {
            return Ok(await Mediator.Send(new GetMWOEBPById(Id)));
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllMWOResponseQuery()));
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(MWOCreatedResponse request)
        {
            return Ok(await Mediator.Send(new DeleteMWOCommand(request)));
        }
        [HttpGet("GetMWOToApprove/{MWOId}")]
        public async Task<IActionResult> GetMWOToApprove(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetMWOToApproveQuery(MWOId)));
        }
        [HttpGet("GetMWOApproved/{MWOId}")]
        public async Task<IActionResult> GetMWOApproved(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetNewMWOApprovedById(MWOId)));
        }
    }
}
