using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class UserRole
    {
        [Key]
        public Guid user_role_id { get; set; } = Guid.NewGuid();
        public Guid user_id { get; set; } = Guid.NewGuid();
        public Guid role_id { get; set; } = Guid.NewGuid();
        public DateTime effective_from { get; set; }
        public DateTime? effective_to { get; set; }
        public bool is_primary { get; set; } = true;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual SigapUser SigapUsers { get; set; } = new SigapUser();
        public Role RoleDto { get; set; } = new Role();
    }
    public class UserRoleDto
    {
        public Guid user_role_id { get; set; } = Guid.NewGuid();
        public Guid user_id { get; set; } = Guid.NewGuid();
        public Guid role_id { get; set; } = Guid.NewGuid();
        public string effective_from { get; set; } = String.Empty;
        public string? effective_to { get; set; }
        public bool is_primary { get; set; } = true;
        public RoleDto RoleDto { get; set; } = new RoleDto();
    } 
}