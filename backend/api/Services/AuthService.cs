using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
namespace api.Services;

public class AuthService
{
    public string GenerateAccessToken(ClaimsIdentity claims, string jwtKey, DateTime accessTokenExpiry)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = accessTokenExpiry,
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public List<Claim> GetClaims(SigapUserDto sigapUser)
    {
        Console.WriteLine($"[AUTH] Building claims for user_id={sigapUser.user_id}, RoleCode={sigapUser.RoleCode}");
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, sigapUser.user_id.ToString())
            // , new Claim(ClaimTypes.NameIdentifier, sigapUser.user_id.ToString())
            , new Claim("user_id", sigapUser.user_id.ToString())
            , new Claim("nama", sigapUser.nama)
            , new Claim("bagian_id", sigapUser.bagian_id?.ToString() ?? "0")
            , new Claim("bagian_nama", sigapUser.BagianUserDto?.nama?.ToString() ?? "")
            // , new Claim(ClaimTypes.Role, "admin")
        };

        if (!string.IsNullOrEmpty(sigapUser.RoleCode))
        {
            claims.Add(new Claim("RoleCode", sigapUser.RoleCode));
            claims.Add(new Claim(ClaimTypes.Role, sigapUser.RoleCode));
        }
        return claims;
    }
     public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string? hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}