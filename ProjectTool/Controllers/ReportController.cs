namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IMediator Mediator { get; set; }

        public ReportController(IMediator mediator)
        {
            Mediator = mediator;
        }
        [HttpGet("GetEBPReport/{MWOId}")]
        public async Task<IActionResult> GetEBPReport(Guid MWOId)
        {
            return Ok(await Mediator.Send(new NewMWOEBPReportQuery(MWOId)));
        }
    }
}
