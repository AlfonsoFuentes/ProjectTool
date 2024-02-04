using Application.Features.BudgetItems.Command;
using Application.Features.BudgetItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.BudgetItems;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetItemController:ControllerBase
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
        [HttpGet("GetAllItems/{MWOId}")]
        public async Task<IActionResult> GetAllByMWOId(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetAllBudgetItemQuery(MWOId)));
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
        [HttpGet("SumPercEngContItems/{MWOId}")]
        public async Task<IActionResult> GetSumPercEngCont(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetSumPercentageEngContQuery(MWOId)));
        }
        [HttpGet("SumBudgetItems/{MWOId}")]
        public async Task<IActionResult> GetSumBudget(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetSumBudgetItemsQuery(MWOId)));
        }
        [HttpGet("SumTaxesBudget/{BudgetItemId}")]
        public async Task<IActionResult> GetSumTaxesBudget(Guid BudgetItemId)
        {
            return Ok(await Mediator.Send(new GetSumBugetTaxesItemsQuery(BudgetItemId)));
        }
    }
}
