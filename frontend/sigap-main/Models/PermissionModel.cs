namespace Lexa.Models {
    public class PermissionRequest
    {
        public string permission_id { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
}