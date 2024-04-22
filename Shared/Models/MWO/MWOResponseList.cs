namespace Shared.Models.MWO
{
    public class MWOResponseList
    {
        public IEnumerable<MWOCreatedResponse> MWOsCreated { get; set; } = null!;
        public IEnumerable<MWOApprovedResponse> MWOsApproved { get; set; } = null!;
        public IEnumerable<MWOClosedResponse> MWOsClosed { get; set; } = null!;
    }
}
