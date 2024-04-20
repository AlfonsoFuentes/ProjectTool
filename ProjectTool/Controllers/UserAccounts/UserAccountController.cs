using Application.Features.UserAccounts.Commands;
using Application.Features.UserAccounts.Queries;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;

namespace Server.Controllers.UserAccounts
{
    [Route("[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private IMediator Mediator { get; set; }
        CurrentUser currentUser { get; set; }

        public UserAccountController(IMediator mediator, CurrentUser currentUser)
        {
            Mediator = mediator;
            this.currentUser = currentUser;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {

            return Ok(await Mediator.Send(new RegisterUserCommand(request)));
        }
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(LoginUserRequest request)
        {
            var result = await Mediator.Send(new LoginUserCommand(request));
           
            return Ok(result);
        }
        [HttpPost("RegisterSuperAdminUser")]
        public async Task<IActionResult> RegisterSuperAdminUser(RegisterSuperAdminUserRequest request)
        {

            return Ok(await Mediator.Send(new RegisterSuperAdminUserCommand(request)));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserList()
        {

            return Ok(await Mediator.Send(new GetUsersQuery()));
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email)
        {

            return Ok(await Mediator.Send(new ResetPasswordCommand(email)));
        }
        
        [HttpPost("ValidatePasswordMatch")]
        public async Task<IActionResult> ValidatePasswordMatch(LoginUserRequest user)
        {

            return Ok(await Mediator.Send(new ValidatePasswordMatchQuery(user)));
        }
        [HttpGet("ValidateEmailExist/{email}")]
        public async Task<IActionResult> ValidateEmailExist(string email)
        {

            return Ok(await Mediator.Send(new ValidateEmailExistQuery(email)));
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordUserRequest request)
        {

            return Ok(await Mediator.Send(new ChangePasswordUserCommand(request)));
        }

        [HttpGet("GetCurrentUser/{UserId}")]
        public async Task<IActionResult> ReceiveCurrentUser(string UserId)
        {

            return Ok(await Mediator.Send(new ReceiveUserFromClientCommand(UserId)));
        }
        [HttpGet("ValidatePasswordConfirmed/{email}")]
        public async Task<IActionResult> ValidatePasswordConfirmed(string email)
        {

            return Ok(await Mediator.Send(new ValidatePasswordConfirmedQuery(email)));
        }
        [HttpGet("ClearCurrentUser")]
        public async Task<IActionResult> ClearCurrentUser()
        {
            currentUser.UserId = "";
            currentUser.UserName = "";
            currentUser.Role = "";
            return Ok();
        }
    }
}
