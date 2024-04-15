using Application.Interfaces;
using Domain.Entities.Account;
using Infrastructure.GenerateTokens;
using Infrastructure.Persistence.Repositories.Suppliers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;

namespace Infrastructure.Persistence.Repositories.UserAccounts
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private UserManager<AplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IGenerateToken GenerateToken;

        public UserAccountRepository(RoleManager<IdentityRole> roleManager, UserManager<AplicationUser> userManager, IGenerateToken generateToken)
        {

            this.roleManager = roleManager;
            this.userManager = userManager;
            GenerateToken = generateToken;
        }

        public async Task<IResult<RegisterUserResponse>> RegisterUser(RegisterUserRequest userDTO)
        {
            if (userDTO is null) return Result<RegisterUserResponse>.Fail("Model is empty");
            var newUser = new AplicationUser()
            {
                InternalRole = userDTO.Role.Name,
                Email = userDTO.Email,
                PasswordHash = userDTO.Password,
                UserName = userDTO.Email
            };
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return Result<RegisterUserResponse>.Fail("User registered already");

            var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
            if (!createUser.Succeeded) return Result<RegisterUserResponse>.Fail(createUser.Errors.Select(x => x.Description).FirstOrDefault()!);

            //Assign Default Role : Admin to first registrar; rest is user
            var checkUser = await roleManager.FindByNameAsync(userDTO.Role.Name);
            if (checkUser is null)
                await roleManager.CreateAsync(new IdentityRole() { Name = userDTO.Role.Name });

            await userManager.AddToRoleAsync(newUser, userDTO.Role.Name);
            RegisterUserResponse result = new()
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
                UserName = userDTO.Email
            };
            return Result<RegisterUserResponse>.Success(result, "Account Created succesfully");
        }

        public async Task<IResult<LoginUserResponse>> LoginUser(LoginUserRequest loginDTO)
        {
            LoginUserResponse result = new()
            {
                Email = loginDTO.Email,
                Password = loginDTO.Password,

            };
            if (loginDTO == null)
                return Result<LoginUserResponse>.Fail("Login container is empty");

            var getUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser is null)
                return Result<LoginUserResponse>.Fail("User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPasswords)
                return Result<LoginUserResponse>.Fail("Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);

            result.Id = getUser.Id;
            result.Role = getUserRole.First();

            result.Token = GenerateToken.GetToken(result);

            return Result<LoginUserResponse>.Success(result, "Login completed");
        }

        public async Task<IResult<RegisterUserResponse>> RegisterSuperAdminUser(RegisterSuperAdminUserRequest userDTO)
        {
            const string Administrator = "Administrator";
            if (userDTO is null) return Result<RegisterUserResponse>.Fail("Model is empty");
            var newUser = new AplicationUser()
            {
                InternalRole = Administrator,
                Email = userDTO.Email,
                PasswordHash = userDTO.Password,
                UserName = userDTO.Email,
                EmailConfirmed = true,
            };
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return Result<RegisterUserResponse>.Fail("User registered already");

            var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
            if (!createUser.Succeeded) return Result<RegisterUserResponse>.Fail(createUser.Errors.Select(x => x.Description).FirstOrDefault()!);

            //Assign Default Role : Admin to first registrar; rest is user
            var checkUser = await roleManager.FindByNameAsync(Administrator);
            if (checkUser is null)
                await roleManager.CreateAsync(new IdentityRole() { Name = Administrator });

            await userManager.AddToRoleAsync(newUser, Administrator);
            RegisterUserResponse result = new()
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
                UserName = userDTO.Email
            };
            return Result<RegisterUserResponse>.Success(result, "Account Created succesfully");
        }
        public async Task<IResult<UserReponseList>?> GetUserList()
        {
            UserReponseList response = new();

            var userlist = await userManager.Users.ToListAsync();
            response.Users = userlist.Select(x => new UserReponse()
            {
                Id = x.Id,
                Email = x.Email!,
                IsEmailConfirmed = x.EmailConfirmed,
                Name = x.Email!,
                Role = x.InternalRole,

            }).ToList();
            return Result<UserReponseList>.Success(response);
        }
        public async Task<AplicationUser?> ValidateIfEmailExist(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user;
        }
        public async Task<bool> ValidateIfPasswordConfirmed(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return false;

            return user.EmailConfirmed;
        }
        public async Task<bool> ValidateIfPasswordMatch(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return false;
            if (!user.EmailConfirmed) return false;

            bool checkUserPasswords = await userManager.CheckPasswordAsync(user, password);

            return checkUserPasswords;
        }
        public async Task<bool> ResetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return false;
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, "RegisterUserPassword123*");
            await userManager.UpdateAsync(user);
            return true;
        }

        public async Task<IResult<UserReponse>> ChangePasswordUser(ChangePasswordUserRequest Data)
        {
            var user = await userManager.FindByEmailAsync(Data.Email);
            if (user is null) return Result<UserReponse>.Fail("User not found");

            try
            {

                await userManager.ChangePasswordAsync(user, "RegisterUserPassword123*", Data.Password);

                user.EmailConfirmed = true;

                await userManager.UpdateAsync(user);
                return Result<UserReponse>.Success("Password changed succesfully");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return Result<UserReponse>.Fail("Something went wrong");
        }
    }
}
