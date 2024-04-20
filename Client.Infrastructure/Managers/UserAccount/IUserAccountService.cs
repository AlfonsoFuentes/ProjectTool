﻿using Azure.Core;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserAccounts.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers.UserAccount
{
    public interface IUserAccountService : IManager
    {
        Task<IResult<RegisterUserResponse>> RegisterUser(RegisterUserRequest request);
        Task<IResult<LoginUserResponse>> LoginUser(LoginUserRequest request);
        Task<IResult<RegisterUserResponse>> RegisterSuperAdminUser(RegisterSuperAdminUserRequest request);
        Task<IResult<UserReponseList>> GetUserList();
        Task<IResult<UserReponse>> ValidatePasswordMatch(LoginUserRequest request);
        Task<IResult<UserReponse>> ValidateIfEmailExist(string email);
        Task<IResult<UserReponse>> ValidateIfPasswordConfirmed(string email);
        Task<IResult<UserReponse>> ResetPassword(string email);
        Task<IResult<UserReponse>> ChangePasswordUser(ChangePasswordUserRequest request);
        Task<IResult> SendCurrentUserToServer(string UserId);
        Task<IResult> ClearUserInServer();
    }
    public class UserAccountService : IUserAccountService
    {
        private HttpClient Http;

        public UserAccountService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IResult<RegisterUserResponse>> RegisterUser(RegisterUserRequest request)
        {
          
            var httpresult = await Http.PostAsJsonAsync("UserAccount/RegisterUser", request);

            return await httpresult.ToResult<RegisterUserResponse>();
        }

        public async Task<IResult<LoginUserResponse>> LoginUser(LoginUserRequest request)
        {
         
            var httpresult = await Http.PostAsJsonAsync("UserAccount/LoginUser", request);

            return await httpresult.ToResult<LoginUserResponse>();
        }
        public async Task<IResult<RegisterUserResponse>> RegisterSuperAdminUser(RegisterSuperAdminUserRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync("UserAccount/RegisterSuperAdminUser", request);

            return await httpresult.ToResult<RegisterUserResponse>();
        }

        public async Task<IResult<UserReponseList>> GetUserList()
        {
            var httpresult = await Http.GetAsync($"UserAccount/GetAll");
            return await httpresult.ToResult<UserReponseList>();
        }

        public async Task<IResult<UserReponse>> ValidateIfEmailExist(string email)
        {
            var httpresult = await Http.GetAsync($"UserAccount/ValidateEmailExist/{email}");
            return await httpresult.ToResult<UserReponse>();
        }

       
        public async Task<IResult<UserReponse>> ResetPassword(string email)
        {
            var httpresult = await Http.PostAsJsonAsync($"UserAccount/ResetPassword", email);

            return await httpresult.ToResult<UserReponse>();
        }

        public async Task<IResult<UserReponse>> ChangePasswordUser(ChangePasswordUserRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"UserAccount/ChangePassword", request);

            return await httpresult.ToResult<UserReponse>();
        }

        

        public async Task<IResult<UserReponse>> ValidatePasswordMatch(LoginUserRequest request)
        {
          
            var httpresult = await Http.PostAsJsonAsync($"UserAccount/ValidatePasswordMatch", request);
            return await httpresult.ToResult<UserReponse>();
        }

        public async Task<IResult> SendCurrentUserToServer(string UserId)
        {
           var httpresult2 = await Http.GetAsync($"UserAccount/GetCurrentUser/{UserId}");
            return await httpresult2.ToResult<bool>();
        }
        public async Task<IResult> ClearUserInServer()
        {
            var httpresult2 = await Http.GetAsync($"UserAccount/ClearCurrentUser");
            return await httpresult2.ToResult<bool>();
        }
        public async Task<IResult<UserReponse>> ValidateIfPasswordConfirmed(string email)
        {
            var httpresult = await Http.GetAsync($"UserAccount/ValidatePasswordConfirmed/{email}");
            return await httpresult.ToResult<UserReponse>();
        }

    }
}
