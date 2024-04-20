using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace Server.Services
{
    internal class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor httpContextAccesor;

        public TenantService(IHttpContextAccessor httpContextAccesor)
        {
            this.httpContextAccesor = httpContextAccesor;
        }

        public string GetTenantId()
        {
            var httpcontext = httpContextAccesor.HttpContext;
            if (httpcontext is null)
            {
                return string.Empty;
            }
            var authTicket = DecrypAuthCookie(httpcontext);
            if (authTicket is null)
            {
                return string.Empty;
            }
            var claimtenant = authTicket.Principal.
                Claims.FirstOrDefault(x => x.Type == Constants.ClaimTenantId);
            if (claimtenant is null)
                return string.Empty;
            return claimtenant.Value;
        }
        private static AuthenticationTicket? DecrypAuthCookie(HttpContext httpcontext)
        {
            var opt = httpcontext.RequestServices
                .GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>()
                .Get("Identity.Application");
            var cookie = opt.CookieManager.GetRequestCookie(httpcontext, opt.Cookie.Name!);

            return opt.TicketDataFormat.Unprotect(cookie);
        }
    }
}
