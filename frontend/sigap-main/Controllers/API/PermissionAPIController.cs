using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/Permission")]
    public class PermissionAPIController : Controller
    {
        private readonly ApiService _apiService;

        public PermissionAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        // [Authorize(Policy = "Permission:permission:read")]
        public async Task<IActionResult> GetPermission()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Permission?orderColumn=code&orderDir=asc");
            return await _apiService.HandleApiResponse(response);
            // return Ok(new {message = "OK"});
        }
        
        [HttpGet("{id}")]
        // [Authorize(Policy = "Permission:permission:read")]
        public async Task<IActionResult> DetailPermission(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Permission/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("datatable")]
        // [Authorize(Policy = "Permission:permission:read")]
        public async Task<IActionResult> GetPermissionDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Permission/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        // [Authorize(Policy = "Permission:permission:create")]
        public async Task<IActionResult> AddPermission([FromBody] PermissionRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Permission", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPut("{id}")]
        // [Authorize(Policy = "Permission:permission:update")]
        public async Task<IActionResult> EditPermission(string id, [FromBody] PermissionRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/Permission/{id}", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpDelete("{id}")]
        // [Authorize(Policy = "Permission:permission:delete")]
        public async Task<IActionResult> DeletePermission(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/Permission/{id}");
            return await _apiService.HandleApiResponse(response);
        }
    }
}
