using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Commands
{
    public record ChangePasswordUserCommand(ChangePasswordUserRequest Data) : IRequest<IResult<UserReponse>>;
    public class ChangePasswordUserCommandHandler : IRequestHandler<ChangePasswordUserCommand, IResult<UserReponse>>
    {
        private IUserAccountRepository Repository { get; set; }

        public ChangePasswordUserCommandHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UserReponse>> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
        {
            return await Repository.ChangePasswordUser(request.Data);


        }
    }
}
