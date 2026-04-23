using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class Uom
    {
        [Required]
        [Key]
        [StringLength(20)]
        public string satuanbar_id { get; set; } = string.Empty;
        [StringLength(50)]
        public string nama_satuanbar { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
        public virtual ICollection<Item> Item { get; set; } = new List<Item>();
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class UomDto
    {
        public string satuanbar_id { get; set; } = string.Empty;
        public string nama_satuanbar { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
}