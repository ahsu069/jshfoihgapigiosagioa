using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class SuperAdminBypassHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var roleClaim = context.User.FindFirst("roleNama")?.Value;

        // If user is SuperAdmin, grant access to everything
        if (string.Equals(roleClaim, "SuperAdmin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(roleClaim, "Super Admin", StringComparison.OrdinalIgnoreCase))
        {
            foreach (var requirement in context.PendingRequirements.ToList())
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
