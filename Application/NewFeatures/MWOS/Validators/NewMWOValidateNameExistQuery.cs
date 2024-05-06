namespace Application.Features.MWOs.Queries
{
    public record NewMWOValidateNameExistQuery(Guid MWOId,string mwoname) : IRequest<bool>;
    public class NewMWOValidateNameExistQuerytHandler : IRequestHandler<NewMWOValidateNameExistQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewMWOValidateNameExistQuerytHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewMWOValidateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfMWONameExist(request.MWOId, request.mwoname);
        }
    }
}
