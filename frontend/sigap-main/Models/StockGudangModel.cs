using System.ComponentModel.DataAnnotations;

namespace Lexa.Models {
    public class StockGudangRequest
    {
        // public string nama_barang { get; set; } = string.Empty;
        // public int msl_barang { get; set; } = 0;
        // public int jumlah_barang { get; set; } = 0;
        // public string satuanbar_id { get; set; } = string.Empty;
        // public string kategoribar_id { get; set; } = string.Empty;
        // public string link_gambar_bar { get; set; } = string.Empty;
        // public string status_bar { get; set; } = string.Empty;
        // public bool is_deleted { get; set; } = false;
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
        public Guid kategoribar_id { get; set; }
        // public string? link_gambar_bar { get; set; } = string.Empty;
        public IFormFile? link_gambar_bar { get; set; }
        public string? status_bar { get; set; } = string.Empty;
        public bool is_deleted { get; set; } = false;
    }
}
