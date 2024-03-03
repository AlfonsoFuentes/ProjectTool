using Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commons.Results;
using Shared.Models.Registers;
using Shared.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserManagements.Commands
{
    public record CreateUserCommand(CreateUserRequest Data):IRequest<IResult>;
    public class CreateUserCommandHandler:IRequestHandler<CreateUserCommand,IResult>
    {
        private UserManager<AplicationUser> userManager;
        private IUserStore<AplicationUser> userStore;

        public CreateUserCommandHandler(IUserStore<AplicationUser> userStore, UserManager<AplicationUser> userManager)
        {
            this.userStore = userStore;
            this.userManager = userManager;
        }

        public async Task<IResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var identity = new AplicationUser();
                await userManager.SetUserNameAsync(identity, request.Data.Email);
                identity.InternalRole = request.Data.Role;
                var emailStore = (IUserEmailStore<AplicationUser>)userStore;
                await emailStore.SetEmailAsync(identity, request.Data.Email,
                CancellationToken.None);

                var result = await userManager.CreateAsync(identity, request.Data.Password);
                var claims = new List<Claim>
            {
                             new(ClaimTypes.Name, request.Data.Email),
                             new(ClaimTypes.Email, request.Data.Email),
                             new(ClaimTypes.Role, request.Data.Role)
                         };

                await userManager.AddClaimsAsync(identity, claims);

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
            return Result.Success($"{request.Data.Email} created Succesfully");
            
        }
    }
}
