using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    public class RolePermission
    {
        [Key]
        public Guid role_permission_id { get; set; } = Guid.NewGuid();
        [Required]
        [ForeignKey("role_id")]
        public Guid role_id { get; set; } = Guid.NewGuid();
        [Required]
        public virtual Role? RoleDto { get; set; }
        [ForeignKey("permission_id")]
        public Guid permission_id { get; set; } = Guid.NewGuid();
        public virtual Permission? PermissionDto { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class RolePermissionDto
    {
        public Guid role_permission_id { get; set; } = Guid.NewGuid();
        public Guid role_id { get; set; } = Guid.NewGuid();
        public RoleDto RoleDto { get; set; } = new RoleDto();
        public Guid permission_id { get; set; } = Guid.NewGuid();
        public PermissionDto PermissionDto { get; set; } = new PermissionDto();
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
    public class RolePermissionRequest
    {
        [Required]
        public Guid role_id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid permission_id { get; set; } = Guid.NewGuid();
    }
}