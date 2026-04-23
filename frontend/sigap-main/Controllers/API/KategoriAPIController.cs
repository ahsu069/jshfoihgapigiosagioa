using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/Kategori")]
    public class KategoriAPIController : Controller
    {
        private readonly ApiService _apiService;

        public KategoriAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        // [Authorize(Policy = "Permission:kategori_barang:read")]
        public async Task<IActionResult> GetKategori()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Category?is_deleted=false&orderColumn=namakategoribar&orderDir=asc");
            return await _apiService.HandleApiResponse(response);
            // return Ok(new {message = "OK"});
        }

        [HttpPost("datatable")]
        // [Authorize(Policy = "Permission:kategori_barang:read")]
        public async Task<IActionResult> GetKategoriDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Category/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        // [Authorize(Policy = "Permission:kategori_barang:create")]
        public async Task<IActionResult> AddKategori([FromBody] KategoriRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Category", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPut("{id}")]
        // [Authorize(Policy = "Permission:kategori_barang:update")]
        public async Task<IActionResult> EditKategori(string id, [FromBody] KategoriRequest model)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/Category/{id}", model);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpDelete("{id}")]
        // [Authorize(Policy = "Permission:kategori_barang:delete")]
        public async Task<IActionResult> DeleteKategori(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/Category/{id}");
            return await _apiService.HandleApiResponse(response);
        }
    }
}
