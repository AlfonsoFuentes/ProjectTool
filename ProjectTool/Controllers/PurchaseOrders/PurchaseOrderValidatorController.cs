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
        [HttpGet("ValidateNameExist/{MWOId}/{name}")]
        public async Task<IActionResult> ValidateNameExistInPurchaseOrder(Guid MWOId,string name)
        {
            return Ok(await Mediator.Send(new ValidateNameExistPurchaseOrderQuery(MWOId,name)));
        }
        [HttpGet("ValidatePurchaseRequisitionExist/{purchaserequisition}")]
        public async Task<IActionResult> ValidatePurchaseRequisitionExistInPurchaseOrder(string purchaserequisition)
        {
            return Ok(await Mediator.Send(new ValidatePurchaseRequisitionExistPurchaseOrder(purchaserequisition)));
        }
        [HttpGet("ValidateNameExist/{MWOId}/{PurchaseOrderId}/{name}")]
        public async Task<IActionResult> ValidateNameExistInPurchaseOrder(Guid MWOId, Guid PurchaseOrderId, string name)
        {
            return Ok(await Mediator.Send(new ValidateNameExistPurchaseOrderCreatedQuery(MWOId,PurchaseOrderId, name)));
        }
        [HttpGet("ValidatePurchaseRequisitionExist/{PurchaseOrderId}/{purchaserequisition}")]
        public async Task<IActionResult> ValidatePurchaseRequisitionExistInPurchaseOrder(string purchaserequisition, Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new ValidatePurchaseRequisitionExistPurchaseOrderCreated(PurchaseOrderId, purchaserequisition)));
        }
        [HttpGet("ValidatePONumberExist/{PurchaseOrderId}/{ponumber}")]
        public async Task<IActionResult> ValidatePONumberExistInPurchaseOrder(string ponumber, Guid PurchaseOrderId)
        {
            return Ok(await Mediator.Send(new ValidatePONumberExist(PurchaseOrderId, ponumber)));
        }
        [HttpGet("ValidatePONumberExist/{ponumber}")]
        public async Task<IActionResult> ValidatePONumberExistInPurchaseOrder(string ponumber)
        {
            return Ok(await Mediator.Send(new ValidateNewPONumberExist(ponumber)));
        }
    }
}
