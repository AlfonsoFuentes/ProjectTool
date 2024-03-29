using Application.Interfaces;
using Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Queries
{
    public record ValidateEmailExistQuery(string email) : IRequest<IResult<UserReponse>>;
    public class ValidateEmailExistQueryHandler : IRequestHandler<ValidateEmailExistQuery, IResult<UserReponse>>
    {
        IUserAccountRepository Repository { get; set; }

        public ValidateEmailExistQueryHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UserReponse>> Handle(ValidateEmailExistQuery request, CancellationToken cancellationToken)
        {
            var user = await Repository.ValidateIfEmailExist(request.email);
            if (user == null)
            {
                return Result<UserReponse>.Fail("Email doesn't exist");
            }
           
          
            UserReponse result = new()
            {
                Email = user.Email!,
                Id = user.Id,
                IsEmailConfirmed = user.EmailConfirmed,
          
                Role = user.InternalRole,

            };

            return Result<UserReponse>.Success(result);
        }
    }
}
