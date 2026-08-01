using System.Security.Claims;
using BlazorCarRepairsApp.Contracts;
using BlazorCarRepairsApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorCarRepairsApp.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class TechnicianOwnerOrAdminAttribute(string routeParameter = "techId") : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //----> Get the current user.
        var user = context.HttpContext.User;
        
        //----> Check for authentication.
        if (user.Identity is { IsAuthenticated: false })
        {
            context.Result = new UnauthorizedObjectResult("Invalid credentials");
            return;
        }
        
        //----> Check for admin privilege.
        if (user.IsInRole(Roles.Admin))
        {
            return;
        }
        
        //----> Get the route-parameter.
        var resourceOwnerId = context.RouteData.Values[routeParameter]?.ToString();

        if (!Guid.TryParse(resourceOwnerId, out var idOfTech))
        {
            context.Result = new UnauthorizedObjectResult("Invalid credentials");
        }
        
        //----> Get user id from tech-repo.
        var resourceRepo = context.HttpContext.RequestServices.GetRequiredService<ITechnicianRepo>();
        var userIdFromTech = (resourceRepo.GetTechById(idOfTech).Result).UserId;
        
        //----> Get the current user id from claims.
        var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        //----> Check for null id.
        if (currentUserId is null)
        {
            context.Result = new UnauthorizedObjectResult("Invalid credentials");
            return;
        }
        
        //----> Check for ownership.
        var isOwner = userIdFromTech.ToString().Equals(currentUserId);
        if (!isOwner)
        {
            context.Result = new ForbidResult();
        }

    }
}