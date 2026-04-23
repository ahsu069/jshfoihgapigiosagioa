using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/User")]
    public class UserAPIController : Controller
    {
        private readonly ApiService _apiService;

        public UserAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        // [Authorize(Policy = "Permission:user:read")]
        public async Task<IActionResult> GetUser()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/User?orderColumn=nama&orderDir=asc");
            return await _apiService.HandleApiResponse(response);
        }
        
        [HttpGet("{id}")]
        // [Authorize(Policy = "Permission:user:read")]
        public async Task<IActionResult> DetailUser(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/User/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("datatable")]
        // [Authorize(Policy = "Permission:user:read")]
        public async Task<IActionResult> GetUserDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/User/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        // [Authorize(Policy = "Permission:user:create")]
        public async Task<IActionResult> AddUser([FromBody] UserRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.password))
                model.password = "pass1234";

            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/User", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPut("{id}")]
        // [Authorize(Policy = "Permission:user:update")]
        public async Task<IActionResult> EditUser(string id, [FromBody] UserRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/User/{id}", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpDelete("{id}")]
        // [Authorize(Policy = "Permission:user:delete")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/User/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("ubah_password")]
        public async Task<IActionResult> UbahPassword([FromBody] PasswordChangeRequest model)
        {
            try
            {
                // Get current user data
                var getUserResponse = await _apiService.SendAuthorizedAsync(
                    HttpMethod.Get, 
                    $"api/User/{model.userId}"
                );

                if (!getUserResponse.IsSuccessStatusCode)
                {
                    return Unauthorized(new 
                    { 
                        success = false, 
                        message = "User tidak ditemukan!" 
                    });
                }

                var userDataJson = await getUserResponse.Content.ReadAsStringAsync();
                var userDataResponse = JsonSerializer.Deserialize<ApiResponse<UserData>>(userDataJson, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (userDataResponse?.Data == null)
                {
                    return Unauthorized(new 
                    { 
                        success = false, 
                        message = "Data user tidak ditemukan!" 
                    });
                }

                var userData = userDataResponse.Data;

                // Verify old password
                bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(model.oldPassword, userData?.password);
                if (!isOldPasswordValid)
                {
                    return Unauthorized(new 
                    { 
                        success = false, 
                        message = "Password lama tidak sesuai!",
                        errors = new 
                        {
                            passwordLama = new[] { "Password lama tidak sesuai!" }
                        }
                    });
                }

                // Check if new passwords match
                if (model.newPassword != model.confirmPassword)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Password baru dan konfirmasi password tidak sama!",
                        errors = new 
                        {
                            passwordBaru = new[] { "Password baru dan konfirmasi password tidak sama!" }
                        }
                    });
                }

                // Update password via API
                var updatePasswordPayload = new
                {
                    nama = userData?.nama,
                    bagian_id = userData?.bagian_id,
                    username = userData?.username,
                    password = model.newPassword,
                    role_id = userData?.userRoleDto?.role_id
                };

                var updateResponse = await _apiService.SendAuthorizedAsync(
                    HttpMethod.Put, 
                    $"api/User/{model.userId}",
                    updatePasswordPayload
                );

                if (updateResponse.IsSuccessStatusCode)
                {
                    return Ok(new 
                    { 
                        success = true, 
                        message = "Password berhasil diubah!" 
                    });
                }
                else
                {
                    return Unauthorized(new 
                    { 
                        success = false, 
                        message = "Gagal mengubah password!" 
                    });
                }
            }
            catch (Exception ex)
            {
                return Unauthorized(new 
                { 
                    success = false, 
                    message = $"Terjadi kesalahan: {ex.Message}" 
                });
            }
        }
    }
}
