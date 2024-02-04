namespace Application.Interfaces
{
    public interface IRepository
    {
        IAppDbContext Context { get; }
    }
}
