namespace Application.NewFeatures.SoftwareVersions.Validators
{
    public record NewSoftwareVersionValidateNameQuery(string Name) : IRequest<bool>;

    public class NewSoftwareVersionValidateNameQueryHandler : IRequestHandler<NewSoftwareVersionValidateNameQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSoftwareVersionValidateNameQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSoftwareVersionValidateNameQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfSupplierNameExist(request.Name);
        }
    }
}
