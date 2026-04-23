using System.Text.Json.Serialization;

namespace Lexa.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }  // Nullable generic type

        [JsonPropertyName("errors")]
        public Dictionary<string, string[]>? Errors { get; set; }
    }

    public class TokenData
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("accessTokenExpiresIn")]
        public DateTime? AccessTokenExpiresIn { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("refreshTokenExpiresIn")]
        public DateTime? RefreshTokenExpiresIn { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse : ApiResponse<TokenData> { }
    public class LogoutResponse : ApiResponse<object?> { }

    public class ProfileDto
    {
        public string User_Id { get; set; }
        public string Nama { get; set; }
        public int Bagian_Id { get; set; }

        public BagianUserDto BagianUserDto { get; set; }
        public FungsiUserDto FungsiUserDto { get; set; }
        public RoleDto RoleDto { get; set; }
        public List<PermissionDto> PermissionDto { get; set; }

        public string Created_At { get; set; }
        public string Updated_At { get; set; }
    }

    public class BagianUserDto
    {
        public int Bagian_Id { get; set; }
        public string Nama { get; set; }
        public int Fungsi_Id { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
    }

    public class FungsiUserDto
    {
        public int Fungsi_Id { get; set; }
        public string Nama { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
    }

    public class RoleDto
    {
        public string Role_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Is_Active { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
    }

    public class PermissionDto
    {
        public string Permission_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }
    }

    public class TokenResult
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

}