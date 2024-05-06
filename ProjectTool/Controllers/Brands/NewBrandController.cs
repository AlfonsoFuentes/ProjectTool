

namespace Server.Controllers.Brands
{
    [Route("[controller]")]
    [ApiController]
    public class NewBrandController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public NewBrandController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Create))]
        public async Task<IActionResult> Create(NewBrandCreateRequest request)
        {
            return Ok(await Mediator.Send(new NewBrandCreateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Update))]
        public async Task<IActionResult> Update(NewBrandUpdateRequest request)
        {
            return Ok(await Mediator.Send(new NewBrandUpdateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Delete))]
        public async Task<IActionResult> Delete(NewBrandResponse response)
        {
            return Ok(await Mediator.Send(new NewBrandDeleteCommand(response)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.CreateAndReponse))]
        public async Task<IActionResult> CreateAndReponse(NewBrandCreateRequest request)
        {
            return Ok(await Mediator.Send(new NewBrandCreateAndResponseCommand(request)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await Mediator.Send(new NewBrandGetByIdToUpdateQuery(Id)));
        }
        
        [HttpGet(nameof(ClientEndPoint.Actions.GetAll))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new NewBrandGetAllQuery()));
        }
    }
}
