namespace Application.Features.MWOs.Queries
{
    public record NewMWOValidateNumberExistQuery(Guid MWOId, string mwonumber):IRequest<bool>;

    public class NewMWOValidateNumberExistQueryHandler : IRequestHandler<NewMWOValidateNumberExistQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewMWOValidateNumberExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewMWOValidateNumberExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfMWONumberExist(request.MWOId,request.mwonumber);
        }
    }


}
