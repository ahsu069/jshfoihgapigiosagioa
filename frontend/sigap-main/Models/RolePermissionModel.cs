using System.ComponentModel.DataAnnotations;

namespace Lexa.Models {
    public class RolePermissionRequest
    {
        [Required]
        public Guid role_id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid permission_id { get; set; } = Guid.NewGuid();
    }
}