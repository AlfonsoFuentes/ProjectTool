using Application.NewFeatures.BudgetItems.Commands;
using Application.NewFeatures.BudgetItems.Queries;
using Shared.NewModels.BudgetItems.Request;
using Shared.NewModels.BudgetItems.Responses;

namespace Server.Controllers.BudgetItems
{
    [ApiController]
    [Route("[controller]")]
    public class NewBudgetItemController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public NewBudgetItemController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Create))]
        public async Task<IActionResult> Create(NewBudgetItemMWOCreatedRequest request)
        {
            return Ok(await Mediator.Send(new NewBudgetItemCreateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Update))]
        public async Task<IActionResult> Update(NewBudgetItemMWOUpdateRequest request)
        {
            return Ok(await Mediator.Send(new NewBudgetItemUpdateCommand(request)));
        }
        [HttpPost(nameof(ClientEndPoint.Actions.Delete))]
        public async Task<IActionResult> Delete(NewBudgetItemMWOCreatedResponse request)
        {
            return Ok(await Mediator.Send(new NewBudgetItemDeleteCommand(request)));
        }
        

        [HttpGet("{nameof(ClientEndPoint.Actions.GetByIdToUpdate)}/{BudgetItemId}")]
        public async Task<IActionResult> GetById(Guid BudgetItemId)
        {
            return Ok(await Mediator.Send(new NewBudgetItemGetByIdToUpdateQuery(BudgetItemId)));
        }
        [HttpGet("GetAllMWOCreatedWithItems/{MWOId}")]
        public async Task<IActionResult> GetAllCreatedWithItems(Guid MWOId)
        {
            return Ok(await Mediator.Send(new NewMWOCreatedGetByIdWithItemsQuery(MWOId)));
        }
        [HttpGet("GetAllMWOApprovedWithItems/{MWOId}")]
        public async Task<IActionResult> GetAllApprovedWithItems(Guid MWOId)
        {
            return Ok(await Mediator.Send(new NewMWOApprovedGetByIdWithItemsQuery(MWOId)));
        }
        [HttpGet("GetMWOItemsToApplyTaxes/{MWOId}")]
        public async Task<IActionResult> GetMWOItemsToApplyTaxes(Guid MWOId)
        {
            return Ok(await Mediator.Send(new NewMWOCreatedGetByIdWithItemsQuery(MWOId)));
        }
        [HttpGet("GetByIdMWOApproved/{BudgetItemId}")]
        public async Task<IActionResult> GetByIdMWOApproved(Guid BudgetItemId)
        {
            return Ok(await Mediator.Send(new NewBudgetItemGetByIdMWOApprovedQuery(BudgetItemId)));
        }
        [HttpGet("GetAllApprovedForCreatePurchaseOrder/{MWOId}")]
        public async Task<IActionResult> GetAllApprovedForCreatePurchaseOrder(Guid MWOId)
        {
            return Ok(await Mediator.Send(new NewMWOApprovedForCreatePurchaseOrderGetByIdQuery(MWOId)));
        }
    }
}
