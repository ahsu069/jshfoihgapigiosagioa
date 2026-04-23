using api.Commons;
using api.Data;
using api.Models;
using api.Models.Mappers;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;
        private readonly IConfiguration _config;
        private string _jwtKey = string.Empty;
        private DateTime _accessTokenExpiry = DateTime.Now;
        private DateTime _refreshTokenExpiry = DateTime.Now;
        // private static Dictionary<string, string> _refreshTokens = new();
        public Dictionary<string, string[]>? Errors { get; set; } = new();
        public AuthController(ApplicationDbContext context, AuthService authService, IConfiguration config)
        {
            _context = context;
            _authService = authService;
            _config = config;
            var jwtSettings = _config.GetSection("JwtSettings");
            _jwtKey = jwtSettings["Key"] ?? throw new ArgumentNullException("JWT Key is not configured.");
            string accessTokenExpiryMinutes = jwtSettings["AccessTokenExpiryMinutes"] ?? "15";
            string refreshTokenExpiryDays = jwtSettings["RefreshTokenExpiryDays"] ?? "7";
            _accessTokenExpiry = DateTime.Now.AddMinutes(int.Parse(accessTokenExpiryMinutes));
            _refreshTokenExpiry = DateTime.Now.AddDays(int.Parse(refreshTokenExpiryDays));
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("One or more validation errors occurred", ValidationHelper.GetErrorDictionary(errors)));

                var SigapUser = _context.SigapUsers.Include(o => o.BagianUserDto).FirstOrDefault(u => u.username == request.username);
                if (SigapUser == null)
                    Errors?.Add("username", new[] { "The field 'username' value is not found." });
                else if(!AuthService.VerifyPassword(request.password, SigapUser?.password))
                {
                    Errors?.Add("password", new[] { "The field 'password' value is invalid."} );
                }
                if (Errors?.Any() == true)
                    return Unauthorized(ApiResponse<object>.Fail("Login failed", Errors));
                
                var claims = _authService.GetClaims(SigapUser.MapToDto());
                var appIdentity = new ClaimsIdentity(claims);

                AuthUser authUserDto = new AuthUser
                {
                    accessToken = _authService.GenerateAccessToken(appIdentity, _jwtKey, _accessTokenExpiry),
                    accessTokenExpiresIn = _accessTokenExpiry,
                    refreshToken = _authService.GenerateRefreshToken(),
                    refreshTokenExpiresIn = _refreshTokenExpiry,
                };
                SigapUser!.refresh_token = authUserDto?.refreshToken;
                _context.SigapUsers.Update(SigapUser);
                _context.SaveChanges();
                // _refreshTokens[request.username] = authUserDto.refreshToken;
                return Ok(ApiResponse<AuthUser>.Ok("Login successfully", authUserDto));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            try
            {
                if (!ValidationHelper.TryValidate(request, out var errors))
                    return StatusCode(400, ApiResponse<object>.Fail("Refresh token failed", ValidationHelper.GetErrorDictionary(errors)));

                var SigapUser = _context.SigapUsers.Include(o => o.BagianUserDto).FirstOrDefault(u => u.refresh_token == request.refreshToken);
                if (SigapUser == null)
                    Errors?.Add("refreshToken", new[] { "The field 'refreshToken' is not found or invallid."} );

                // if (!_refreshTokens.TryGetValue(request.username, out var savedRefreshToken) || savedRefreshToken != request.refreshToken)
                //     Errors?.Add("refreshToken", new[] { "The field 'refreshToken' is invalid."} );
                // var SigapUser = _context.SigapUsers.Include(o => o.BagianUserDto).FirstOrDefault(u => u.nama == request.username);
                // if (SigapUser == null)
                //     Errors?.Add("username", new[] { "The field 'username' value is not found."} );

                if (Errors?.Any() == true)
                    return Unauthorized(ApiResponse<object>.Fail("Refresh token failed", Errors));

                var claims = _authService.GetClaims(SigapUser.MapToDto());
                var appIdentity = new ClaimsIdentity(claims);

                AuthUser authUserDto = new AuthUser
                {
                    accessToken = _authService.GenerateAccessToken(appIdentity, _jwtKey, _accessTokenExpiry),
                    accessTokenExpiresIn = _accessTokenExpiry,
                    refreshToken = _authService.GenerateRefreshToken(),
                    refreshTokenExpiresIn = _refreshTokenExpiry,
                };

                SigapUser!.refresh_token = authUserDto.refreshToken;
                _context.SigapUsers.Update(SigapUser);
                _context.SaveChanges();
                // _refreshTokens[request.username] = authUserDto.refreshToken;
                return Ok(ApiResponse<AuthUser>.Ok("Refresh token successfully", authUserDto));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                // if (!ValidationHelper.TryValidate(request, out var errors))
                //     return StatusCode(400, ApiResponse<object>.Fail("Logout failed", ValidationHelper.GetErrorDictionary(errors)));

                // Ambil username dari JWT token
                var tokenUserid = User.Identity?.Name;

                var SigapUser = _context.SigapUsers.Include(o => o.BagianUserDto).FirstOrDefault(u => u.user_id.ToString() == tokenUserid);
                // string user_id = "";
                // if (SigapUser == null)
                //     Errors?.Add("username", new[] { "The field 'username' value is not found." });
                // else
                //     user_id = SigapUser.user_id.ToString();

                if (tokenUserid == null)
                    Errors?.Add("token", new[] { "The bearer 'token' is invalid." });
                // Cek apakah username dari route sama dengan yang di token
                // else if (!string.Equals(tokenUserid, request.username, StringComparison.OrdinalIgnoreCase))
                //     return Forbid(); // atau bisa return BadRequest

                if (Errors?.Any() == true)
                    return Unauthorized(ApiResponse<object>.Fail("Logout failed", Errors));
                else if (SigapUser != null)
                {
                    SigapUser.refresh_token = null;
                    _context.SigapUsers.Update(SigapUser);
                    _context.SaveChanges();

                    return Ok(ApiResponse<object>.Ok("Logout successful"));
                }
                else
                    return BadRequest(ApiResponse<object>.Fail("User not logged in or already logged out"));
            }
            catch (DbUpdateConcurrencyException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Concurrency error: " + ex.Message));
            }
            catch (DbUpdateException ex){
                return StatusCode(500, ApiResponse<object>.Fail("Database error: " + ex.Message));
            }
            catch (Exception ex){
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error: " + ex.Message));
            }
        }
    }
}
