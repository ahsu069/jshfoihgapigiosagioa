using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/RolePermission")]
    // [Authorize(Policy = "Permission:rbac:permission:manage")]
    public class RolePermissionAPIController : Controller
    {
        private readonly ApiService _apiService;

        public RolePermissionAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetRolePermission()
        // {
        //     var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/RolePermission");
        //     return await _apiService.HandleApiResponse(response);
        // }
        
        // [HttpGet]
        // public async Task<IActionResult> GetRolePermission([FromQuery] Guid? roleId, [FromQuery] Guid? permissionId)
        // {
        //     // Build query string dynamically based on provided parameters
        //     var query = new List<string>();
        //     if (roleId.HasValue)
        //         query.Add($"roleId={roleId}");
        //     if (permissionId.HasValue)
        //         query.Add($"permissionId={permissionId}");

        //     var queryString = query.Count > 0 ? "?" + string.Join("&", query) : string.Empty;

        //     var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/RolePermission{queryString}");
        //     return await _apiService.HandleApiResponse(response);
        // }

        [HttpGet]
        public async Task<IActionResult> GetRolePermission([FromQuery] string? role_id, [FromQuery] string? permission_id)
        {
            // Build query string dynamically based on provided parameters
            var query = new List<string>();
            if (!string.IsNullOrEmpty(role_id))
                query.Add($"role_id={role_id}");
            if (!string.IsNullOrEmpty(permission_id))
                query.Add($"permission_id={permission_id}");

            var queryString = query.Count > 0 ? "?" + string.Join("&", query) : string.Empty;

            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/RolePermission{queryString}");
            return await _apiService.HandleApiResponse(response);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> DetailRolePermission(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/RolePermission/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("datatable")]
        public async Task<IActionResult> GetRolePermissionDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/RolePermission/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddRolePermission([FromBody] RolePermissionRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/RolePermission", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditRolePermission(string id, [FromBody] RolePermissionRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/RolePermission/{id}", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolePermission(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/RolePermission/{id}");
            return await _apiService.HandleApiResponse(response);
        }
    }
}
