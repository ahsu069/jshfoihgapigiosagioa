namespace Lexa.Models {
    public class RoleRequest
    {
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool is_active { get; set; } = true;
    }
}