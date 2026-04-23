using System;
using System.Security.Claims;

namespace Lexa.Helpers
{
    public static class SuperAdminBypassHelper
    {
        public static bool IsSuperAdmin(this ClaimsPrincipal user)
        {
            if (user == null) return false;

            // adapt claim name if you use different claim type
            var roleClaim = user.FindFirst("roleNama")?.Value;
            if (string.IsNullOrWhiteSpace(roleClaim)) return false;

            return string.Equals(roleClaim, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(roleClaim, "Super Admin", StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasPermissionOrSuper(this ClaimsPrincipal user, string permission)
        {
            if (user == null) return false;
            if (user.IsSuperAdmin()) return true;          // <-- bypass for superadmin
            return user.HasClaim("permission", permission);
        }
    }
}
