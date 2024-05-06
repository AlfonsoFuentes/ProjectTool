

using Shared.NewModels.SoftwareVersion;

namespace Application.NewFeatures.Brands.Queries
{
    public record NewSoftwareVersionGetAllQuery : IRequest<IResult<NewSoftwareVersionListResponse>>;

    internal class NewSoftwareVersionGetAllQueryHandler : IRequestHandler<NewSoftwareVersionGetAllQuery, IResult<NewSoftwareVersionListResponse>>
    {
        private IQueryRepository Repository { get; set; }
        private readonly IAppDbContext _cache;
        public NewSoftwareVersionGetAllQueryHandler(IQueryRepository repository, IAppDbContext cache)
        {
            Repository = repository;
            _cache = cache;
        }

        public async Task<IResult<NewSoftwareVersionListResponse>> Handle(NewSoftwareVersionGetAllQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<SoftwareVersion>>> getAll = () => Repository.GetAllAsync<SoftwareVersion>();
            try
            {
                var List = await _cache.GetOrAddAsync(Cache.GetAllSoftwareVersion, getAll);
                NewSoftwareVersionListResponse responseList = new NewSoftwareVersionListResponse()
                {
                    SoftwareVersion = List.Select(x => new NewSoftwareVersionResponse()
                    {
                        SoftwareVersionId = x.Id,
                        Name = x.Name,

                    }).ToList(),
                };
                return Result<NewSoftwareVersionListResponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }


            return Result<NewSoftwareVersionListResponse>.Fail();
        }
    }
}
