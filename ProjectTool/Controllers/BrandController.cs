﻿using Application.Features.Brands.Command;
using Application.Features.Brands.Queries;
using Application.Features.MWOs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Brands;
using Shared.Models.MWO;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController: ControllerBase
    {
        private IMediator Mediator { get; set; }

        public BrandController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost("CreateBrand")]
        public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
        {
            return Ok(await Mediator.Send(new CreateBrandCommand(request)));
        }
        [HttpPost("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand(UpdateBrandRequest request)
        {
            return Ok(await Mediator.Send(new UpdateBrandCommand(request)));
        }
        [HttpPost("DeleteBrand")]
        public async Task<IActionResult> DeleteBrand(BrandResponse response)
        {
            return Ok(await Mediator.Send(new DeleteBrandCommand(response)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBrandbyId(Guid Id)
        {
            return Ok(await Mediator.Send(new GetByIdBrandQuery(Id)));  
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllBrands()
        {
            return Ok(await Mediator.Send(new GetAllBrandQuery()));
        }
        [HttpGet("CreateNameExist")]
        public async Task<IActionResult> ReviewIfNameExist(string name)
        {
            return Ok(await Mediator.Send(new BrandCreateNameExistQuery(name)));
        }
        [HttpPost("UpdateNameExist")]
        public async Task<IActionResult> ReviewIfNameExist(UpdateBrandRequest request)
        {
            return Ok(await Mediator.Send(new BrandUpdateNameExistQuery(request)));
        }
    }
}