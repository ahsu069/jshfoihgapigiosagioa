using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using api.Services;
namespace api.Models
{
    public class TransactionHistory
    {
        [Key]
        public Guid transact_id { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(20)]
        [ForeignKey("kategori_transact_id")]
        public string kategori_transact_id { get; set; } = string.Empty;
        public virtual CategoryTransaction? CategoryTransactionsDto { get; set; }
        [Required]
        [StringLength(10)]
        [ForeignKey("kategori_pekerja")]
        public string kategori_pekerja { get; set; } = string.Empty;
        public virtual CategoryEmployee? CategoryEmployeeDto { get; set; }
        [Required]
        [StringLength(50)]
        public string no_miv_safety { get; set; } = string.Empty;
        [StringLength(50)]
        public string? no_miv_custom { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        [ForeignKey("users_cache_id")]
        public string users_cache_id { get; set; } = string.Empty;
        public virtual UsersCache? UsersCacheDto { get; set; }
        [ForeignKey("pekerja_temp_id")]
        public Guid? pekerja_temp_id { get; set; } = Guid.NewGuid();
        public virtual Employee? EmployeeDto { get; set; }
    //    [Required]
        public Guid? approval_manajemen_pekerja_id { get; set; }

        [ForeignKey(nameof(approval_manajemen_pekerja_id))]
        public virtual ApprovalStatus? ApprovalManajemenPekerjaIdDto { get; set; }

        // [Required]
        public Guid? approval_gudang_id { get; set; }

        [ForeignKey(nameof(approval_gudang_id))]
        public virtual ApprovalStatus? ApprovalGudangIdDto { get; set; }

        // [Required]
        public Guid? approval_sectionhead_id { get; set; }

        [ForeignKey(nameof(approval_sectionhead_id))]
        public virtual ApprovalStatus? ApprovalSectionheadIdDto { get; set; }
        [Required]
        [StringLength(30)]
        public string status { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        // public List<TransactionDetail> TransactionDetails{ get; set; }  = new List<TransactionDetail>();
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }
    public class TransactionHistoryDto
    {
        public Guid transact_id { get; set; } = Guid.NewGuid();
        public string kategori_transact_id { get; set; } = string.Empty;
        public CategoryTransactionDto CategoryTransactionsDto { get; set; } = new CategoryTransactionDto();
        public string kategori_pekerja { get; set; } = string.Empty;
        public CategoryEmployeeDto CategoryEmployeeDto { get; set; } = new CategoryEmployeeDto();
        public string no_miv_safety { get; set; } = string.Empty;
        public string? no_miv_custom { get; set; } = string.Empty;
        public string users_cache_id { get; set; } = string.Empty;
        public UsersCacheDto UsersCacheDto { get; set; } = new UsersCacheDto();
        public Guid? pekerja_temp_id { get; set; } = Guid.NewGuid();
        public EmployeeDto? EmployeeDto { get; set; } = new EmployeeDto();
        public Guid? approval_manajemen_pekerja_id { get; set; } = Guid.NewGuid();
        public ApprovalStatusDto? ApprovalManajemenPekerjaIdDto { get; set; }
        public Guid? approval_gudang_id { get; set; } = Guid.NewGuid();
        public ApprovalStatusDto? ApprovalGudangIdDto { get; set; }
        public Guid? approval_sectionhead_id { get; set; } = Guid.NewGuid();
        public ApprovalStatusDto? ApprovalSectionheadIdDto { get; set; }
        public string status { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public List<TransactionDetailDto> TransactionDetailDto { get; set; } = new List<TransactionDetailDto>();
        public bool is_allow_to_approve { get; set; } = false;
    }

    public class TransactionRequest
    {
        public TransactionHistoryRequest transactionHistory { get; set; } = new TransactionHistoryRequest();
        
        [FromForm(Name = "transactionDetail")]
        [ModelBinder(BinderType = typeof(JsonFormBinder<List<TransactionDetailRequest>>))]
        public List<TransactionDetailRequest> transactionDetail { get; set; } = new();
        public EmployeeRequest? employeeRequest { get; set; } = null;
    }
    public class TransactionHistoryRequest
    {
        [Required]
        [StringLength(20)]
        [ForeignKey("kategori_transact_id")]
        [DefaultValue("IN")]
        public string kategori_transact_id { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        [ForeignKey("kategori_pekerja")]
        [DefaultValue("KON")]
        public string kategori_pekerja { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        [DefaultValue("MIV-000")]
        public string no_miv_safety { get; set; } = string.Empty;
        [StringLength(50)]
        [DefaultValue("MIV-000-Custom")]
        public string no_miv_custom { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        [ForeignKey("users_cache_id")]
        [DefaultValue("af205c4c-bc1b-49c9-9364-0905d73bf2f4")]
        public string users_cache_id { get; set; } = string.Empty;
        // [ForeignKey("pekerja_temp_id")]
        // [DefaultValue("A4C3B38F-D6BE-43EC-97EC-2BF788AA2B3E")]
        // public Guid? pekerja_temp_id { get; set; } = Guid.Empty;
    }
}