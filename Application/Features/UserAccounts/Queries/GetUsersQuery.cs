using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Features.UserAccounts.Queries
{
    public  record GetUsersQuery:IRequest<IResult<UserReponseList>>;

    public class GetUsersQueryHandler:IRequestHandler<GetUsersQuery,IResult<UserReponseList>>
    {
        IUserAccountRepository Repository { get; set; }

        public GetUsersQueryHandler(IUserAccountRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UserReponseList>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await Repository.GetUserList();
        }
    }


}
