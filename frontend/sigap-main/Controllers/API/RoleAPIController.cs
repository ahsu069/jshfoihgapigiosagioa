using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/Role")]
    public class RoleAPIController : Controller
    {
        private readonly ApiService _apiService;

        public RoleAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        // [Authorize(Policy = "Permission:role:read")]
        public async Task<IActionResult> GetRole()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Role?orderColumn=code&orderDir=asc");
            return await _apiService.HandleApiResponse(response);
        }
        
        [HttpGet("{id}")]
        // [Authorize(Policy = "Permission:role:read")]
        public async Task<IActionResult> DetailRole(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Role/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("datatable")]
        // [Authorize(Policy = "Permission:role:read")]
        public async Task<IActionResult> GetRoleDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Role/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        // [Authorize(Policy = "Permission:role:create")]
        public async Task<IActionResult> AddRole([FromBody] RoleRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Role", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPut("{id}")]
        // [Authorize(Policy = "Permission:role:update")]
        public async Task<IActionResult> EditRole(string id, [FromBody] RoleRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/Role/{id}", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpDelete("{id}")]
        // [Authorize(Policy = "Permission:role:delete")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/Role/{id}");
            return await _apiService.HandleApiResponse(response);
        }
    }
}
