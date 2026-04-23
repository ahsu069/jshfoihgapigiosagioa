using Lexa.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Lexa.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IHostEnvironment _env;

        public AuthController(ApiService apiService, IHostEnvironment env)
        {
            _apiService = apiService;
            _env = env;
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
                // return RedirectToAction("Index", "Home");
                return StatusCode(204);

            return View();
        } 

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            // if (!ModelState.IsValid)
            //     return View(model);
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var response = await _apiService.PostAsync("api/Auth/login", model);
            if (!response.IsSuccessStatusCode)
            {
                // TempData["Error"] = await _apiService.GetErrorMessage(response);
                // return View(model);
                return await _apiService.HandleApiResponse(response);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
            // var json = result?.Data?.FirstOrDefault();
            var tokenData = result?.Data;
            if (tokenData == null)
            {
                // TempData["Error"] = "Respons server invalid.";
                // return View(model);
                return new ObjectResult(new { message = "Respons server: data kosong!"})
                {
                    StatusCode = 400
                };
            }

            DateTimeOffset expiresUtcAccessToken = tokenData.AccessTokenExpiresIn ?? DateTimeOffset.UtcNow.AddMinutes(15);
            // if (_env.IsDevelopment())
            //     expiresUtcAccessToken = DateTimeOffset.UtcNow.AddSeconds(45);

            DateTimeOffset expiresUtcRefreshToken = tokenData.RefreshTokenExpiresIn ??
                                                    DateTimeOffset.UtcNow.AddDays(7);

            // var claims = new List<Claim>
            // {
            //     new Claim(ClaimTypes.Name, model.Username),
            //     new Claim("AccessToken", tokenData.AccessToken),
            //     new Claim("RefreshToken", tokenData.RefreshToken),
            //     new Claim("AccessTokenExpiresIn", expiresUtcAccessToken.UtcDateTime.ToString("O")),
            //     new Claim("RefreshTokenExpiresIn", expiresUtcRefreshToken.UtcDateTime.ToString("O")),
            // };

            var claims = new List<Claim> {};

            // Fetch profile user data
            var profileResponse = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Profile", token: tokenData.AccessToken);
            if (!profileResponse.IsSuccessStatusCode)
            {
                // TempData["Error"] = "Respons server: gagal fetch profile.";
                // return View(model);
                return await _apiService.HandleApiResponse(profileResponse);
            }

            var profileContent = await profileResponse.Content.ReadFromJsonAsync<ApiResponse<ProfileDto>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var profile = profileContent?.Data;
            if (profile != null)
            {
                // Add profile info as claims (or store in session if too big)
                claims.Add(new Claim("Username", model.Username));
                claims.Add(new Claim("UserId", profile.User_Id));
                // Username = nama profil user, di database. Seharusnya tidak begitu
                claims.Add(new Claim("UserNama", profile.Nama ?? ""));
                claims.Add(new Claim("BagianId", profile.BagianUserDto.Bagian_Id.ToString() ?? ""));
                claims.Add(new Claim("BagianNama", profile.BagianUserDto.Nama ?? ""));
                claims.Add(new Claim("FungsiId", profile.FungsiUserDto?.Fungsi_Id.ToString() ?? ""));
                claims.Add(new Claim("FungsiNama", profile.FungsiUserDto?.Nama ?? ""));
                claims.Add(new Claim("RoleId", profile.RoleDto?.Role_Id ?? ""));
                claims.Add(new Claim("RoleCode", profile.RoleDto?.Code ?? ""));
                claims.Add(new Claim("RoleNama", profile.RoleDto?.Name ?? ""));
                claims.Add(new Claim("AccessToken", tokenData.AccessToken));
                claims.Add(new Claim("RefreshToken", tokenData.RefreshToken));
                claims.Add(new Claim("AccessTokenExpiresIn", expiresUtcAccessToken.UtcDateTime.ToString("O")));
                claims.Add(new Claim("RefreshTokenExpiresIn", expiresUtcRefreshToken.UtcDateTime.ToString("O")));

                // List permissions disimpan di Claims 
                if (profile.PermissionDto != null)
                {
                    foreach (var permission in profile.PermissionDto)
                    {
                        claims.Add(new Claim("Permission", permission.Code));
                    }
                }
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                ExpiresUtc = expiresUtcRefreshToken.UtcDateTime,
                IsPersistent = true
            });

            foreach (var cookieName in new[] { "AccessToken", "RefreshToken", "AccessTokenExpiresIn" })
            HttpContext.Response.Cookies.Delete(cookieName);

            HttpContext.Response.Cookies.Append("AccessToken", tokenData.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = expiresUtcRefreshToken.UtcDateTime
            });

            HttpContext.Response.Cookies.Append("RefreshToken", tokenData.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = expiresUtcRefreshToken.UtcDateTime
            });

            HttpContext.Response.Cookies.Append("AccessTokenExpiresIn", expiresUtcAccessToken.UtcDateTime.ToString("O"), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = expiresUtcRefreshToken.UtcDateTime
            });

            // return RedirectToAction("Index", "Home");
            return StatusCode(200);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // var username = User.Identity?.Name;
            // var payload = new { username };
            // var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Auth/logout", payload);
            // var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Auth/logout");

            // if (!response.IsSuccessStatusCode)
            // {
            //     TempData["Error"] = "Sesi anda habis, silahkan login kembali.";
            //     return RedirectToAction("Login", "Auth");
            // }

            try
            {
                // Call API logout
                var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Auth/logout");

                // If API fail → user session likely expired
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Sesi anda habis, silahkan login kembali.";
                }
                else
                {
                    TempData["Success"] = "Anda berhasil logout.";
                }
            }
            catch (Exception)
            {
                // Optional: log the error
                // _logger.LogError(ex, "Logout error");

                TempData["Error"] = "Terjadi kesalahan saat logout. Silahkan coba lagi.";
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            HttpContext.Session.Clear();

            // TempData["Success"] = "Anda berhasil logout.";
            return RedirectToAction("Login", "Auth");
        }
    }
}