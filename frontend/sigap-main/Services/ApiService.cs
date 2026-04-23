using System.Net.Http.Headers;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

public class ApiService : Controller
{
    private static readonly SemaphoreSlim _refreshLock = new(1, 1);
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;

    public ApiService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IConfiguration configuration, IHostEnvironment env)
    {
        _httpClient = httpClient;
        _contextAccessor = contextAccessor;
        _configuration = configuration;
        _env = env;

        //var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL")
        //                ?? _configuration["ApiSettings:BaseUrl"]
        //                ?? "http://localhost:5000";

        var baseUrl = "http://localhost:5000";

        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, object payload)
        => await _httpClient.PostAsJsonAsync(endpoint, payload);

    // public async Task<HttpResponseMessage> SendAuthorizedAsync(HttpMethod method, string endpoint, object? body = null, string? token = null, bool isMultipart = false)
    // {
    //     // var accessToken = token ?? _contextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;
    //     var ctx = _contextAccessor.HttpContext;
    //     var accessToken = token 
    //         ?? ctx?.Request.Cookies["AccessToken"];

    //     HttpRequestMessage MakeRequest(string tk)
    //     {
    //         var req = new HttpRequestMessage(method, endpoint);
    //         if (!string.IsNullOrEmpty(tk))
    //             req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tk);

    //         if (body != null)
    //         {
    //             if (isMultipart && body is MultipartFormDataContent multipart)
    //                 req.Content = multipart;
    //             else
    //                 req.Content = JsonContent.Create(body);
    //         }

    //         return req;
    //     }

    //     var request = MakeRequest(accessToken);
    //     var response = await _httpClient.SendAsync(request);

    //     if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    //     {
    //         var refreshed = await TryRefreshTokenAsync();
    //         if (refreshed)
    //         {
    //             // var newAccessToken = _contextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;
    //             var newAccessToken = ctx?.Request.Cookies["AccessToken"];
    //             var retry = MakeRequest(newAccessToken);
    //             response = await _httpClient.SendAsync(retry);
    //         }
    //     }

    //     return response;
    // }

    public async Task<HttpResponseMessage> SendAuthorizedAsync(
        HttpMethod method, string endpoint, object? body = null, string? token = null, bool isMultipart = false)
    {
        var ctx = _contextAccessor.HttpContext;
        var accessToken = token ?? ctx?.Request.Cookies["AccessToken"];

        HttpRequestMessage MakeRequest(string? tk)
        {
            var req = new HttpRequestMessage(method, endpoint);
            if (!string.IsNullOrEmpty(tk))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tk);

            // if (body != null)
            // {
            //     if (isMultipart && body is MultipartFormDataContent multipart)
            //         req.Content = multipart;
            //     else
            //         req.Content = JsonContent.Create(body);
            // }

            // if (body != null)
            // {
            //     if (isMultipart)
            //     {
            //         if (body is MultipartFormDataContent multipart)
            //         {
            //             req.Content = multipart;
            //         }
            //         else
            //         {
            //             // Convert a POCO object to multipart content
            //             var form = new MultipartFormDataContent();
            //             foreach (var prop in body.GetType().GetProperties())
            //             {
            //                 var value = prop.GetValue(body);
            //                 if (value == null) continue;
            //                 form.Add(new StringContent(value.ToString()), prop.Name);
            //             }
            //             req.Content = form;
            //         }
            //     }
            //     else
            //     {
            //         req.Content = JsonContent.Create(body);
            //     }
            // }

            if (body != null)
            {
                // Case 1: already MultipartFormDataContent
                if (isMultipart && body is MultipartFormDataContent multipart)
                {
                    req.Content = multipart;
                }
                // Case 2: body is IFormCollection (from controller)
                else if (isMultipart && body is IFormCollection form)
                {
                    var formContent = new MultipartFormDataContent();

                    foreach (var key in form.Keys)
                    {
                        foreach (var value in form[key])
                            formContent.Add(new StringContent(value), key);
                    }

                    foreach (var file in form.Files)
                    {
                        if (file.Length > 0)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream());
                            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                            formContent.Add(fileContent, file.Name, file.FileName);
                        }
                    }

                    req.Content = formContent;
                }
                // Case 3: regular POCO object converted to multipart
                else if (isMultipart)
                {
                    var formContent = new MultipartFormDataContent();

                    foreach (var prop in body.GetType().GetProperties())
                    {
                        var value = prop.GetValue(body);
                        if (value == null) continue;
                        formContent.Add(new StringContent(value.ToString()), prop.Name);
                    }

                    req.Content = formContent;
                }
                // Case 4: JSON payload
                else
                {
                    req.Content = JsonContent.Create(body);
                }
            }

            return req;
        }

        var request = MakeRequest(accessToken);
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Try to refresh tokens
            var refreshResult = await TryRefreshTokenAsync();
            if (refreshResult.Success && !string.IsNullOrEmpty(refreshResult.AccessToken))
            {
                // Retry with freshly obtained token
                var retry = MakeRequest(refreshResult.AccessToken);
                response = await _httpClient.SendAsync(retry);
            }
        }

        return response;
    }

    // public async Task<bool> TryRefreshTokenAsync()
    // {
    //     await _refreshLock.WaitAsync();

    //     try
    //     {
    //         var httpContext = _contextAccessor.HttpContext;
    //         if (httpContext == null || httpContext.User.Identity.IsAuthenticated != true)
    //             return false;
            
    //         // ✅ RECHECK: maybe another thread already refreshed while we were waiting
    //         var accessTokenExpiry = httpContext.Request.Cookies["AccessTokenExpiresIn"];
    //         if (DateTimeOffset.TryParse(accessTokenExpiry, out var expiryCheck) &&
    //             DateTimeOffset.UtcNow < expiryCheck)
    //         {
    //             Console.WriteLine("[TryRefreshToken] Another thread already refreshed token — skip refresh.");
    //             return true;
    //         }

    //         var refreshToken = httpContext.Request.Cookies["RefreshToken"];
    //         var refreshPayload = new { refreshToken };

    //         var refreshResponse = await _httpClient.PostAsJsonAsync("api/Auth/refresh", refreshPayload);
    //         if (!refreshResponse.IsSuccessStatusCode)
    //         {
    //             // if (!_env.IsDevelopment())
    //             //     await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //             await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //             return false;
    //         }

    //         LoginResponse? parsed = null;
    //         try
    //         {
    //             var content = await refreshResponse.Content.ReadAsStringAsync();
    //             parsed = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
    //             {
    //                 PropertyNameCaseInsensitive = true,
    //             });
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine("[TryRefreshToken] error: " + ex.Message);
    //             return false;
    //         }

    //         var newTokens = parsed?.Data;
    //         if (newTokens == null)
    //             return false;

    //         DateTimeOffset accessExpiry = newTokens.AccessTokenExpiresIn ?? DateTimeOffset.UtcNow.AddMinutes(15);
    //         // if (_env.IsDevelopment())
    //         //     accessExpiry = DateTimeOffset.UtcNow.AddSeconds(45);

    //         DateTimeOffset refreshExpiryNew = newTokens.RefreshTokenExpiresIn ?? DateTimeOffset.UtcNow.AddDays(7);

    //         var claims = new List<Claim> {};

    //         // Fetch profile user data
    //         var req = new HttpRequestMessage(HttpMethod.Get, "api/Profile");
    //         req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.AccessToken);
    //         var profileResponse = await _httpClient.SendAsync(req);
    //         if (!profileResponse.IsSuccessStatusCode)
    //         {
    //             await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //             return false;
    //         }

    //         var profileContent = await profileResponse.Content.ReadFromJsonAsync<ApiResponse<ProfileDto>>(new JsonSerializerOptions
    //         {
    //             PropertyNameCaseInsensitive = true
    //         });

    //         var profile = profileContent?.Data;
    //         if (profile != null)
    //         {
    //             // Add profile info as claims (or store in session if too big)
    //             claims.Add(new Claim("UserId", profile.User_Id));
    //             // Username = nama profil user, di database. Seharusnya tidak begitu
    //             claims.Add(new Claim("UserNama", profile.Nama ?? ""));
    //             claims.Add(new Claim("BagianNama", profile.BagianUserDto.Nama ?? ""));
    //             claims.Add(new Claim("FungsiNama", profile.FungsiUserDto?.Nama ?? ""));
    //             claims.Add(new Claim("RoleNama", profile.RoleDto?.Name ?? ""));
    //             claims.Add(new Claim("AccessToken", newTokens.AccessToken));
    //             claims.Add(new Claim("RefreshToken", newTokens.RefreshToken));
    //             claims.Add(new Claim("AccessTokenExpiresIn", accessExpiry.UtcDateTime.ToString("O")));
    //             claims.Add(new Claim("RefreshTokenExpiresIn", refreshExpiryNew.UtcDateTime.ToString("O")));

    //             // List permissions disimpan di Claims 
    //             if (profile.PermissionDto != null)
    //             {
    //                 foreach (var permission in profile.PermissionDto)
    //                 {
    //                     claims.Add(new Claim("Permission", permission.Code));
    //                 }
    //             }
    //         }

    //         var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //         var principal = new ClaimsPrincipal(identity);

    //         await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
    //         {
    //             ExpiresUtc = refreshExpiryNew.UtcDateTime,
    //             IsPersistent = true
    //         });

    //         foreach (var cookieName in new[] { "AccessToken", "RefreshToken", "AccessTokenExpiresIn" })
    //         httpContext.Response.Cookies.Delete(cookieName);

    //         httpContext.Response.Cookies.Append("AccessToken", newTokens.AccessToken, new CookieOptions
    //         {
    //             HttpOnly = true,
    //             Secure = true,
    //             SameSite = SameSiteMode.Lax,
    //             Expires = refreshExpiryNew.UtcDateTime
    //         });

    //         httpContext.Response.Cookies.Append("RefreshToken", newTokens.RefreshToken, new CookieOptions
    //         {
    //             HttpOnly = true,
    //             Secure = true,
    //             SameSite = SameSiteMode.Lax,
    //             Expires = refreshExpiryNew.UtcDateTime
    //         });

    //         httpContext.Response.Cookies.Append("AccessTokenExpiresIn", accessExpiry.UtcDateTime.ToString("O"), new CookieOptions
    //         {
    //             HttpOnly = true,
    //             Secure = true,
    //             SameSite = SameSiteMode.Lax,
    //             Expires = refreshExpiryNew.UtcDateTime
    //         });


    //         return true;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("[TryRefreshToken] error: " + ex.Message);
    //         return false;
    //     }
    //     finally
    //     {
    //         _refreshLock.Release();
    //     }
    // }

    public async Task<TokenResult> TryRefreshTokenAsync()
    {
        await _refreshLock.WaitAsync();

        try
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null || httpContext.User.Identity?.IsAuthenticated != true)
                return new TokenResult { Success = false };

            // Prevent redundant refresh if another thread already succeeded
            var expiryRaw = httpContext.Request.Cookies["AccessTokenExpiresIn"];
            if (DateTimeOffset.TryParse(expiryRaw, out var expiry) &&
                DateTimeOffset.UtcNow < expiry)
            {
                Console.WriteLine("[TryRefreshToken] Another thread already refreshed token — skip.");
                return new TokenResult { Success = true, AccessToken = httpContext.Request.Cookies["AccessToken"] };
            }

            var refreshToken = httpContext.Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return new TokenResult { Success = false };

            var refreshPayload = new { refreshToken };
            var refreshResponse = await _httpClient.PostAsJsonAsync("api/Auth/refresh", refreshPayload);
            if (!refreshResponse.IsSuccessStatusCode)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return new TokenResult { Success = false };
            }

            var content = await refreshResponse.Content.ReadAsStringAsync();
            var parsed = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            var newTokens = parsed?.Data;
            if (newTokens == null)
                return new TokenResult { Success = false };

            DateTimeOffset accessExpiry = newTokens.AccessTokenExpiresIn ?? DateTimeOffset.UtcNow.AddMinutes(15);
            DateTimeOffset refreshExpiryNew = newTokens.RefreshTokenExpiresIn ?? DateTimeOffset.UtcNow.AddDays(7);

            // Fetch user profile
            var profileReq = new HttpRequestMessage(HttpMethod.Get, "api/Profile");
            profileReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.AccessToken);
            var profileResponse = await _httpClient.SendAsync(profileReq);

            if (!profileResponse.IsSuccessStatusCode)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return new TokenResult { Success = false };
            }

            var profileContent = await profileResponse.Content.ReadFromJsonAsync<ApiResponse<ProfileDto>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var profile = profileContent?.Data;
            if (profile == null)
                return new TokenResult { Success = false };

            // Build claims
            var claims = new List<Claim>
            {
                new("Username", User.FindFirst("Username")?.Value),
                new("UserId", profile.User_Id),
                new("UserNama", profile.Nama ?? ""),
                new("BagianId", profile.BagianUserDto.Bagian_Id.ToString() ?? ""),
                new("BagianNama", profile.BagianUserDto.Nama ?? ""),
                new("FungsiId", profile.FungsiUserDto?.Fungsi_Id.ToString() ?? ""),
                new("FungsiNama", profile.FungsiUserDto?.Nama ?? ""),
                new("RoleId", profile.RoleDto?.Role_Id ?? ""),
                new("RoleCode", profile.RoleDto?.Code ?? ""),
                new("RoleNama", profile.RoleDto?.Name ?? ""),
                new("AccessToken", newTokens.AccessToken),
                new("RefreshToken", newTokens.RefreshToken),
                new("AccessTokenExpiresIn", accessExpiry.UtcDateTime.ToString("O")),
                new("RefreshTokenExpiresIn", refreshExpiryNew.UtcDateTime.ToString("O"))
            };

            if (profile.PermissionDto != null)
            {
                foreach (var perm in profile.PermissionDto)
                    claims.Add(new Claim("Permission", perm.Code));
            }

            // Re-sign the principal
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                ExpiresUtc = refreshExpiryNew.UtcDateTime,
                IsPersistent = true
            });

            // Replace cookies
            foreach (var cookieName in new[] { "AccessToken", "RefreshToken", "AccessTokenExpiresIn" })
                httpContext.Response.Cookies.Delete(cookieName);

            httpContext.Response.Cookies.Append("AccessToken", newTokens.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = refreshExpiryNew.UtcDateTime
            });

            httpContext.Response.Cookies.Append("RefreshToken", newTokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = refreshExpiryNew.UtcDateTime
            });

            httpContext.Response.Cookies.Append("AccessTokenExpiresIn", accessExpiry.UtcDateTime.ToString("O"), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = refreshExpiryNew.UtcDateTime
            });

            return new TokenResult
            {
                Success = true,
                AccessToken = newTokens.AccessToken,
                RefreshToken = newTokens.RefreshToken
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("[TryRefreshToken] error: " + ex.Message);
            return new TokenResult { Success = false };
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    public async Task<string> GetErrorMessage(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            var json = JsonSerializer.Deserialize<ApiResponse<object>>(content);
            // var json = JsonSerializer.Deserialize<ApiResponse<object>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return json?.Message ?? response.ReasonPhrase ?? "Unknown error.";
        }
        catch
        {
            return content;
        }
    }

    public async Task<Dictionary<string, string[]>> GetValidationErrors(HttpResponseMessage response)
    {
        var res = await response.Content.ReadFromJsonAsync<ApiResponse<object?>>();
        try
        {
            return res?.Errors;
        }
        catch
        {
            return new Dictionary<string, string[]>
            {
                { "General", new[] { "Tidak dapat memproses kesalahan validasi dari server." } }
            };
        }
    }

    public async Task<IActionResult> HandleApiResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                Content = content,
                ContentType = "application/json",
                StatusCode = (int)response.StatusCode
            };
        }

        // var res = await response.Content.ReadAsStringAsync();
        var res = await response.Content.ReadFromJsonAsync<ApiResponse<object?>>();
        return new ObjectResult(new { message = res.Message, errors = res.Errors })
        // return new ObjectResult(new { message = "API call failed", error = err })
        {
            StatusCode = (int)response.StatusCode
        };
    }

}
