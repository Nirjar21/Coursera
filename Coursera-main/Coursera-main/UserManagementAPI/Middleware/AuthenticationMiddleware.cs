namespace UserManagementAPI.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for Swagger endpoints
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(token) || !IsValidToken(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("{\"error\": \"Unauthorized. Valid token required.\"}");
            return;
        }

        await _next(context);
    }

    private bool IsValidToken(string token)
    {
        // Simple validation for demo purposes
        // In production, use JWT or proper authentication
        return token == "Bearer valid-token-123";
    }
}