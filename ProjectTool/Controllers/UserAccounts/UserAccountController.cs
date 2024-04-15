using Application.Features.MWOs.Commands;
using Application.Features.UserAccounts;
using Application.Features.UserAccounts.Commands;
using Application.Features.UserAccounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Commons.UserManagement;
using Shared.Models.MWO;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;

namespace Server.Controllers.UserAccounts
{
    [Route("[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private IMediator Mediator { get; set; }
        private CurrentUser CurrentUser;
        public UserAccountController(IMediator mediator, CurrentUser currentUser)
        {
            Mediator = mediator;
            CurrentUser = currentUser;
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
        [HttpGet("ValidatePasswordConfirmed/{email}")]
        public async Task<IActionResult> ValidatePasswordConfirmed(string email)
        {

            return Ok(await Mediator.Send(new ValidatePasswordConfirmedQuery(email)));
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
        [HttpPost("ReceiveCurrentUser")]
        public async Task<IActionResult> ReceiveCurrentUser(LoginUserResponse user)
        {
            CurrentUser.UserId = user.Id;
            CurrentUser.Role = user.Role;
            CurrentUser.UserName = user.Email;   
            return Ok(await Task.FromResult(true));
        }

    }
}
