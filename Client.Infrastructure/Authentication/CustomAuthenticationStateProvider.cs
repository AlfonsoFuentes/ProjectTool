using Blazored.LocalStorage;
using Client.Infrastructure.Managers.UserAccount;
using Shared.Commons.UserManagement;
using Shared.Models.UserAccounts.Reponses;
using System.Security.Claims;

namespace Client.Infrastructure.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        private ILocalStorageService localStorageService;
        private CurrentUser currentUser;
        private IUserAccountService userAccountService;
        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, CurrentUser currentUser, IUserAccountService userAccountService)
        {
            this.localStorageService = localStorageService;
            this.currentUser = currentUser;
            this.userAccountService = userAccountService;
        }



        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string stringToken = (await localStorageService.GetItemAsStringAsync("token"))!;

                if (string.IsNullOrWhiteSpace(stringToken))
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var claims = Generics.Generics.GetClaimsFromToken(stringToken);
                currentUser.UserId = claims.Id;
                currentUser.Role = claims.Role;
                currentUser.UserName = claims.Email;
                LoginUserResponse userresponse = new()

                {
                    Email = claims.Email,
                    Id = claims.Id,
                    Role = claims.Role,
                };

                await userAccountService.SendCurrentUserToServer(userresponse);
                var claimsPrincipal = Generics.Generics.SetClaimPrincipal(claims);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public async Task UpdateAuthenticationState(string? token)
        {
            ClaimsPrincipal claimsPrincipal = new();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var userSession = Generics.Generics.GetClaimsFromToken(token);
                claimsPrincipal = Generics.Generics.SetClaimPrincipal(userSession);
                currentUser.UserId = userSession.Id;
                currentUser.Role = userSession.Role;
                currentUser.UserName = userSession.Email;
                LoginUserResponse userresponse = new()

                {
                    Email = userSession.Email,
                    Id = userSession.Id,
                    Role = userSession.Role,
                };
                await userAccountService.SendCurrentUserToServer(userresponse);
                await localStorageService.SetItemAsStringAsync("token", token);
            }
            else
            {
                await userAccountService.SendCurrentUserToServer(new());
                claimsPrincipal = anonymous;
                await localStorageService.RemoveItemAsync("token");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
