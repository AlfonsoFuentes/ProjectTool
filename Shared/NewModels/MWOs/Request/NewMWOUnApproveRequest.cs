namespace Shared.NewModels.MWOs.Request
{
    public class NewMWOUnApproveRequest
    {
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        
    }
}
