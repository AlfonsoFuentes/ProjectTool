using Blazored.LocalStorage;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;
using Shared.Models.UserManagements;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Infrastructure.Managers.UserManagement
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
        Task Logout();
        Task<string> RefreshToken();
        Task<UserResponseList> GetUserList();
        Task<bool> ValidatePasswordMatch(LoginUserRequest request);
        Task<bool> ValidateIfEmailExist(string email);
        Task<bool> ValidateIfPasswordConfirmed(string email);
        Task<bool> ResetPassword(string email);
        Task<bool> ChangePasswordUser(ChangePasswordUserRequest request);

        Task<bool> DeleteUser(string email);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient Http;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            Http = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var content = JsonSerializer.Serialize(userForRegistration);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var registrationResult = await Http.PostAsync("accounts/Registration", bodyContent);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();

            if (!registrationResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, _options);
                return result!;
            }

            return new RegistrationResponseDto { IsSuccessfulRegistration = true };
        }
        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
        {
            var content = JsonSerializer.Serialize(userForAuthentication);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var authResult = await Http.PostAsync("accounts/login", bodyContent);
            var authContent = await authResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, _options);

            if (!authResult.IsSuccessStatusCode)
                return result!;

            await _localStorage.SetItemAsync("authToken", result!.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return new AuthResponseDto { UserId = result.UserId, IsAuthSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            Http.DefaultRequestHeaders.Authorization = null;
        }
        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var tokenDto = JsonSerializer.Serialize(new RefreshTokenDto { Token = token!, RefreshToken = refreshToken! });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");

            var refreshResult = await Http.PostAsync("token/refresh", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(refreshContent, _options);

            if (!refreshResult.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong during the refresh token action");

            await _localStorage.SetItemAsync("authToken", result!.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return result.Token;
        }

        public async Task<UserResponseList> GetUserList()
        {
            var httpresult = await Http.GetAsync($"accounts/GetAll");
            return await httpresult.ToObject<UserResponseList>();
        }


        public async Task<bool> ValidatePasswordMatch(LoginUserRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"accounts/ValidatePasswordMatch", request);
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidateIfEmailExist(string email)
        {
            var httpresult = await Http.GetAsync($"accounts/ValidateEmailExist/{email}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidateIfPasswordConfirmed(string email)
        {
            var httpresult = await Http.GetAsync($"accounts/ValidatePasswordConfirmed/{email}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ResetPassword(string email)
        {
            var httpresult = await Http.GetAsync($"accounts/ResetPassword/{email}");


            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ChangePasswordUser(ChangePasswordUserRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"accounts/ChangePassword", request);

            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> DeleteUser(string email)
        {
            var httpresult = await Http.GetAsync($"accounts/DeletedUser/{email}");


            return await httpresult.ToObject<bool>();
        }
    }
}
