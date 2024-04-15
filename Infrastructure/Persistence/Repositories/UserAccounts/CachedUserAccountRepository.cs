using Application.Interfaces;
using Domain.Entities.Account;
using Microsoft.Extensions.Caching.Memory;
using Shared.Commons.Results;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;

namespace Infrastructure.Persistence.Repositories.UserAccounts
{
    public class CachedUserAccountRepository : IUserAccountRepository
    {
        private UserAccountRepository _decorated { get; set; }
        private IMemoryCache memoryCache { get; set; }

        public CachedUserAccountRepository(IMemoryCache memoryCache, UserAccountRepository decorated)
        {
            this.memoryCache = memoryCache;
            _decorated = decorated;
        }

        public Task<IResult<RegisterUserResponse>> RegisterUser(RegisterUserRequest Data)=>_decorated.RegisterUser(Data);

        public Task<IResult<LoginUserResponse>> LoginUser(LoginUserRequest Data)=>_decorated.LoginUser(Data);

        public Task<IResult<RegisterUserResponse>> RegisterSuperAdminUser(RegisterSuperAdminUserRequest userDTO)=>_decorated.RegisterSuperAdminUser(userDTO);

        public Task<IResult<UserReponseList>?> GetUserList()
        {
            string Key = $"UserAccount-GetUserList";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.GetUserList();
                }
                );

 
        }

        public Task<bool> ResetPassword(string email)=>_decorated.ResetPassword(email);

        public Task<bool> ValidateIfPasswordConfirmed(string email)
        {
            string Key = $"UserAccount-ValidateIfPasswordConfirmed-{email}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ValidateIfPasswordConfirmed(email);
                }
                );
        }

        public Task<AplicationUser?> ValidateIfEmailExist(string email)
        {
            string Key = $"UserAccount-ValidateIfEmailExist-{email}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ValidateIfEmailExist(email);
                }
                );
        }

        public Task<IResult<UserReponse>> ChangePasswordUser(ChangePasswordUserRequest Data)=>_decorated.ChangePasswordUser(Data);

        public Task<bool> ValidateIfPasswordMatch(string email, string password)
        {
            string Key = $"UserAccount-ValidateIfPasswordMatch-{email}-{password}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ValidateIfPasswordMatch(email,password);
                }
                );
        }
    }
}
