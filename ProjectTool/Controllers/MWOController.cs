using Application.Features.MWOs;
using Application.Features.MWOs.Commands;
using Application.Features.MWOs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Filters;
using Shared.Models.MWO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MWOController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public MWOController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost("createMWO")]
        public async Task<IActionResult> CreateMWO(CreateMWORequest request)
        {

            return Ok( await Mediator.Send(new CreateMWOCommand(request)));
        }
        
        [HttpPost("updateMWOMinimal")]
        public async Task<IActionResult> UpdateMWOMinimal(UpdateMWOMinimalRequest request)
        {

            return Ok(await Mediator.Send(new UpdateMWOMinimalCommand(request)));
        }
        [HttpPost("updateMWO")]
        public async Task<IActionResult> UpdateMWO(UpdateMWORequest request)
        {

            return Ok(await Mediator.Send(new UpdateMWOCommand(request)));
        }
        [HttpPost("approveMWO")]
        public async Task<IActionResult> ApproveMWO(ApproveMWORequest request)
        {

            return Ok(await Mediator.Send(new ApproveMWOCommand(request)));
        }
        [HttpGet("CreateNameExist")]
        public async Task<IActionResult> ReviewIfNameExist(string name)
        {
            return Ok( await Mediator.Send(new MWOCreateNameExistQuery(name)));
        }
        [HttpPost("UpdateNameExist")]
        public async Task<IActionResult> ReviewIfNameExist(UpdateMWORequest request)
        {
            return Ok(await Mediator.Send(new MWOUpdateNameExistQuery(request)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await Mediator.Send(new GetByIdMWOQuery(Id)));
        }
       
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllMWOQuery()));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(MWOResponse request)
        {
            return Ok(await Mediator.Send(new DeleteMWOCommand(request)));
        }
        [HttpGet("GetMWOToApprove/{MWOId}")]
        public async Task<IActionResult> GetMWOToApprove(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetMWOToApproveQuery(MWOId)));
        }
    }
}
