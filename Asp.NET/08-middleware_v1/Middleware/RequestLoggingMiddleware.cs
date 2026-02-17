public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Request Path: {context.Request.Path}");

        context.Response.Headers.Add("X-App-Name", "UserApi");

        await _next(context);
    }
}
