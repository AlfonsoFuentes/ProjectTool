using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Filters;

namespace Server.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [Route("[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
