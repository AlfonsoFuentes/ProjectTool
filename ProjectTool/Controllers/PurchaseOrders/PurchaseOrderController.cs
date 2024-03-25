using Application.Features.PurchaseOrders.Commands;
using Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Creates;
using Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Edits;
using Application.Features.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Server.Controllers.PurchaseOrders
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
        public async Task<IActionResult> ReceivePurchaseOrder(ReceiveRegularPurchaseOrderRequestDto request)
        {
            return Ok(await Mediator.Send(new ReceivePurchaseOrderCommand(request)));
        }

        [HttpPost("CreateRegularPurchaseOrder")]
        public async Task<IActionResult> CreatePurchaseOrder(CreatedRegularPurchaseOrderRequestDto request)
        {
            return Ok(await Mediator.Send(new CreateRegularPurchaseOrderCommand(request)));
        }
        [HttpPost("CreatePurchaseOrderCapitalizedSalary")]
        public async Task<IActionResult> CreatePurchaseOrderCapitalizedSalary(CreateCapitalizedSalaryPurchaseOrderRequestDto request)
        {
            return Ok(await Mediator.Send(new CreatePurchaseOrderCapitalizedSalaryCommand(request)));
        }
        [HttpPost("CreateTaxPurchaseOrder")]
        public async Task<IActionResult> CreateTaxPurchaseOrder(CreateTaxPurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new CreateTaxPurchaseOrderCommand(request)));
        }
        [HttpPost("ApproveRegularPurchaseOrder")]
        public async Task<IActionResult> ApprovePurchaseOrder(ApprovedRegularPurchaseOrderRequestDto request)
        {
            return Ok(await Mediator.Send(new ApproveRegularPurchaseOrderCommand(request)));

        }
        [HttpPost("ApprovePurchaseOrderForAlteration")]
        public async Task<IActionResult> ApprovePurchaseOrderForAlteration(ApprovedRegularPurchaseOrderRequestDto request)
        {
            return Ok(await Mediator.Send(new ApprovePurchaseOrderForAlterationCommand(request)));

        }
        [HttpPost("EditPurchaseOrderCreated")]
        public async Task<IActionResult> EditPurchaseOrderCreated(EditPurchaseOrderRegularCreatedRequestDto request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderCreatedCommand(request)));
        }
        //[HttpPost("UpdatePurchaseOrderMinimal")]
        //public async Task<IActionResult> UpdatePurchaseOrderMinimal(UpdatePurchaseOrderMinimalRequest request)
        //{
        //    return Ok(await Mediator.Send(new UpdatePurchaseOrderMinimalCommand(request)));
        //}
        [HttpPost("EditPurchaseOrderApproved")]
        public async Task<IActionResult> EditPurchaseOrderApproved(EditPurchaseOrderRegularApprovedRequestDto request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderApprovedCommand(request)));
        }
        [HttpPost("EditPurchaseOrderClosed")]
        public async Task<IActionResult> EditPurchaseOrderClosed(EditPurchaseOrderRegularClosedRequestDto request)
        {

            return Ok(await Mediator.Send(new EditPurchaseOrderClosedCommand(request)));
        }
        [HttpPost("EditPurchaseOrderTax")]
        public async Task<IActionResult> EditPurchaseOrderTax(EditTaxPurchaseOrderRequest request)
        {
            return Ok(await Mediator.Send(new EditTaxPurchaseOrderCommand(request)));
        }
        [HttpPost("EditPurchaseOrderCapitalizedSalary")]
        public async Task<IActionResult> EditPurchaseOrderCapitalizedSalary(EditCapitalizedSalaryPurchaseOrderRequestDto request)
        {
            return Ok(await Mediator.Send(new EditPurchaseOrderCapitalizedSalaryCommand(request)));
        }
        [HttpGet("GetBudgetItemsToCreatePurchaseOrder/{BudgetItemId}")]
        public async Task<IActionResult> GetBudgetItemsToCreatePurchaseOrder(Guid BudgetItemId)
        {
            return Ok(await Mediator.Send(new GetBudgetItemsForPurchaseOrderQuery(BudgetItemId)));
        }
        [HttpGet("GetPurchaseOrderCreatedToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetEditPurchaseOrder(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderCreatedToEditById(PurchaseOrderId)));
        }
        [HttpGet("GetPurchaseOrderApprovedToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderApprovedToEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderApprovedToEditById(PurchaseOrderId)));
        }
        [HttpGet("GetPurchaseOrderClosedToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderClosedToEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderClosedToEditById(PurchaseOrderId)));
        }
        [HttpGet("GetReceivePurchaseOrder/{PurchaseOrderId}")]
        public async Task<IActionResult> GetReceivePurchaseOrder(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderToReceiveById(PurchaseOrderId)));
        }
        [HttpGet("GetPurchaseOrderToApprove/{PurchaseOrderId}")]
        public async Task<IActionResult> GetApprovePurchaseOrder(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderToApproveById(PurchaseOrderId)));
        }
        [HttpGet("GetPurchaseOrderTaxToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderTaxToEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderTaxesToEditById(PurchaseOrderId)));
        }
        [HttpGet("GetPurchaseOrderCapitalizedSalaryToEdit/{PurchaseOrderId}")]
        public async Task<IActionResult> GetPurchaseOrderCapitalizedSalaryEdit(Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new GetPurchaseOrderCapitalizedSalaryToEditById(PurchaseOrderId)));
        }
        [HttpGet("GetAllPurchaseOrder")]
        public async Task<IActionResult> GetAllPurchaseorder()
        {
            return Ok(await Mediator.Send(new GetAllPurchaseOrderQuery()));
        }

    }
}
