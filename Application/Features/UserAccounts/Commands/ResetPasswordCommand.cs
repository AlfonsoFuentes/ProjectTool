using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Commands
{
    public record ResetPasswordCommand(string email):IRequest<IResult<UserReponse>>;

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult<UserReponse>>
    {
        private IUserAccountRepository Repository { get; set; }

        public ResetPasswordCommandHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UserReponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result= await Repository.ResetPassword(request.email);
            if(result)
            {
                return Result<UserReponse>.Success("Password reseted");
            }
            return Result<UserReponse>.Fail("could not reset password");
        }
    }
}
