using Application.Features.PurchaseOrders.Commands;
using Application.Features.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.PurchaseOrders.Requests.Receives;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public PurchaseOrderController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost("ReceivePurchaseOrder")]
        public async Task<IActionResult> ReceivePurchaseOrder(ReceivePurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new ReceivePurchaseOrderCommand(request)));
        }
        [HttpPost("CreatePurchaseOrder")]
        public async Task<IActionResult> CreatePurchaseOrder(CreatePurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new CreatePurchaseOrderCommand(request)));
        }
        [HttpPost("ApprovePurchaseOrder")]
        public async Task<IActionResult> ApprovePurchaseOrder(ApprovePurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new ApprovePurchaseOrderCommand(request)));
        }
        [HttpPost("EditPurchaseOrderCreated")]
        public async Task<IActionResult> EditPurchaseOrderCreated(EditPurchaseOrderCreatedRequest request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderCreatedCommand(request)));
        }
        [HttpPost("EditPurchaseOrderApproved")]
        public async Task<IActionResult> EditPurchaseOrderApproved(ApprovePurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderApproovedCommand(request)));
        }
        [HttpGet("GetCreatePurchaseOrder/{BudgetItemId}")]
        public async Task<IActionResult> GetCreatePurchaseOrder(Guid BudgetItemId)
        {
            return Ok(await Mediator.Send(new GetDataForCreatePurchaseOrderQuery(BudgetItemId)));
        }
        [HttpGet("GetDataForEditPurchaseOrder/{PurchaseOrderId}")]
        public async Task<IActionResult> GetEditPurchaseOrder(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetDataForEditPurchaseOrderQuery(PurchaseOrderId)));
        }
        [HttpGet("GetReceivePurchaseOrder/{PurchaseOrderId}")]
        public async Task<IActionResult> GetReceivePurchaseOrder(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderToReceiveById(PurchaseOrderId)));
        }
        [HttpGet("GetApprovePurchaseOrder/{PurchaseOrderId}")]
        public async Task<IActionResult> GetApprovePurchaseOrder(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderToApproveById(PurchaseOrderId)));
        }
        [HttpGet("GetAllPurchaseOrder")]
        public async Task<IActionResult> GetAllPurchaseorder()
        {
            return Ok(await Mediator.Send(new GetAllPurchaseOrderQuery()));
        }
        
    }
}
