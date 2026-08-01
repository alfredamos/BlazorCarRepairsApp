using System.Security.Claims;
using BlazorCarRepairsApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorCarRepairsApp.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class UserOwnerOrAdminAttribute(string routeParameter = "userId") : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //----> Get the current user.
        var user = context.HttpContext.User;
        
        //----> check for authentication.
        if (user.Identity is { IsAuthenticated: false })
        {
            context.Result = new UnauthorizedObjectResult("Invalid credentials");
            return;
        }
        
        //----> Check for admin.
        if (user.IsInRole(Roles.Admin))
        {
            return;
        }
        
        //----> Retrieve the route parameter.
        var resourceOwnerId = context.RouteData.Values[routeParameter]?.ToString();
        if (!Guid.TryParse(resourceOwnerId, out var idOfUser))
        {
            context.Result = new BadRequestObjectResult("Invalid route parameter");
        }
        
        //----> Get the current user id.
        var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        //----> Check for ownership.
        var isOwner = idOfUser.ToString().Equals(currentUserId);
        if (!isOwner)
        {
            context.Result = new ForbidResult();
        }
    }
}