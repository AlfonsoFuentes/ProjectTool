namespace Shared.NewModels.SoftwareVersion
{
    public class NewSoftwareVersionResponse
    {
        public Guid SoftwareVersionId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class NewSoftwareVersionListResponse
    {
        public List<NewSoftwareVersionResponse> SoftwareVersion { get; set; } = new();
    }
}
