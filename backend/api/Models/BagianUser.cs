using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    public class BagianUser
    {
        [Key]
        public int bagian_id { get; set; }
        [StringLength(120)]
        public string nama { get; set; } = string.Empty;
        public int? fungsi_id { get; set; } = 0;
        public virtual ICollection<SigapUser> SigapUsers { get; set; } = new List<SigapUser>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class BagianUserDto
    {
        public int bagian_id { get; set; }
        public string? nama { get; set; } = null!;
        public int? fungsi_id { get; set; } = 0;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
}