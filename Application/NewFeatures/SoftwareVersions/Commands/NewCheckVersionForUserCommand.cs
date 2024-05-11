using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.NewFeatures.SoftwareVersions.Commands
{
    public record NewCheckVersionForUserCommand(String UserId) : IRequest<IResult>;
    internal class NewCheckVersionForUserCommandHandler : IRequestHandler<NewCheckVersionForUserCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IVersionRepository VersionRepository { get; set; }
        private IRepository Repository {  get; set; }
        private IQueryRepository QueryRepository { get; set; }
        private readonly UserManager<AplicationUser> _userManager;
        public NewCheckVersionForUserCommandHandler(IVersionRepository repository, IAppDbContext appDbContext,
            IRepository _Repository,
            IQueryRepository queryRepository, UserManager<AplicationUser> userManager)
        {
            VersionRepository = repository;
            _appDbContext = appDbContext;
            QueryRepository = queryRepository;
            _userManager = userManager;
            Repository = _Repository;
        }

        public async Task<IResult> Handle(NewCheckVersionForUserCommand request, CancellationToken cancellationToken)
        {
            var softwarerVersionList = await QueryRepository.GetAllAsync<SoftwareVersion>();

            var currenuser = await _userManager.FindByIdAsync(request.UserId);
            var userversions = await QueryRepository.GetVersionsByUserIdAsync(request.UserId);
            bool updateVersion = false;
            foreach (var version in softwarerVersionList)
            {
                if (!userversions.Any(x => x.SoftwareVersionId == version.Id))
                {
                    updateVersion = true;
                    switch (version.Version)
                    {
                        case 1:
                            await UpdateVersion1();
                            await AddVersionToUser(currenuser!, version.Id);
                            break;
                        case 2:
                            await UpdateVersion2();
                            await AddVersionToUser(currenuser!, version.Id);
                            break;
                        case 3:
                            await UpdateVersion3();
                            await AddVersionToUser(currenuser!, version.Id);
                            break;
                    }

                }
            }

            if (updateVersion)
            {
                var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllSoftwareVersion);

                return result > 0 ?
                    Result.Success(ResponseMessages.ReponseSuccesfullyMessage("", ResponseType.Created, ClassNames.SoftwareVersion)) :
                    Result.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.Created, ClassNames.SoftwareVersion));
            }
            return Result.Success(ResponseMessages.ReponseSuccesfullyMessage("", ResponseType.Updated, ClassNames.SoftwareVersion));


        }
        async Task UpdateVersion1()
        {
            var budgetItemEngoneeringContingency = await VersionRepository.GetItemsToUpdateVersion1();

            foreach (var row in budgetItemEngoneeringContingency)
            {
                row.IsEngineeringItem = true;
                await Repository.UpdateAsync(row);
            }


        }
        async Task UpdateVersion2()
        {

            var purchaseorders = await VersionRepository.GetPurchaseOrderItemsToUpdateVersion2();

            foreach (var purchaseorder in purchaseorders)
            {
                var newactual = purchaseorder.AddPurchaseOrderReceived();
                newactual.ValueReceivedCurrency = purchaseorder.POItemActualCurrency;
                newactual.USDCOP = purchaseorder.USDCOP;
                newactual.USDEUR = purchaseorder.USDEUR;
                newactual.CurrencyDate = purchaseorder.PurchaseOrder.CurrencyDate;

                await Repository.AddAsync(newactual);
            }

        }
        async Task UpdateVersion3()
        {

            var purchaseOrderItemReceiveds = await VersionRepository.GetPurchaseOrderItemsReceivedToUpdateVersion3();

            foreach (var item in purchaseOrderItemReceiveds)
            {
                item.ReceivedDate = item.CurrencyDate;
                await Repository.UpdateAsync(item);
            }

        }
        async Task AddVersionToUser(AplicationUser user, Guid SoftwareVersionId)
        {
            var version = user.AddUpdatedSoftwarerVersion(SoftwareVersionId);
            await Repository.AddAsync(version);

        }
    }

}
