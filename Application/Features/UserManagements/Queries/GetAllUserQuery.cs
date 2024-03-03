using Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.Suppliers;
using Shared.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserManagements.Queries
{
    public record GetAllUserQuery : IRequest<IResult<List<UserResponse>>>;

    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IResult<List<UserResponse>>>
    {
        private UserManager<AplicationUser> userManager;

        public GetAllUserQueryHandler(UserManager<AplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IResult<List<UserResponse>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var resultManager = await userManager.Users.ToListAsync();
            List<UserResponse> result = resultManager.Select(x => new UserResponse()
            {
                UserId = x.Id,
                Role = x.InternalRole,
                UserName = x.UserName!

            }).ToList();
            return Result<List<UserResponse>>.Success(result);
        }
    }

}
