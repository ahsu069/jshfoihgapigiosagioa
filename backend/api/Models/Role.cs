using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class Role
    {
        [Key]
        public Guid role_id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(40)]
        public string code { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string name { get; set; } = string.Empty;
        [StringLength(255)]
        public string? description { get; set; } = string.Empty;
        [Required]
        public bool is_active { get; set; } = false;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
    public class RoleDto
    {
        public Guid role_id { get; set; } = Guid.NewGuid();
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool is_active { get; set; } = false;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
    public class RoleRequest
    {
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool is_active { get; set; } = false;
    }
}
