using Client.Identity.Models;
using Client.Pages.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.Requests;
using Shared.Models.Responses;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;


namespace Client.Identity
{
    /// <summary>
    /// Handles state for cookie-based auth.
    /// </summary>
    public class CookieAuthenticationStateProvider : AuthenticationStateProvider, ICookieAuthenticationStateProvider
    {
        private CurrentUser _currentUser;
        /// <summary>
        /// Map the JavaScript-formatted properties to C#-formatted classes.
        /// </summary>
        private readonly JsonSerializerOptions jsonSerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

        /// <summary>
        /// Special auth client.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Authentication state.
        /// </summary>
        private bool _authenticated = false;

        /// <summary>
        /// Default principal for anonymous (not authenticated) users.
        /// </summary>
        private readonly ClaimsPrincipal Unauthenticated =
            new(new ClaimsIdentity());

        /// <summary>
        /// Create a new instance of the auth provider.
        /// </summary>
        /// <param name="httpClientFactory">Factory to retrieve auth client.</param>
        public CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory, CurrentUser currentUser)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
            _currentUser = currentUser;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result serialized to a <see cref="FormResult"/>.
        /// </returns>
        async Task<IResult<RegisterResponse>> GetRegisterFromController(RegisterRequest registerRequest)
        {
            var result = await _httpClient.PostAsJsonAsync(
                   "Account/register/", registerRequest);
            var retorno = await result.ToResult<RegisterResponse>();
            return retorno.Succeeded ? Result<RegisterResponse>.Success(retorno.Data) : Result<RegisterResponse>.Fail();
        }
        public async Task<IResult<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest)
        {
            string[] defaultDetail = ["An unknown error prevented registration from succeeding."];

            try
            {
                // make the request
                var result = await _httpClient.PostAsJsonAsync(
                    "Account/register/", registerRequest);

                // successful?
                if (result.IsSuccessStatusCode)
                {
                    return Result<RegisterResponse>.Success();
                }

                // body should contain details about why it failed
                var details = await result.Content.ReadAsStringAsync();
                var problemDetails = JsonDocument.Parse(details);
                var errors = new List<string>();
                var errorList = problemDetails.RootElement.GetProperty("errors");

                foreach (var errorEntry in errorList.EnumerateObject())
                {
                    if (errorEntry.Value.ValueKind == JsonValueKind.String)
                    {
                        errors.Add(errorEntry.Value.GetString()!);
                    }
                    else if (errorEntry.Value.ValueKind == JsonValueKind.Array)
                    {
                        errors.AddRange(
                            errorEntry.Value.EnumerateArray().Select(
                                e => e.GetString() ?? string.Empty)
                            .Where(e => !string.IsNullOrEmpty(e)));
                    }
                }

                // return the error list
                return Result<RegisterResponse>.Fail(problemDetails == null ? defaultDetail : [.. errors]);

            }
            catch (Exception ex)
            {
                string exm = ex.Message;
            }
            return Result<RegisterResponse>.Fail(defaultDetail);
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The result of the login request serialized to a <see cref="FormResult"/>.</returns>
        async Task<IResult<LoginResponse>> GetLoginController(LoginRequest loginRequest)
        {
            var result = await _httpClient.PostAsJsonAsync(
                   "Account/login/", loginRequest);
            var retorno = await result.ToResult<LoginResponse>();
            return retorno.Succeeded ? Result<LoginResponse>.Success(retorno.Data) : Result<LoginResponse>.Fail();
        }
        public async Task<IResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {

            try
            {
                // login with cookies

                var result = await GetLoginController(loginRequest);
                // success?
                if (result.Succeeded)
                {
                    // need to refresh auth state
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                    // success!
                    return Result<LoginResponse>.Success(result.Data);
                }
            }
            catch { }

            // unknown error
            return Result<LoginResponse>.Fail("Invalid email and/or password.");
        }

        /// <summary>
        /// Get authentication state.
        /// </summary>
        /// <remarks>
        /// Called by Blazor anytime and authentication-based decision needs to be made, then cached
        /// until the changed state notification is raised.
        /// </remarks>
        /// <returns>The authentication state asynchronous request.</returns>
        async Task<IResult> DefinCurrentUserServer(string email)
        {
            var result = await _httpClient.GetAsync(
                   $"Account/DefineCurrentUser?email={email}");
            var retorno = await result.ToResult<CurrentUser>();
            if (retorno.Succeeded)
            {
                _currentUser.UserId = retorno.Data.UserId;
                _currentUser.Roles = retorno.Data.Roles;
                _currentUser.UserName = retorno.Data.UserName;
                return Result.Success();
            }
            return Result.Fail();
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;

            // default to not authenticated
            var user = Unauthenticated;

            try
            {
                // the user info endpoint is secured, so if the user isn't logged in this will fail
                var userResponse = await _httpClient.GetAsync("manage/info");

                // throw if user info wasn't retrieved
                userResponse.EnsureSuccessStatusCode();

                // user is authenticated,so let's build their authenticated identity
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);
                
                if (userInfo != null && !string.IsNullOrEmpty(userInfo!.Email))
                {
                    await DefinCurrentUserServer(userInfo!.Email);
                    // in our system name and email are the same
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, userInfo.Email),
                        new(ClaimTypes.Email, userInfo.Email),
                        new(ClaimTypes.Role, userInfo.Role)
                    };

                    // add any additional claims
                    claims.AddRange(
                        userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                            .Select(c => new Claim(c.Key, c.Value)));

                    // set the principal
                    var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                    user = new ClaimsPrincipal(id);
                    _authenticated = true;
                }
            }
            catch (Exception ex) 
            { 
            string message = ex.Message;
            }

            // return the state
            return new AuthenticationState(user);
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("Account/logout", null);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }
    }
    }
