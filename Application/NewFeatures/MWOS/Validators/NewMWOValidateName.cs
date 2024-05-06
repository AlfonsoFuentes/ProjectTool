namespace Application.Features.MWOs.Queries
{
    public record NewMWOValidateNameQuery(string mwoname) : IRequest<bool>;
    public class NewMWOValidateNameHandler : IRequestHandler<NewMWOValidateNameQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewMWOValidateNameHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewMWOValidateNameQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfMWONameExist(request.mwoname);
        }
    }

}
