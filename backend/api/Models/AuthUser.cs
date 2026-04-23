using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class AuthUser
    {
        public string accessToken { get; set; } = null!;
        public DateTime accessTokenExpiresIn { get; set; }
        public string refreshToken { get; set; } = null!;
        public DateTime refreshTokenExpiresIn { get; set; }
        // public SigapUserDto user { get; set; } = new SigapUserDto();
    }
    public class LoginRequest{
        [StringLength(120)]
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }
    public class RefreshRequest{
        [StringLength(120)]
        // public string username { get; set; } = null!;
        public string refreshToken { get; set; } = null!;
    }
    // public class LogoutRequest{
    //     [StringLength(120)]
    //     // public string username { get; set; } = null!;
    // }
}