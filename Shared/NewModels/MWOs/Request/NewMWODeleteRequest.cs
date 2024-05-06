namespace Shared.NewModels.MWOs.Request
{
    public class NewMWODeleteRequest
    {
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
