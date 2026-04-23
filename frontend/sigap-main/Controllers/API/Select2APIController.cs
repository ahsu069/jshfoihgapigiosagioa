using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/Select2")]
    public class Select2APIController : Controller
    {
        private readonly ApiService _apiService;

        public Select2APIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("bagianuser")]
        public async Task<IActionResult> GetSelect2BagianUser([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Shared/GetBagianUserSelect2?search={search}&page={page}&pageSize={pageSize}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetSelect2Role([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Shared/GetRoleSelect2?search={search}&page={page}&pageSize={pageSize}");
            return await _apiService.HandleApiResponse(response);
        }

        // [HttpGet("employee")]
        // public async Task<IActionResult> GetSelect2Employee([FromQuery] string? search, [FromQuery] int? bagian_id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        // {
        //     var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Shared/GetEmployeeSelect2?search={search}&bagian_id={bagian_id}&page={page}&pageSize={pageSize}");
        //     return await _apiService.HandleApiResponse(response);
        // }

        [HttpGet("employee")]
        public async Task<IActionResult> GetSelect2Employee(
            [FromQuery] string? search,
            [FromQuery] int? bagian_id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            // Encode search to prevent URL errors
            string encodedSearch = Uri.EscapeDataString(search ?? string.Empty);

            // Build query
            var query = $"api/Shared/GetEmployeeSelect2?search={encodedSearch}&page={page}&pageSize={pageSize}";

            // Only append bagian_id if not null
            if (bagian_id.HasValue)
                query += $"&bagian_id={bagian_id.Value}";

            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, query);
            return await _apiService.HandleApiResponse(response);
        }

    }
}
