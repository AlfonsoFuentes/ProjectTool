using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.ChangePasswords;
using Shared.Models.Logins;
using Shared.Models.Registers;
using Shared.Models.SuperAdmins;
using System.Security.Claims;


namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private SignInManager<AplicationUser> signInManager;
        private UserManager<AplicationUser> userManager;
        private IUserStore<AplicationUser> userStore;
        private RoleManager<IdentityRole> roleManager;
        private CurrentUser CurrentUser;
        public AccountController(RoleManager<IdentityRole> roleManager,
            IUserStore<AplicationUser> userStore,
            UserManager<AplicationUser> userManager,
            SignInManager<AplicationUser> signInManager,
            CurrentUser currentUser)
        {
            this.roleManager = roleManager;
            this.userStore = userStore;
            this.userManager = userManager;
            this.signInManager = signInManager;
            CurrentUser = currentUser;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
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
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null) return Ok(Result.Fail());
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var emailconfirmed = await userManager.ConfirmEmailAsync(user, token);
            if (emailconfirmed.Succeeded)
            {
                var changePasswordResult = await userManager.ChangePasswordAsync(user, loginRequest.OldPassword, loginRequest.Password);



                if (changePasswordResult.Succeeded)
                {
                    LoginResponse response = new()
                    {
                        Email = loginRequest.Email,
                        Password = loginRequest.Password,
                    };
                    return Ok(Result<LoginResponse>.Success(response));

                }
            }

            return Ok(Result<LoginResponse>.Fail());
        }
        [HttpGet("reviewchangepassword")]
        public async Task<IActionResult> ReviewChangePassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null) return Ok(Result.Fail());
            var emailconfirmed = await userManager.IsEmailConfirmedAsync(user!);

            if (emailconfirmed)
            {
                return Ok(Result.Success());
            }

            return Ok(Result<LoginResponse>.Fail());
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await CreateUser(registerRequest);

            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
        [HttpPost("reviewsuperadminexist")]
        public async Task<IActionResult> ReviewCreateSuperAdmin()
        {
            string superadmin = "alfonsofuen@gmail.com";
            var result = await userManager.FindByEmailAsync(superadmin);
            return Ok(result == null ? Result.Fail() : Result.Success());

        }
        [HttpPost("createsuperadmin")]
        public async Task<IActionResult> CreateSuperAdmin()
        {
            SuperAdminRequest superAdminRequest = new()
            {
                Name = "alfonsofuen@gmail.com",
                Password = "1506*Afd1974*"

            };

            var identity = new AplicationUser();
            await userManager.SetUserNameAsync(identity, superAdminRequest.Name);

            var emailStore = (IUserEmailStore<AplicationUser>)userStore;
            await emailStore.SetEmailAsync(identity, superAdminRequest.Name,
                CancellationToken.None);

            var result = await userManager.CreateAsync(identity, superAdminRequest.Password);

            var claims = new List<Claim>
            {
                             new(ClaimTypes.Name, superAdminRequest.Name),
                             new(ClaimTypes.Email, superAdminRequest.Name),
                             new(ClaimTypes.Role, "SuperAdmin")
                         };

            await userManager.AddClaimsAsync(identity, claims);
            var superAdminRole = new IdentityRole("SuperAdmin");
            await roleManager.CreateAsync(superAdminRole);
            await userManager.AddToRoleAsync(identity, "SuperAdmin");

            return Ok(Result.Success());
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> ReviewEmailExist(string email)
        {
            var result = await userManager.FindByEmailAsync(email);
            return Ok(result == null ? Result.Fail() : Result.Success());
        }
        [HttpGet("DefineCurrentUser")]
        public async Task<IActionResult> DefineCurrentUser(string email)
        {
            var result = await userManager.FindByEmailAsync(email);
            if (result == null) Result.Fail();

            CurrentUser.UserId = result!.Id.ToString();
            CurrentUser.UserName = result!.UserName!;
            CurrentUser.Roles = await userManager.GetRolesAsync(result);
            var retorno = Result<CurrentUser>.Success(CurrentUser);
            return Ok(retorno);
        }
        async Task<IResult<RegisterResponse>> CreateUser([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var identity = new AplicationUser();
                await userManager.SetUserNameAsync(identity, registerRequest.UserName);

                var emailStore = (IUserEmailStore<AplicationUser>)userStore;
                await emailStore.SetEmailAsync(identity, registerRequest.Email,
                    CancellationToken.None);

                var result = await userManager.CreateAsync(identity, registerRequest.Password);

                var claims = new List<Claim>
                         {
                             new(ClaimTypes.Name, registerRequest.UserName),
                             new(ClaimTypes.Email, registerRequest.Email),
                             new(ClaimTypes.Role, registerRequest.Role)
                         };

                await userManager.AddClaimsAsync(identity, claims);
                var roleExist = await roleManager.RoleExistsAsync(registerRequest.Role);
                if (!roleExist)
                {
                    var NewRole = new IdentityRole(registerRequest.Role);
                    await roleManager.CreateAsync(NewRole);


                }
                await userManager.AddToRoleAsync(identity, registerRequest.Role);
                RegisterResponse response = new()
                {
                    Name = registerRequest.Email,
                    Password = registerRequest.Password,
                };
                return result.Succeeded ? Result<RegisterResponse>.Success(response) : Result<RegisterResponse>.Fail();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return Result<RegisterResponse>.Fail();
        }
    }
}
