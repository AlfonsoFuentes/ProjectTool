namespace Application.NewFeatures.SoftwareVersions.Validators
{
    public record NewSoftwareVersionValidateNameExistQuery(Guid SoftwareVersionId, string Name) : IRequest<bool>;
    public class NewSoftwareVersionValidateNameExistQueryHandler : IRequestHandler<NewSoftwareVersionValidateNameExistQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSoftwareVersionValidateNameExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSoftwareVersionValidateNameExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewIfSoftwareVersionNameExist(request.SoftwareVersionId, request.Name);
            return result;
        }
    }
}
