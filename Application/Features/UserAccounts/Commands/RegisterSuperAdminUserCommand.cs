using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Commands
{
    public record RegisterSuperAdminUserCommand(RegisterSuperAdminUserRequest Data) : IRequest<IResult<RegisterUserResponse>>;
    public class RegisterSuperAdminUserCommandHandler : IRequestHandler<RegisterSuperAdminUserCommand, IResult<RegisterUserResponse>>
    {
        private IUserAccountRepository Repository { get; set; }

        public RegisterSuperAdminUserCommandHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<RegisterUserResponse>> Handle(RegisterSuperAdminUserCommand request, CancellationToken cancellationToken)
        {
            return await Repository.RegisterSuperAdminUser(request.Data);
        }
    }
}
