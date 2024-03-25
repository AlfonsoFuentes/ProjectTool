using Application.Features.Suppliers.Command;
using Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public SupplierController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpPost("CreateSupplier")]
        public async Task<IActionResult> CreateSupplier(CreateSupplierRequestDto request)
        {
            return Ok(await Mediator.Send(new CreateSupplierCommand(request)));
        }
        [HttpPost("CreateSupplierForPurchaseorder")]
        public async Task<IActionResult> CreateSupplierForPurchaseorder(CreateSupplierRequestDto request)
        {
            return Ok(await Mediator.Send(new CreateSupplierForPurchaseorderCommand(request)));
        }
        [HttpPost("UpdateSupplier")]
        public async Task<IActionResult> UpdateSupplier(UpdateSupplierRequestDto request)
        {
            return Ok(await Mediator.Send(new UpdateSupplierCommand(request)));
        }
        [HttpPost("DeleteSupplier")]
        public async Task<IActionResult> DeleteSupplier(SupplierResponse response)
        {
            return Ok(await Mediator.Send(new DeleteSupplierCommand(response)));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSupplierbyId(Guid Id)
        {
            return Ok(await Mediator.Send(new GetByIdSupplierQuery(Id)));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            return Ok(await Mediator.Send(new GetAllSupplierQuery()));
        }
       


    }
}
