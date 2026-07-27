using System.Net;

namespace BlazorCarRepairsApp.Dto;

public class ResponseMessage
{
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public HttpStatusCode StatusCode{ get; set; }
}