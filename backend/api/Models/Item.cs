using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    public class Item
    {
        [Key]
        public Guid barang_id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(120)]
        public string nama_barang { get; set; } = string.Empty;
        public int? msl_barang { get; set; }
        [Required]
        public int jumlah_barang { get; set; }
        [Required]
        [StringLength(20)]
        public string satuanbar_id { get; set; } = string.Empty;
        [NotMapped]
        public virtual Uom? uomDto { get; set; }
        [Required]
        public Guid kategoribar_id { get; set; } = Guid.NewGuid();
        [NotMapped]
        public virtual Category? categoryDto { get; set; }
        [StringLength(255)]
        public string? link_gambar_bar { get; set; } = string.Empty;
        [StringLength(50)]
        public string? status_bar { get; set; } = string.Empty;
        [Required]
        public bool is_deleted { get; set; } = false;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [NotMapped]
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
        [NotMapped]
        public int booked_qty { get; set; } = 0;
    }
    public class ItemDto
    {
        public Guid barang_id { get; set; } = Guid.NewGuid();
        public string? nama_barang { get; set; } = string.Empty;
        public int? msl_barang { get; set; }
        public int? jumlah_barang { get; set; }
        public int booked_qty { get; set; } = 0;
        [ForeignKey("satuanbar_id")]
        public string? satuanbar_id { get; set; } = string.Empty;
        public UomDto? uomDto { get; set; } = new UomDto();
        [ForeignKey("kategoribar_id")]
        public Guid? kategoribar_id { get; set; } = Guid.NewGuid();
        public CategoryDto? categoryDto { get; set; } = new CategoryDto();
        public string? link_gambar_bar { get; set; } = string.Empty;
        public string? status_bar { get; set; } = string.Empty;
        public bool? is_deleted { get; set; } = false;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        // public decimal readiness_item { get; set; }
    }
    public class  ItemRequest
    {
        [Required]
        [StringLength(120)]
        public string nama_barang { get; set; } = string.Empty;
        public int? msl_barang { get; set; }
        [Required]
        public int jumlah_barang { get; set; }
        [Required]
        [StringLength(20)]
        public string satuanbar_id { get; set; } = string.Empty;
        [Required]
        public Guid kategoribar_id { get; set; } = Guid.NewGuid();
        // public string? link_gambar_bar { get; set; } = string.Empty;
        public IFormFile? link_gambar_bar { get; set; }
        public string? status_bar { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
    }
}