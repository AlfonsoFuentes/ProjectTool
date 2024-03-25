using Shared.Models.ChangePasswords;
using Shared.Models.Logins;
using Shared.Models.UserManagement;

namespace Client.Infrastructure.Managers.Accounts
{
    public interface IAccountManager : IManager
    {
        Task<IResult<UsersResponse>> GetUsersAsync();
        Task<IResult> ReviewEmailExist(string email);
        Task<IResult> CreateSuperAdminUser();

        Task<IResult> ChangePassWord(ChangePasswordRequest changePasswordRequest);
        Task<IResult> ReviewChangePassWord(LoginRequest loginRequest);
        Task<IResult> Delete(CurrentUser loginRequest);
    }
    public class AccountManager : IAccountManager
    {
        private HttpClient Http;

        public AccountManager(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IResult> ReviewEmailExist(string email)
        {

            try
            {
                var httpresult = await Http.GetAsync($"Account/{email}");

                var result = await httpresult.ToResult();
                return result;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return Result.Fail();
        }

        public async Task<IResult> CreateSuperAdminUser()
        {
            var reviewSuperadmin = await Http.PostAsync($"Account/reviewsuperadminexist", null);
            var reviewSuperadminresult = await reviewSuperadmin.ToResult();
            if (!reviewSuperadminresult.Succeeded)
            {
                var httpresult = await Http.PostAsync($"Account/createsuperadmin", null);
                var result = await httpresult.ToResult();
                return result.Succeeded ? Result.Success("Admin user created") : Result.Success("Something went wrong with creation of adminuser");
            }
            return Result.Success("Admin user already exist");


        }

        public async Task<IResult> ChangePassWord(ChangePasswordRequest loginRequest)
        {
            var httpresult = await Http.PostAsJsonAsync($"Account/changepassword", loginRequest);
            var result = await httpresult.ToResult();
            return result;

        }
        public async Task<IResult> ReviewChangePassWord(LoginRequest loginRequest)
        {
            var httpresult = await Http.GetAsync($"Account/reviewchangepassword?email={loginRequest.Email}");
            var result = await httpresult.ToResult();
            return result;

        }

        public async Task<IResult<UsersResponse>> GetUsersAsync()
        {
            var httpresult = await Http.GetAsync($"Account/GetUserList");
            var result = await httpresult.ToResult<UsersResponse>();
            return result;
        }

        public Task<IResult> Delete(CurrentUser loginRequest)
        {
            throw new NotImplementedException();
        }
    }
}
