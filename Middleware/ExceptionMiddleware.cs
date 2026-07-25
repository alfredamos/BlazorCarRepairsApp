using System.Text.Json;
using BlazorCarRepairsApp.Exceptions;

namespace BlazorCarRepairsApp.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CustomException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.StatusCode;

            var response = new 
            { 
                message = ex.Message, 
                statusCode = ex.StatusCode 
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}