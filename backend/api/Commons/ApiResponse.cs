namespace api.Commons
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public T? Data { get; set; }
          // 🔥 Tambahan untuk validasi
        public Dictionary<string, string[]>? Errors { get; set; }
       
       public static ApiResponse<T> Ok(string message = "", T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
        public static ApiResponse<T> Fail(string message, Dictionary<string, string[]>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}