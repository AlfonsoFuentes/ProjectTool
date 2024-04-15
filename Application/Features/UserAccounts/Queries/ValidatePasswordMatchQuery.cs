using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Logins;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Queries
{
    public record ValidatePasswordMatchQuery(LoginUserRequest Data) : IRequest<IResult<UserReponse>>;
    public class ValidatePasswordMatchQueryHandler : IRequestHandler<ValidatePasswordMatchQuery, IResult<UserReponse>>
    {
        IUserAccountRepository Repository { get; set; }

        public ValidatePasswordMatchQueryHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UserReponse>> Handle(ValidatePasswordMatchQuery request, CancellationToken cancellationToken)
        {
            var result = await Repository.ValidateIfPasswordMatch(request.Data.Email,request.Data.Password);


            return result ? Result<UserReponse>.Success("Password Confirmed") : Result<UserReponse>.Fail("Password confirmed");
        }
    }

}
