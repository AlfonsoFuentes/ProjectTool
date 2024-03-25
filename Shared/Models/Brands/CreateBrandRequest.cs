namespace Shared.Models.Brands
{
    public class CreateBrandRequest
    {

        public Func<Task<bool>> Validator { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public async Task ChangeName(string name)
        {
            Name = name;
            if (Validator != null) await Validator();
        }
    }
}
