using Shared.Models.MWOTypes;

namespace Shared.Models.MWO
{
    public class UpdateMWORequest
    {
        public Guid Id { get; set; }    
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.Replacement;


    }
}
