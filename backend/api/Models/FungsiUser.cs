using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    public class FungsiUser
    {
        [Key]
        public int fungsi_id { get; set; }
        [StringLength(100)]
        public string nama { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        
    }
    public class FungsiUserDto
    {
        public int fungsi_id { get; set; }
        public string nama { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        
    }
}