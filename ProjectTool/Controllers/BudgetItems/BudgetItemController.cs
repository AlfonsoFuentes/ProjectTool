using Application.Features.BudgetItems.Command;
using Application.Features.BudgetItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.BudgetItems;

namespace Server.Controllers.BudgetItems
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetItemController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public BudgetItemController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost("CreateRegularItem")]
        public async Task<IActionResult> CreateRegularBudgetItem(CreateBudgetItemRequest request)
        {

            return Ok(await Mediator.Send(new CreateRegularBudgetItemCommand(request)));
        }
        [HttpPost("CreateEquipmentItem")]
        public async Task<IActionResult> CreateEquipmentItem(CreateBudgetItemRequest request)
        {

            return Ok(await Mediator.Send(new CreateEquipmentInstrumentsItemCommand(request)));
        }
        [HttpPost("CreateEngContItem")]
        public async Task<IActionResult> CreateEngContItem(CreateBudgetItemRequest request)
        {
            return Ok(await Mediator.Send(new CreateEngContingencyCommand(request)));
        }
        [HttpPost("CreateTaxItem")]
        public async Task<IActionResult> CreateTaxtItem(CreateBudgetItemRequest request)
        {
            return Ok(await Mediator.Send(new CreateTaxItemCommand(request)));
        }
        [HttpPost("CreateAlterationItem")]
        public async Task<IActionResult> CreateAlterationItem(CreateBudgetItemRequest request)
        {
            return Ok(await Mediator.Send(new CreateAlterationBudgetItemCommand(request)));
        }
        [HttpGet("GetAllItems/{MWOId}")]
        public async Task<IActionResult> GetAllByMWOId(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetAllBudgetItemQuery(MWOId)));
        }
        [HttpGet("GetAllApprovedItems/{MWOId}")]
        public async Task<IActionResult> GetAllApprovedByMWOId(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetAllBudgetItemApprovedQuery(MWOId)));
        }
        [HttpGet("GetApprovedItemById/{BudgetItemId}")]
        public async Task<IActionResult> GetApprovedItemById(Guid BudgetItemId)
        {
            return Ok(await Mediator.Send(new GetApprovedBudgetItemQuery(BudgetItemId)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBudgetmById(Guid Id)
        {
            return Ok(await Mediator.Send(new GetByIdBudgetItemQuery(Id)));
        }
        [HttpPost("DeleteBudgetItem")]
        public async Task<IActionResult> Delete(BudgetItemResponse response)
        {
            return Ok(await Mediator.Send(new DeleteBudgetItemCommand(response)));
        }
        
        [HttpGet("GetDataForCreateBudget/{MWOId}")]
        public async Task<IActionResult> GetDataForCreateBudget(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetDataForCreateBudgetItemQuery(MWOId)));
        }
        [HttpPost("UpdateRegularItem")]
        public async Task<IActionResult> UpdateRegularBudgetItem(UpdateBudgetItemRequest request)
        {

            return Ok(await Mediator.Send(new UpdateRegularBudgetItemCommand(request)));
        }
       
        [HttpPost("UpdateEquipmentItem")]
        public async Task<IActionResult> UpdateEquipmentItem(UpdateBudgetItemRequest request)
        {

            return Ok(await Mediator.Send(new UpdateEquipmentInstrumentsItemCommand(request)));
        }
        [HttpPost("UpdateEngContItem")]
        public async Task<IActionResult> UpdateEngContItem(UpdateBudgetItemRequest request)
        {
            return Ok(await Mediator.Send(new UpdateEngContingencyCommand(request)));
        }
        [HttpPost("UpdateTaxItem")]
        public async Task<IActionResult> UpdateTaxtItem(UpdateBudgetItemRequest request)
        {
            return Ok(await Mediator.Send(new UpdateTaxItemCommand(request)));
        }
        [HttpPost("UpdateAlterationItem")]
        public async Task<IActionResult> UpdateAlterationItem(UpdateBudgetItemRequest request)
        {
            return Ok(await Mediator.Send(new UpdateAlterationBudgetItemCommand(request)));
        }
    }
}
