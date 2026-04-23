using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class Category
    {
        [Key]
        public Guid kategoribar_id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(120)]
        public string namakategoribar { get; set; } = string.Empty;
        [Required]
        public bool is_deleted { get; set; } = false;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual ICollection<Item> ItemDto { get; set; } = new List<Item>();

    }
    public class CategoryDto
    {
        public Guid kategoribar_id { get; set; } = Guid.NewGuid();
        public string namakategoribar { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        // public List<ItemDto> ItemDto { get; set; } = new List<ItemDto>();

        // public decimal readiness_item { get; set; }
    }
    public class CategoryRequest
    {
        public string namakategoribar { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
    }
    public class CategoryEmployee
    {
        [Key]
        [StringLength(10)]
        public string kategori_pekerja_id { get; set; } = string.Empty;
        [Required]
        [StringLength(120)]
        public string nama_kategori { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistorys { get; set; } = new List<TransactionHistory>();
    }
    public class CategoryEmployeeDto
    {
        public string kategori_pekerja_id { get; set; } = string.Empty;
        public string nama_kategori { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
    public class CategoryTransaction
    {
        [Key]
        [StringLength(20)]
        public string kategori_transact_id { get; set; } = string.Empty;
        [Required]
        [StringLength(120)]
        public string nama_kategori_transact { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistorys { get; set; } = new List<TransactionHistory>();
    }
    public class CategoryTransactionDto
    {
        public string kategori_transact_id { get; set; } = string.Empty;
        public string nama_kategori_transact { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
    }
}