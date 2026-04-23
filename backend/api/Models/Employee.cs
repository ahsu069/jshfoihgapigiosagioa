using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization.Infrastructure;
namespace api.Models
{
    public class Employee
    {
        [Key]
        public Guid pekerja_temp_id { get; set; } = Guid.Empty;
        [Required]
        [StringLength(120)]
        public string nama_pekerja { get; set; } = string.Empty;
        [StringLength(120)]
        public string? fungsi_pekerja { get; set; } = string.Empty;
        [StringLength(50)]
        public string? id_finger { get; set; } = string.Empty;
        [StringLength(150)]
        public string? perusahaan_pekerja { get; set; } = string.Empty;
        [StringLength(255)]
        public string? link_file_pendukung { get; set; } = string.Empty;
        [ForeignKey("bagian_id")]
        public int? bagian_id { get; set; } = 0;
        public DateTime synced_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistorys { get; set; } = new List<TransactionHistory>();
        public virtual BagianUser? BagianUserDto { get; set; }
    }
    public class EmployeeDto
    {
        public Guid pekerja_temp_id { get; set; } = Guid.NewGuid();
        public string nama_pekerja { get; set; } = string.Empty;
        public string? fungsi_pekerja { get; set; } = string.Empty;
        public string? id_finger { get; set; } = string.Empty;
        public string? perusahaan_pekerja { get; set; } = string.Empty;
        public string? link_file_pendukung { get; set; } = string.Empty;
        public int? bagian_id { get; set; } = 0;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public BagianUserDto BagianUserDto { get; set; } = new BagianUserDto();
    }
    public class EmployeeRequest
    {
        [StringLength(120)]
        public string nama_pekerja { get; set; } = string.Empty;
        [StringLength(120)]
        public string? fungsi_pekerja { get; set; } = string.Empty;
        [StringLength(50)]
        public string? id_finger { get; set; } = string.Empty;
        [StringLength(150)]
        public string? perusahaan_pekerja { get; set; } = string.Empty;
        public IFormFile? link_file_pendukung { get; set; }
        public int? bagian_id { get; set; } = 0;
    }
}