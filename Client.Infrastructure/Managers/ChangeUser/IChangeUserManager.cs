﻿namespace Client.Infrastructure.Managers.ChangeUser
{
    public interface IChangeUserManager : IManager
    {
        public Task<IResult> ChangeUser();
        public Task<IResult> UpdateDataForMWO();
        public Task<IResult> UpdateTenant();

    }
    public class ChangeUserManager:IChangeUserManager 
    {
        private HttpClient Http;

        public ChangeUserManager(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }
        public async Task<IResult> ChangeUser()
        {
            var httpresult = await Http.PostAsync("ChangeUser/ChangeUser", null);

            return await httpresult.ToResult();
        }

        public async Task<IResult> UpdateDataForMWO()
        {
            var httpresult = await Http.PostAsync("ChangeUser/UpdateDataForMWOs", null);

            return await httpresult.ToResult();
        }

        public async Task<IResult> UpdateTenant()
        {
            var httpresult = await Http.PostAsync("ChangeUser/UpdateTenant", null);

            return await httpresult.ToResult();
        }
    }
}
