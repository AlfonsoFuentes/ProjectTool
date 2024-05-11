using Application.Features.PurchaseOrders.Queries;
using Application.NewFeatures.PurchaseOrders.Commands;
using Application.NewFeatures.PurchaseOrders.NewCommands;
using Application.NewFeatures.PurchaseOrders.NewQueries;
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
        [HttpPost(nameof(ClientEndPoint.Actions.CreateSalary))]
        public async Task<IActionResult> CreateSalary(NewPurchaseOrderCreateSalaryRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderCreateSalaryCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.EditCreate))]
        public async Task<IActionResult> EditCreate(NewPurchaseOrderEditCreateRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderEditCreateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.EditApproved))]
        public async Task<IActionResult> EditApproved(NewPurchaseOrderEditApproveRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderEditApproveCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Approve))]
        public async Task<IActionResult> Approve(NewPurchaseOrderApproveRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderApproveCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Receive))]
        public async Task<IActionResult> Receive(NewPurchaseOrderReceiveRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderReceivingCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.EditReceive))]
        public async Task<IActionResult> EditReceive(NewPurchaseOrderEditReceiveRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderEditReceivingCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.EditSalary))]
        public async Task<IActionResult> EditSalary(NewPurchaseOrderEditSalaryRequest request)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderEditSalaryCommand(request)));
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
        [HttpGet("GetToApprove/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderToApprove(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetByIdToApprovedQuery(PurchaseOrderId)));
        }
        [HttpGet("GetCreatedToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderCreatedToEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetByIdToEditCreatedQuery(PurchaseOrderId)));
        }
        [HttpGet("GetApprovedToReceive/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderApprovedToReceive(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetByIdToReceiveQuery(PurchaseOrderId)));
        }
        [HttpGet("GetReceivedToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderReceivedToEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetByIdToEditReceiveQuery(PurchaseOrderId)));
        }
        [HttpGet("GetApprovedToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderApprovedToEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetByIdToEditApprovedQuery(PurchaseOrderId)));
        }
        [HttpGet("GetSalaryToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderSalaryToEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new NewPurchaseOrderGetByIdToEditSalaryQuery(PurchaseOrderId)));
        }

    }
}
