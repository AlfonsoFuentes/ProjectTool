namespace Shared.Models.MWO
{
    public class UnApproveMWORequest
    {
        public Guid MWOId {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string CECName {  get; set; } = string.Empty;
    }
}
