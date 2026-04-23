using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/Dashboard")]
    public class DashboardAPIController : Controller
    {
        private readonly ApiService _apiService;

        public DashboardAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Dashboard");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpGet("readiness")]
        public async Task<IActionResult> GetDetailReadiness()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Dashboard/GetReadiness");
            return await _apiService.HandleApiResponse(response);
        }
    }
}
