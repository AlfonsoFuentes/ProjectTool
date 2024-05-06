namespace Application.NewFeatures.Brands.Queries
{
    public record NewBrandGetByIdToUpdateQuery(Guid BrandId) : IRequest<IResult<NewBrandUpdateRequest>>;

    internal class NewBrandGetByIdQueryHamdler : IRequestHandler<NewBrandGetByIdToUpdateQuery, IResult<NewBrandUpdateRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewBrandGetByIdQueryHamdler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewBrandUpdateRequest>> Handle(NewBrandGetByIdToUpdateQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetBrandByIdAsyn(request.BrandId);
            if (row == null)
            {
                return Result<NewBrandUpdateRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.Brand));
            }
            var result = row.ToUpdateRequest();
            return Result<NewBrandUpdateRequest>.Success(result);

        }
    }
}
