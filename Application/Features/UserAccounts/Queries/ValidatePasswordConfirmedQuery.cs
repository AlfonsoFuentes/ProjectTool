using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Queries
{
    public record ValidatePasswordConfirmedQuery(string email) : IRequest<IResult<UserReponse>>;
    public class ValidatePasswordConfirmedQueryHandler : IRequestHandler<ValidatePasswordConfirmedQuery, IResult<UserReponse>>
    {
        IUserAccountRepository Repository { get; set; }

        public ValidatePasswordConfirmedQueryHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UserReponse>> Handle(ValidatePasswordConfirmedQuery request, CancellationToken cancellationToken)
        {
            var result = await Repository.ValidateIfPasswordConfirmed(request.email);
            

            return result? Result<UserReponse>.Success("Password Confirmed"):Result<UserReponse>.Fail("Password confirmed");
        }
    }

}
