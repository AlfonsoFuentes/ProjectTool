using Shared.NewModels.SoftwareVersion;

namespace Application.NewFeatures.SoftwareVersions.Commands
{
    public record NewSoftwareVersionCreateCommand(NewSoftwareVersionCreateRequest Data) : IRequest<IResult>;

    internal class NewSoftwareVersionCreateCommandHandler : IRequestHandler<NewSoftwareVersionCreateCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        private IQueryRepository QueryRepository { get; set; }
        public NewSoftwareVersionCreateCommandHandler(IRepository repository, IAppDbContext appDbContext, IQueryRepository queryRepository)
        {
            Repository = repository;
            _appDbContext = appDbContext;
            QueryRepository = queryRepository;
        }

        public async Task<IResult> Handle(NewSoftwareVersionCreateCommand request, CancellationToken cancellationToken)
        {
            var versionlist = await QueryRepository.GetAllAsync<SoftwareVersion>();
            var last = versionlist.Count == 0 ? 1 : versionlist.OrderBy(x => x.Version).Last().Version + 1;

            var row = SoftwareVersion.Create(request.Data.Name, last);
            await Repository.AddAsync(row);
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllSoftwareVersion);

            return result > 0 ?
                Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.SoftwareVersion)) :
                Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.SoftwareVersion));
        }
    }


}
