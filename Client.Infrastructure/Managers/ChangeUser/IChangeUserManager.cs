using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers.ChangeUser
{
    public interface IChangeUserManager : IManager
    {
        public Task<IResult> ChangeUser();

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
    }
}
