using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/approval")]
    public class ApprovalAPIController : Controller
    {
        private readonly ApiService _apiService;

        public ApprovalAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("{id}")]
        // [Authorize(Policy = "Permission:approval:read")]
        public async Task<IActionResult> DetailApproval(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Approval/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("datatable")]
        // [Authorize(Policy = "Permission:approval:read")]
        public async Task<IActionResult> GetApprovalDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Approval/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddApproval([FromBody] ApprovalRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Approval", model);
            return await _apiService.HandleApiResponse(response);
        }
    }
}
