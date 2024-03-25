using Application.Features.BudgetItems.Queries;
using Application.Features.MWOs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.BudgetItems
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetItemValidatorController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public BudgetItemValidatorController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("ValidateNameExist/{MWOId}/{name}")]
        public async Task<IActionResult> ValidateNameExist(Guid MWOId, string name)
        {
            return Ok(await Mediator.Send(new ValidateBudgetItemNameExist(MWOId, name)));
        }
        [HttpGet("ValidateNameExist/{MWOId}/{BudgetItemId}/{name}")]
        public async Task<IActionResult> ValidateMWONumberExist(Guid MWOId, Guid BudgetItemId, string name)
        {
            return Ok(await Mediator.Send(new ValidateBudgetItemExistingNameExist(BudgetItemId, MWOId, name)));
        }
    }
}
