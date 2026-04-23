using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers
{
    public class DebugController : Controller
    {
#if DEBUG
        [HttpGet("debug/data")]
        [AllowAnonymous]
        public IActionResult DebugData()
        {
            // Prevent running in production
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env?.Equals("Production", StringComparison.OrdinalIgnoreCase) == true)
                return RedirectToAction("Error403", "Error");

            // Extract commonly used claims
            // var username = User.Identity?.Name;
            var username = User.FindFirst("Username")?.Value;
            var userId = User.FindFirst("UserId")?.Value;
            var userNama = User.FindFirst("UserNama")?.Value;
            var bagianId = User.FindFirst("BagianId")?.Value;
            var bagianNama = User.FindFirst("BagianNama")?.Value;
            var fungsiId = User.FindFirst("FungsiId")?.Value;
            var fungsiNama = User.FindFirst("FungsiNama")?.Value;
            var roleId = User.FindFirst("RoleId")?.Value;
            var roleCode = User.FindFirst("RoleCode")?.Value;
            var roleNama = User.FindFirst("RoleNama")?.Value;

            // Token info
            var accessToken = User.FindFirst("AccessToken")?.Value;
            var refreshToken = User.FindFirst("RefreshToken")?.Value;
            var accessTokenExpiresIn = User.FindFirst("AccessTokenExpiresIn")?.Value;
            var refreshTokenExpiresIn = User.FindFirst("RefreshTokenExpiresIn")?.Value;
            var accessTokenCookie = HttpContext.Request.Cookies["AccessToken"];
            var refreshTokenCookie = HttpContext.Request.Cookies["RefreshToken"];
            var accessTokenExpiresInCookie = HttpContext.Request.Cookies["AccessTokenExpiresIn"];
            // var refreshTokenExpiresInCookie = HttpContext.Request.Cookies["RefreshTokenExpiresIn"];

            // All permission claims (you store each as "Permission")
            var permissions = User.Claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            object? decodedPayload = null;

            // Decode JWT payload (Access Token)
            if (!string.IsNullOrEmpty(accessToken) && accessToken.Split('.').Length == 3)
            {
                try
                {
                    var payload = accessToken.Split('.')[1];
                    var padded = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                    var jsonBytes = Convert.FromBase64String(padded.Replace('-', '+').Replace('_', '/'));
                    var jsonString = Encoding.UTF8.GetString(jsonBytes);
                    decodedPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);
                }
                catch
                {
                    decodedPayload = new { error = "Invalid JWT format" };
                }
            }

            // List all claims for full debug visibility
            var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Json(new
            {
                environment = env,
                username,
                userId,
                userNama,
                bagianId,
                bagianNama,
                fungsiId,
                fungsiNama,
                roleId,
                roleCode,
                roleNama,
                permissions,
                accessToken,
                accessTokenExpiresIn,
                refreshToken,
                refreshTokenExpiresIn,
                accessTokenCookie,
                accessTokenExpiresInCookie,
                refreshTokenCookie,
                // refreshTokenExpiresInCookie,
                decodedAccessToken = decodedPayload,
                allClaims
            });
        }
#endif
    }
}
