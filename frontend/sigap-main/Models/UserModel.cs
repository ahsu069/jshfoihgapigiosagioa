using System.ComponentModel.DataAnnotations;

namespace Lexa.Models
{
    public class UserRequest
    {
        public string nama { get; set; }
        public int? bagian_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role_id { get; set; }
    }
    public class PasswordChangeRequest
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
        public string userId { get; set; }
        public string userNama { get; set; }
    }

    public class UserData
    {
        public string user_id { get; set; }
        public string nama { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int bagian_id { get; set; }
        // Add other properties as needed
        public userRoleDto userRoleDto { get; set; }
    }

    public class userRoleDto
    {
        public string role_id { get; set; }
    }
}