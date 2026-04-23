using System.Text.Json.Serialization;

namespace Lexa.Models
{
    public class ApprovalRequest
    {
        public string is_approved { get; set; } = String.Empty;
        public string remark { get; set; } = String.Empty;
        public List<Guid> transact_id { get; set; } = new List<Guid>();
    }
}