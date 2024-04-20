using Blazored.LocalStorage;
using Client.Infrastructure.Managers.UserAccount;
using Shared.Models.UserAccounts.Reponses;

namespace Client.Infrastructure.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        private ILocalStorageService localStorageService;
  
        private IUserAccountService userAccountService;
        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, IUserAccountService userAccountService)
        {
            this.localStorageService = localStorageService;
       
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
                var UserId = claims.Id;
                               
                await userAccountService.SendCurrentUserToServer(UserId);
                var claimsPrincipal = Generics.Generics.SetClaimPrincipal(claims);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch(Exception ex) 
            {
                string exm=ex.Message;
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
                var UserId = userSession.Id;
               
                await userAccountService.SendCurrentUserToServer(UserId);
                await localStorageService.SetItemAsStringAsync("token", token);
            }
            else
            {
                await userAccountService.ClearUserInServer();
                claimsPrincipal = anonymous;
                await localStorageService.RemoveItemAsync("token");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
