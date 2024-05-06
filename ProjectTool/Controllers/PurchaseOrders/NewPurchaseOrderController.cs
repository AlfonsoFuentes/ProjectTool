using Application.NewFeatures.PurchaseOrders.Commands;
using Application.NewFeatures.PurchaseOrders.Queries;
using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Server.Controllers.PurchaseOrders
{
    [ApiController]
    [Route("[controller]")]
    public class NewPurchaseOrderController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public NewPurchaseOrderController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Create))]
        public async Task<IActionResult> Create(NewPurchaseOrderCreateRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderCreateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Approve))]
        public async Task<IActionResult> Approve(NewPurchaseOrderApproveRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderApproveCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.UnApprove))]
        public async Task<IActionResult> UnApprove(NewPurchaseOrderUnApproveRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderUnApproveCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.ReOpen))]
        public async Task<IActionResult> ReOpen(NewPurchaseOrderReOpenRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderReOpenCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Delete))]
        public async Task<IActionResult> Delete(NewPurchaseOrderDeleteRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderDeleteCommand(request)));
        }
        [HttpGet(nameof(ClientEndPoint.Actions.GetAllApproved))]
        public async Task<IActionResult> GetAllApproved()
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetAllApprovedQuery()));
        }
        [HttpGet(nameof(ClientEndPoint.Actions.GetAllCreated))]
        public async Task<IActionResult> GetAllCreated()
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetAllCreatedQuery()));
        }
        [HttpGet(nameof(ClientEndPoint.Actions.GetAllClosed))]
        public async Task<IActionResult> GetAllClosed()
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetAllClosedQuery()));
        }
        [HttpGet("GetPurchaseOrderToApprove/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderToApprove(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetByIdToApprovedQuery(PurchaseOrderId)));
        }
    }
}
