using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization.Infrastructure;
namespace api.Models
{
    public class SigapUser
    {
        [Key]
        public Guid user_id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(120)]
        public string nama { get; set; } = string.Empty;
        [ForeignKey("bagian_id")]
        public int? bagian_id { get; set; } = 0;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [StringLength(20)]
        public string? username { get; set; }
        [StringLength(100)]
        public string? password { get; set; }
        [StringLength(100)]
        public string? refresh_token { get; set; }
        public virtual BagianUser? BagianUserDto { get; set; }
        [NotMapped]
        public virtual UserRole? UserRoleDto { get; set; }
    }
    public class SigapUserDto
    {
        public Guid user_id { get; set; } = Guid.NewGuid();
        public string nama { get; set; } = null!;
        public int? bagian_id { get; set; } = 0;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public string? username { get; set; }
        public string? password { get; set; }
        public string? refresh_token { get; set; }
        public BagianUserDto BagianUserDto { get; set; } = new BagianUserDto();
        public UserRoleDto UserRoleDto { get; set; } = new UserRoleDto();
        
    }
    public class SigapUserRequest
    {
        [StringLength(120)]
        public string nama { get; set; } = null!;
        public int? bagian_id { get; set; } = 0;
        [StringLength(20)]
        public string username { get; set; } = string.Empty;
        [StringLength(100)]
        public string password { get; set; } = string.Empty;
        public Guid role_id { get; set; } = Guid.NewGuid();
    }
    public class UsersCache
    {
        [Key]
        [StringLength(120)]
        public string user_id { get; set; } = string.Empty;
        [Required]
        [StringLength(120)]
        public string nama_pekerja { get; set; } = string.Empty;
        [StringLength(120)]
        public string fungsi_pekerja { get; set; } = string.Empty;
        [StringLength(120)]
        public string bagian_pekerja { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistorys { get; set; } = new List<TransactionHistory>();
    }
    public class UsersCacheDto
    {
        public string user_id { get; set; } = string.Empty;
        public string nama_pekerja { get; set; } = string.Empty;
        public string fungsi_pekerja { get; set; } = string.Empty;
        public string bagian_pekerja { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
    public class ProfileDto
    {
        public Guid user_id { get; set; } = Guid.NewGuid();
        public string nama { get; set; } = null!;
        public int? bagian_id { get; set; } = 0;
        public string created_at { get; set; } = string.Empty;
        public string? updated_at { get; set; }
        public BagianUserDto BagianUserDto { get; set; } = new BagianUserDto();
        public FungsiUserDto FungsiUserDto { get; set; } = new FungsiUserDto();
        public RoleDto RoleDto { get; set; } = new RoleDto();
        public List<PermissionDto> PermissionDto { get; set; } = new List<PermissionDto>();
    }
    
}