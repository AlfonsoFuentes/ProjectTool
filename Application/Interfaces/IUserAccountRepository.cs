using Domain.Entities.Account;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;

namespace Application.Interfaces
{
    public interface IUserAccountRepository
    {
       
        Task<IResult<RegisterUserResponse>> RegisterUser(RegisterUserRequest Data);
        Task<IResult<LoginUserResponse>> LoginUser(LoginUserRequest Data);
        Task<IResult<RegisterUserResponse>> RegisterSuperAdminUser(RegisterSuperAdminUserRequest userDTO);
        Task<IResult<UserReponseList>?> GetUserList();

        Task<bool> ResetPassword(string email);
        Task<bool> ValidateIfPasswordConfirmed(string email);
        Task<AplicationUser?> ValidateIfEmailExist(string email);
        Task<IResult<UserReponse>> ChangePasswordUser(ChangePasswordUserRequest Data);
        Task<bool> ValidateIfPasswordMatch(string email, string password);
    }
}
