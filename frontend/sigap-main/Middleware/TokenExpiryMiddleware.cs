using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class TokenExpiryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHostEnvironment _env;

    public TokenExpiryMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, IHostEnvironment env)
    {
        _next = next;
        _scopeFactory = scopeFactory;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var expiryCookie = context.Request.Cookies["AccessTokenExpiresIn"];

                if (!string.IsNullOrEmpty(expiryCookie) &&
                    DateTimeOffset.TryParse(expiryCookie, out var expiryDto) &&
                    DateTimeOffset.UtcNow >= expiryDto.ToUniversalTime())
                {
                    using var scope = _scopeFactory.CreateScope();
                    var apiService = scope.ServiceProvider.GetRequiredService<ApiService>();

                    var refreshed = await apiService.TryRefreshTokenAsync();

                    if (!refreshed.Success)
                    {
                        if (_env.IsDevelopment())
                        {
                            Console.WriteLine("[DEV MODE] Token expired but running in Development — skipping redirect/signout.");
                            await _next(context);
                            return;
                        }

                        context.Response.Redirect("/login?reason=expired");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TokenExpiryMiddleware] Exception: {ex.Message}");
        }

        await _next(context);
    }
}

public static class TokenExpiryMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenExpiryCheck(this IApplicationBuilder builder)
        => builder.UseMiddleware<TokenExpiryMiddleware>();
}
