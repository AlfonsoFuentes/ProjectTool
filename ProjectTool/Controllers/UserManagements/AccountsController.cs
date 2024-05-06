using Domain.Entities.Account;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Services;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;
using Shared.Models.UserManagements;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Controllers.UserManagements
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AplicationUser> signInManager;
        public AccountsController(UserManager<AplicationUser> userManager, ITokenService tokenService,
            SignInManager<AplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            this.signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest();

            var user = new AplicationUser
            {
                UserName = userForRegistration.Email,
                Email = userForRegistration.Email,
                RefreshToken = "!",
                RefreshTokenExpiryTime = DateTime.UtcNow,

            };

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }
            var customClaims = new List<Claim>()
            {
                new Claim(Constants.ClaimTenantId,user.Id),
            };
            await _userManager.AddClaimsAsync(user, customClaims);
            await _userManager.AddToRoleAsync(user, "Viewer");
            await signInManager.SignInAsync(user, isPersistent: true);
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByNameAsync(userForAuthentication.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            var existingclaims = await _userManager.GetClaimsAsync(user);
            if (existingclaims == null || existingclaims.Count == 0 || !existingclaims.Any(x => x.Type == Constants.ClaimTenantId))
            {
                var customClaims = new List<Claim>()
                {
                    new Claim(Constants.ClaimTenantId,user.Id),
                };
                await _userManager.AddClaimsAsync(user, customClaims);
            }


            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            var result = await signInManager.PasswordSignInAsync(user, userForAuthentication.Password, isPersistent: true, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });
            }

            return Ok(new AuthResponseDto { UserId = user.Id, IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken });
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUserList()
        {
            UserResponseList response = new();

            var userlist = await _userManager.Users.ToListAsync();
            var roles = (await _roleManager.Roles.ToListAsync()).Select(x => x.Name);

            foreach (var user in userlist)
            {

                UserResponse userReponse = new UserResponse()
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    IsEmailConfirmed = user.EmailConfirmed,
                    Roles = (await _userManager.GetRolesAsync(user)),
                };
                response.Users.Add(userReponse);
            }

            return Ok(response);
        }
        [HttpPost("ValidatePasswordMatch")]
        public async Task<IActionResult> ValidatePasswordMatch(LoginUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    bool checkUserPasswords = await _userManager.CheckPasswordAsync(user, request.Password);
                    return Ok(checkUserPasswords);
                }
            }

            return Ok(false);
        }
        [HttpGet("ValidateEmailExist/{email}")]
        public async Task<IActionResult> ValidateEmailExist(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);

            return Ok(result != null);
        }

        [HttpGet("ValidatePasswordConfirmed/{email}")]
        public async Task<IActionResult> ValidatePasswordConfirmed(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Ok(false);

            return Ok(user.EmailConfirmed);


        }
        [HttpGet("ResetPassword/{email}")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Ok(false);
            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, "RegisterUserPassword123*");
                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                string exm = ex.Message;
            }


            return Ok(true);
        }



        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) return Ok(false);

            try
            {

                await _userManager.ChangePasswordAsync(user, "RegisterUserPassword123*", request.Password);

                user.EmailConfirmed = true;

                await _userManager.UpdateAsync(user);
                return Ok(true);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return Ok(false);
        }

        [HttpGet("DeletedUser/{email}")]
        public async Task<IActionResult> DeletedUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Ok(false);

            await _userManager.DeleteAsync(user);

            return Ok(true);
        }

    }
}
