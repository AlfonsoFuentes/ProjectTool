using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    //public class CurrentUser /*: ICurrentUser*/
    //{
    //    public string UserId { get; set; } = string.Empty;
    //    public string UserName { get; set; } = string.Empty;
    //    public string Role { get; set; } = string.Empty;

    //    public bool IsSuperAdmin => Role.Contains("Administrator");
    //    public bool IsViewer => Role.Contains(RolesEnum.ViewerUser.Name);
    //    public bool IsRegularUser => Role.Contains(RolesEnum.RegularUser.Name) || Role.Contains("SuperAdmin");

    //}
    //internal sealed class UserContext
    //: IUserContext
    //{
    //    IHttpContextAccessor httpContextAccessor;
    //    public UserContext(IHttpContextAccessor httpContextAccessor)
    //    {
    //        this.httpContextAccessor = httpContextAccessor;
    //    }
    //    public Guid CurrentUserId =>
    //        httpContextAccessor
    //            .HttpContext?
    //            .User
    //            .GetUserId() ??
    //        throw new ApplicationException("User context is unavailable");

    //    public bool IsAuthenticated =>
    //        httpContextAccessor
    //            .HttpContext?
    //            .User
    //            .Identity?
    //            .IsAuthenticated ??
    //        throw new ApplicationException("User context is unavailable");

    //    public string sUserId => CurrentUserId.ToString();
    //}
    //internal static class ClaimsPrincipalExtensions
    //{
    //    public static Guid GetUserId(this ClaimsPrincipal? principal)
    //    {
    //        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    //        return Guid.TryParse(userId, out Guid parsedUserId) ?
    //            parsedUserId :
    //            throw new ApplicationException("User id is unavailable");
    //    }
    //}
}
