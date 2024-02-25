using Application.Features.PurchaseOrders.Commands;
using Application.Features.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.Closeds;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.PurchaseOrders.Requests.Receives;
using Shared.Models.PurchaseOrders.Requests.Taxes;

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
        [HttpPost("CreatePurchaseOrderCapitalizedSalary")]
        public async Task<IActionResult> CreatePurchaseOrderCapitalizedSalary(CreateCapitalizedSalaryPurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new CreatePurchaseOrderCapitalizedSalaryCommand(request)));
        }
        [HttpPost("CreateTaxPurchaseOrder")]
        public async Task<IActionResult> CreateTaxPurchaseOrder(CreateTaxPurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new CreateTaxPurchaseOrderCommand(request)));
        }
        [HttpPost("ApprovePurchaseOrder")]
        public async Task<IActionResult> ApprovePurchaseOrder(ApprovePurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new ApprovePurchaseOrderCommand(request)));

        }
        [HttpPost("ApprovePurchaseOrderForAlteration")]
        public async Task<IActionResult> ApprovePurchaseOrderForAlteration(ApprovePurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new ApprovePurchaseOrderForAlterationCommand(request)));

        }
        [HttpPost("EditPurchaseOrderCreated")]
        public async Task<IActionResult> EditPurchaseOrderCreated(EditPurchaseOrderCreatedRequest request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderCreatedCommand(request)));
        }
        [HttpPost("EditPurchaseOrderApproved")]
        public async Task<IActionResult> EditPurchaseOrderApproved(ApprovePurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderApprovedCommand(request)));
        }
        [HttpPost("EditPurchaseOrderClosed")]
        public async Task<IActionResult> EditPurchaseOrderClosed(ClosedPurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderClosedCommand(request)));
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
        [HttpGet("GetClosedPurchaseOrder/{PurchaseOrderId}")]
        public async Task<IActionResult> GetClosedPurchaseOrder(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderClosedById(PurchaseOrderId)));
        }
        [HttpGet("GetAllPurchaseOrder")]
        public async Task<IActionResult> GetAllPurchaseorder()
        {
            return Ok(await Mediator.Send(new GetAllPurchaseOrderQuery()));
        }
        
    }
}
