using Shared.Models.MWOTypes;

namespace Shared.Models.MWO
{
    public class CreateMWORequest
    {
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;


    }
}
