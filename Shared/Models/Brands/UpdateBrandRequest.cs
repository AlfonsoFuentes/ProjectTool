namespace Shared.Models.Brands
{
    public class UpdateBrandRequest
    {
        public Func<Task<bool>> Validator { get; set; } = null!;
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public async Task ChangeName(string name)
        {
            Name = name;
            if (Validator != null) await Validator();
        }
    }
}
