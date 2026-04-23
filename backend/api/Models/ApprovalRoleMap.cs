using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    public class ApprovalRoleMap
    {
        [Key]
        public Guid approval_role_map_id { get; set; }
        public string legacy_code { get; set; } = String.Empty;
        public Guid role_id { get; set; }
        public string? note { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}