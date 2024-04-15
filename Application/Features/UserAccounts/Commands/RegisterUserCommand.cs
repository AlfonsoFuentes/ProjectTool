using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Commands
{
    public record RegisterUserCommand(RegisterUserRequest Data) : IRequest<IResult<RegisterUserResponse>>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IResult<RegisterUserResponse>>
    {
        private IUserAccountRepository Repository { get; set; }

        public RegisterUserCommandHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await Repository.RegisterUser(request.Data);


        }
    }
}
