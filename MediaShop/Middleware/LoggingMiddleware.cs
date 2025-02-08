using System.Diagnostics;

namespace MediaShop.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var response = context.Response;

        var sw = Stopwatch.StartNew();

        _logger.LogInformation("Received {Method} request for {Path} from IP {IPAddress}", request.Method, request.Path, context.Connection.RemoteIpAddress);

        request.EnableBuffering();
        var requestBodyStream = new MemoryStream();
        await request.Body.CopyToAsync(requestBodyStream);
        request.Body.Position = 0;
        var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
        _logger.LogInformation("Request Content: {Content}", requestBodyText);

        var originalBodyStream = response.Body;

        using var responseBody = new MemoryStream();
        response.Body = responseBody;

        await _next(context);

        responseBody.Position = 0;
        var responseBodyText = new StreamReader(responseBody).ReadToEnd();
        _logger.LogInformation("Response Content: {Content}", responseBodyText);
        responseBody.Position = 0;

        sw.Stop();

        _logger.LogInformation("Response: {StatusCode}, Elapsed Time: {ElapsedTime} ms", response.StatusCode, sw.ElapsedMilliseconds);

        await responseBody.CopyToAsync(originalBodyStream);
    }
}
