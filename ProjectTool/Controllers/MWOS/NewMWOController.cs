

namespace Server.Controllers.MWOS
{
    [ApiController]
    [Route("[controller]")]
    public class NewMWOController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public NewMWOController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Create))]
        public async Task<IActionResult> Create(NewMWOCreateRequest request)
        {
            return Ok(await Mediator.Send(new NewMWOCreateCommand(request)));
        }
        
        [HttpGet(nameof(ClientEndPoint.Actions.GetAllCreated))]
        public async Task<IActionResult> GetAllCreated()
        {
            return Ok(await Mediator.Send(new NewMWOGetAllCreatedQuery()));
        }
        [HttpGet(nameof(ClientEndPoint.Actions.GetAllApproved))]
        public async Task<IActionResult> GetAllApproved()
        {
            return Ok(await Mediator.Send(new NewMWOGetAllApprovedQuery()));
        }
       
        [HttpPost(nameof(ClientEndPoint.Actions.Delete))]
        public async Task<IActionResult> Delete(NewMWODeleteRequest response)
        {
            return Ok(await Mediator.Send(new NewMWODeleteCommand(response)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Update))]
        public async Task<IActionResult> Update(NewMWOUpdateRequest request)
        {
            return Ok(await Mediator.Send(new NewMWOUpdateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.UnApprove))]
        public async Task<IActionResult> UnApprove(NewMWOUnApproveRequest request)
        {
            return Ok(await Mediator.Send(new NewMWOUnApproveCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Approve))]
        public async Task<IActionResult> Approve(NewMWOApproveRequest request)
        {
            return Ok(await Mediator.Send(new NewMWOApproveCommand(request)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await Mediator.Send(new NewMWOGetByIdToUpdateQuery(Id)));
        }
        [HttpGet("GetToApproved/{Id}")]
        public async Task<IActionResult> GetToApproveById(Guid Id)
        {
            return Ok(await Mediator.Send(new NewMWOGetByIdToApproveQuery(Id)));
        }
        [HttpGet("GetEBPReport/{MWOId}")]
        public async Task<IActionResult> GetEBPReport(Guid MWOId)
        {
            return Ok(await Mediator.Send(new NewMWOEBPReportQuery(MWOId)));
        }

    }
   
}
