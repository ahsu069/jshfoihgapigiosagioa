using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    public class ApprovalStatus
    {
        [Key]
        public Guid approval_id { get; set; }

        public string user_id { get; set; } = String.Empty;
        public string role_type { get; set; } = String.Empty;
        public Guid approval_role_id { get; set; }
        public string? is_approved { get; set; }
        public string? remark { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        // [NotMapped]
        // public ICollection<TransactionHistory> ApprovalManajemenPekerjaTransactions { get; set; } = new List<TransactionHistory>();
        // [NotMapped]
        // public ICollection<TransactionHistory> ApprovalGudangTransactions { get; set; } = new List<TransactionHistory>();
        // [NotMapped]
        // public ICollection<TransactionHistory> ApprovalSectionHeadTransactions { get; set; } = new List<TransactionHistory>();
        // [NotMapped]
        public virtual UsersCache? usersCacheDto { get; set;} = new UsersCache();
    }
    public class ApprovalStatusDto
    {
        public Guid approval_id { get; set; }
        public string user_id { get; set; } = String.Empty;
        public string role_type { get; set; } = String.Empty;
        public Guid approval_role_id { get; set; }
        public string? is_approved { get; set; }
        public string? remark { get; set; } = String.Empty;
        public string created_at { get; set; } = String.Empty;
        public string updated_at { get; set; } = String.Empty;
        public UsersCacheDto usersCacheDto { get; set; } = new UsersCacheDto();
    }
    public class ApprovalRequest
    {
        public string is_approved { get; set; } = String.Empty;
        public string remark { get; set; } = String.Empty;
        public List<Guid> transact_id { get; set; } = new List<Guid>();
    }
}