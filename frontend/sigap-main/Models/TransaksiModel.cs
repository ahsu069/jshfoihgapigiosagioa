// using System.ComponentModel.DataAnnotations;
// using System.Text.Json.Serialization;
// using Microsoft.AspNetCore.Mvc;
// using api.Services;

// namespace Lexa.Models
// {
//     public class TransactionRequest
//     {
//         public TransactionHistoryRequest transactionHistory { get; set; } = new TransactionHistoryRequest();
//         [ModelBinder(BinderType = typeof(JsonFormBinder<List<TransactionDetailRequest>>))]
//         public List<TransactionDetailRequest> transactionDetail { get; set; } = new();
//         public EmployeeRequest? employeeRequest { get; set; } = null;
//     }

//     public class TransactionHistoryRequest
//     {
//         [Required]
//         [StringLength(20)]
//         public string kategori_transact_id { get; set; } = string.Empty;
//         [Required]
//         [StringLength(10)]
//         public string kategori_pekerja { get; set; } = string.Empty;
//         [Required]
//         [StringLength(50)]
//         public string no_miv_safety { get; set; } = string.Empty;
//         [StringLength(50)]
//         // public string no_miv_custom { get; set; } = string.Empty;
//         public string? no_miv_custom { get; set; }
//         [Required]
//         [StringLength(50)]
//         public string users_cache_id { get; set; } = string.Empty;
//     }

//     public class TransactionDetailRequest
//     {
//         [Required]
//         public string barang_id { get; set; } = string.Empty;
//         public string jumlah_bar { get; set; } = string.Empty;

//     }

//     public class EmployeeRequest
//     {
//         [StringLength(120)]
//         // public string nama_pekerja { get; set; } = string.Empty;
//         public string? nama_pekerja { get; set; }
//         [StringLength(120)]
//         public string? fungsi_pekerja { get; set; } = string.Empty;
//         [StringLength(50)]
//         public string? id_finger { get; set; } = string.Empty;
//         [StringLength(150)]
//         public string? perusahaan_pekerja { get; set; } = string.Empty;
//         public IFormFile? link_file_pendukung { get; set; }
//     }
// }

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lexa.Models
{
    public class TransactionRequest
    {
        public TransactionHistoryRequest transactionHistory { get; set; } = new TransactionHistoryRequest();

        [ModelBinder(BinderType = typeof(JsonFormBinder<TransactionDetailRequest>))]
        public List<TransactionDetailRequest> transactionDetail { get; set; } = new();

        public EmployeeRequest? employeeRequest { get; set; } = new EmployeeRequest();
    }

    public class TransactionHistoryRequest
    {
        [Required]
        public string kategori_transact_id { get; set; } = string.Empty;
        [Required]
        public string kategori_pekerja { get; set; } = string.Empty;
        [Required]
        public string no_miv_safety { get; set; } = string.Empty;
        public string? no_miv_custom { get; set; } = string.Empty;
        [Required]
        public string users_cache_id { get; set; } = string.Empty;
    }

    public class TransactionDetailRequest
    {
        [Required]
        public string barang_id { get; set; } = string.Empty;
        public string jumlah_bar { get; set; } = string.Empty;
    }

    public class EmployeeRequest
    {
        public string? nama_pekerja { get; set; }
        public string? fungsi_pekerja { get; set; }
        public string? id_finger { get; set; }
        public string? perusahaan_pekerja { get; set; }
        public IFormFile? link_file_pendukung { get; set; }
    }
}
