using Shared.Models.MWOTypes;

namespace Shared.Models.MWO
{
    public class UpdateMWOMinimalRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.Replacement;
       
        public List<string> ValidationErrors { get; set; } = new();
       

        public void ChangeName(string name)
        {
            ValidationErrors.Clear();
            Name = name;


        }
        public void ChangeType(MWOTypeEnum type)
        {
            Type = type;
        }

      
    }
}
