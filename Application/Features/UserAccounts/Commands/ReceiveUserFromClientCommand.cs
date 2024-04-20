using Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commons.Results;

namespace Application.Features.UserAccounts.Commands
{
    public record ReceiveUserFromClientCommand(string UserId):IRequest<IResult<bool>>;

    internal class ReceiveUserFromClientCommandHandler:IRequestHandler<ReceiveUserFromClientCommand, IResult<bool>>
    {
        private CurrentUser _currentUser;
        private UserManager<AplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public ReceiveUserFromClientCommandHandler(CurrentUser currentUser, 
            UserManager<AplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _currentUser = currentUser;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IResult<bool>> Handle(ReceiveUserFromClientCommand request, CancellationToken cancellationToken)
        {
            var getUser = await userManager.FindByIdAsync(request.UserId);
            if (getUser == null) { return Result<bool>.Fail(); }
            var getUserRole = await userManager.GetRolesAsync(getUser);
            _currentUser.UserId = getUser.Id;
            _currentUser.Role = getUserRole.First();
            return Result<bool>.Success();

        }
    }
}
