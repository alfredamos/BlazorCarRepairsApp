using System.Security.Claims;
using BlazorCarRepairsApp.Contracts;
using BlazorCarRepairsApp.Models;
using BlazorCarRepairsApp.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorCarRepairsApp.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class CustomerOwnerOrAdminAttribute(string routeParameter = "customerId") : Attribute, IAuthorizationFilter
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
        
        //----> Retrieve the route-parameter.
        var resourceOwnerId = context.RouteData.Values[routeParameter]?.ToString();
        
        if (!Guid.TryParse(resourceOwnerId, out var idOfCustomer))
        {
            context.Result = new BadRequestObjectResult($"Missing required route parameter: {routeParameter}");
            return;
        }
        
        //----> Get the customer object.
        var customerRepo = context.HttpContext.RequestServices.GetRequiredService<ICustomerRepo>();
        var userIdFromCustomer = (customerRepo.GetCustomerById(idOfCustomer).Result).UserId;

        //----> Get user-id from claim-types.
        var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        //----> Owner check:
        var isOwner = userIdFromCustomer.ToString().Equals(currentUserId, StringComparison.OrdinalIgnoreCase);
        if (!isOwner)
        {
            context.Result = new ForbidResult();
        }

    }
}