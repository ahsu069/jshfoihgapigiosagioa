using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class TransactionDetail
    {
        [Key]
        public Guid transact_detail_id { get; set; } = Guid.NewGuid();
        [Required]
        // [ForeignKey(nameof(transact_id))]
        [ForeignKey("transact_id")]
        public Guid transact_id { get; set; }
        // [ForeignKey("FK_TrxDetail_Trx")]
        [ForeignKey(nameof(transact_id))]
        // [NotMapped]
        public virtual TransactionHistory TransactionHistory { get; set; } = new TransactionHistory();
        [Required]
        // [ForeignKey(nameof(barang_id))]
        [ForeignKey("Itembarang_id")]
        public Guid barang_id { get; set; } = Guid.NewGuid();
        // [NotMapped]
        // [ForeignKey(nameof(barang_id))]
        [NotMapped]
        public virtual Item itemDto { get; set;} = new Item();
        public int jumlah_bar { get; set; } = 0;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        
    }
    public class TransactionDetailDto
    {
        public Guid transact_detail_id { get; set; } = Guid.NewGuid();
        public Guid transact_id { get; set; } = Guid.NewGuid();
        public Guid barang_id { get; set; } = Guid.NewGuid();
        public int jumlah_bar { get; set; } = 0;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public ItemDto itemDto { get; set; } = new ItemDto();
    }
    public class TransactionDetailRequest
    {
        [ForeignKey("barang_id")]
        [Required]
        [DefaultValue("3C487816-DDD9-467D-975E-4D3FF5515F54")]
        public string barang_id { get; set; } = string.Empty;
        [DefaultValue(10)]
        public string jumlah_bar { get; set; } = string.Empty;
        
    }
}