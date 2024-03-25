using Application.Features.PurchaseOrders.Queries;
using Application.Features.PurchaseorderValidators.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.PurchaseOrders
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseOrderValidatorController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public PurchaseOrderValidatorController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("ValidateNameExist/{name}")]
        public async Task<IActionResult> ValidateNameExistInPurchaseOrder(string name)
        {
            return Ok(await Mediator.Send(new ValidateNameExistPurchaseOrderQuery(name)));
        }
        [HttpGet("ValidatePurchaseRequisitionExist/{purchaserequisition}")]
        public async Task<IActionResult> ValidatePurchaseRequisitionExistInPurchaseOrder(string purchaserequisition)
        {
            return Ok(await Mediator.Send(new ValidatePurchaseRequisitionExistPurchaseOrder(purchaserequisition)));
        }
        [HttpGet("ValidateNameExist/{PurchaseOrderId}/{name}")]
        public async Task<IActionResult> ValidateNameExistInPurchaseOrder(string name, Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new ValidateNameExistPurchaseOrderCreatedQuery(PurchaseOrderId, name)));
        }
        [HttpGet("ValidatePurchaseRequisitionExist/{purchaserequisition}/{PurchaseOrderId}")]
        public async Task<IActionResult> ValidatePurchaseRequisitionExistInPurchaseOrder(string purchaserequisition, Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new ValidatePurchaseRequisitionExistPurchaseOrderCreated(PurchaseOrderId, purchaserequisition)));
        }
        [HttpGet("ValidatePONumberExist/{ponumber}/{PurchaseOrderId}")]
        public async Task<IActionResult> ValidatePONumberExistInPurchaseOrder(string ponumber, Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new ValidatePONumberExist(PurchaseOrderId, ponumber)));
        }
    }
}
