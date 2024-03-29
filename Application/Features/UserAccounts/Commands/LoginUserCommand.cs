using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserAccounts.Commands
{
    public record LoginUserCommand(LoginUserRequestDto Data) : IRequest<IResult<LoginUserResponse>>;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IResult<LoginUserResponse>>
    {
        private IUserAccountRepository Repository { get; set; }

        public LoginUserCommandHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await Repository.LoginUser(request.Data);
        }
    }
}
