using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class Permission
    {
        [Key]
        public Guid permission_id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(80)]
        public string code { get; set; } = string.Empty;
        [Required]
        [StringLength(120)]
        public string name { get; set; } = string.Empty;
        [StringLength(255)]
        public string description { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
    public class PermissionDto
    {
        public Guid permission_id { get; set; } = Guid.NewGuid();
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
    public class PermissionRequest
    {
        [Required]
        [StringLength(80)]
        public string code { get; set; } = string.Empty;
        [Required]
        [StringLength(120)]
        public string name { get; set; } = string.Empty;
        [StringLength(255)]
        public string description { get; set; } = string.Empty;
    }
}