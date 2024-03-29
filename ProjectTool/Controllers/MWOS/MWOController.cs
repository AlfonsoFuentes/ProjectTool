﻿using Application.Features.MWOs;
using Application.Features.MWOs.Commands;
using Application.Features.MWOs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Filters;
using Shared.Models.MWO;

namespace Server.Controllers.MWOS
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
        public async Task<IActionResult> CreateMWO(CreateMWORequestDto request)
        {

            return Ok(await Mediator.Send(new CreateMWOCommand(request)));
        }

       
        [HttpPost("updateMWO")]
        public async Task<IActionResult> UpdateMWO(UpdateMWORequestDto request)
        {

            return Ok(await Mediator.Send(new UpdateMWOCommand(request)));
        }
        [HttpPost("approveMWO")]
        public async Task<IActionResult> ApproveMWO(ApproveMWORequestDto request)
        {

            return Ok(await Mediator.Send(new ApproveMWOCommand(request)));
        }
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await Mediator.Send(new GetMWOByIdQuery(Id)));
        }
        [HttpGet("GetMWOToUpdate/{Id}")]
        public async Task<IActionResult> GetMWOToUpdateById(Guid Id)
        {
            return Ok(await Mediator.Send(new GetMWOToUpdateByIdQuery(Id)));
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
        [HttpGet("GetMWOApproved/{MWOId}")]
        public async Task<IActionResult> GetMWOApproved(Guid MWOId)
        {
            return Ok(await Mediator.Send(new GetMWOApprovedById(MWOId)));
        }
    }
}
