using System.Net;

namespace BlazorCarRepairsApp.Exceptions;

public class CustomException(string message, HttpStatusCode statusCode) : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}