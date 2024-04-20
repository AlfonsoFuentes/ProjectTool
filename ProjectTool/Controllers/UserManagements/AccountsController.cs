using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Models.UserManagements;
using System.IdentityModel.Tokens.Jwt;

namespace Server.Controllers.UserManagements
{
    //[Route("[controller]")]
    //[ApiController]
    //public class AccountsController : ControllerBase
    //{
    //    private readonly UserManager<AplicationUser> _userManager;
    //    private readonly ITokenService _tokenService;
    //    public AccountsController(UserManager<AplicationUser> userManager, ITokenService tokenService)
    //    {
    //        _userManager = userManager;
    //        _tokenService = tokenService;
    //    }
    //    [HttpPost("Registration")]
    //    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    //    {
    //        if (userForRegistration == null || !ModelState.IsValid)
    //            return BadRequest();

    //        var user = new AplicationUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

    //        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
    //        if (!result.Succeeded)
    //        {
    //            var errors = result.Errors.Select(e => e.Description);

    //            return BadRequest(new RegistrationResponseDto { Errors = errors });
    //        }

    //        await _userManager.AddToRoleAsync(user, "Viewer");

    //        return StatusCode(201);
    //    }

    //    [HttpPost("Login")]
    //    public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
    //    {
    //        var user = await _userManager.FindByNameAsync(userForAuthentication.Email);

    //        if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
    //            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

    //        var signingCredentials = _tokenService.GetSigningCredentials();
    //        var claims = await _tokenService.GetClaims(user);
    //        var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
    //        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

    //        user.RefreshToken = _tokenService.GenerateRefreshToken();
    //        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    //        await _userManager.UpdateAsync(user);

    //        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, RefreshToken = user.RefreshToken });
    //    }

    //}
}
