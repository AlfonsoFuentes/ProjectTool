using Application.Features.UserManagements.Commands;
using Application.Features.UserManagements.Queries;
using Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Commons.Results;
using Shared.Models.Logins;
using Shared.Models.UserManagement;


namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManagerController : ControllerBase
    {
        private SignInManager<AplicationUser> signInManager;
        private UserManager<AplicationUser> userManager;
        private IMediator Mediator { get; set; }

        public UserManagerController(IMediator mediator, UserManager<AplicationUser> userManager, SignInManager<AplicationUser> signInManager)
        {
            Mediator = mediator;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            return Ok(await Mediator.Send(new CreateUserCommand(request)));
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await Mediator.Send(new GetAllUserQuery()));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var aplicationuser = await userManager.FindByEmailAsync(loginRequest.Email);
            if (aplicationuser == null)
            {
                return Ok(Result<LoginResponse>.Fail());
            }
            var result = await signInManager.PasswordSignInAsync(aplicationuser, loginRequest.Password, loginRequest.RememberMe,
           lockoutOnFailure: false);

            if (result.Succeeded)
            {
                LoginResponse response = new()
                {
                    Email = loginRequest.Email,
                    Password = loginRequest.Password,
                };
                return Ok(Result<LoginResponse>.Success(response));

            }
            return Ok(Result<LoginResponse>.Fail());
        }
    }
}
