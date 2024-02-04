using Client.Identity.Models;
using Shared.Commons.Results;
using Shared.Models.Requests;
using Shared.Models.Responses;
using System.Threading.Tasks;

namespace Client.Identity
{
    /// <summary>
    /// Account management services.
    /// </summary>
    public interface ICookieAuthenticationStateProvider
    {
      
        Task<IResult<LoginResponse>> LoginAsync(LoginRequest loginRequest);

        /// <summary>
        /// Log out the logged in user.
        /// </summary>
        /// <returns>The asynchronous task.</returns>
        public Task LogoutAsync();

        /// <summary>
        /// Registration service.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
        public Task<IResult<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest);

        public Task<bool> CheckAuthenticatedAsync();
    }
}
