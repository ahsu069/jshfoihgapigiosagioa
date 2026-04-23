using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/Transaksi")]
    public class TransaksiAPIController : Controller
    {
        private readonly ApiService _apiService;

        public TransaksiAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        // [Authorize(Policy = "Permission:transaksi:riwayat:read")]
        public async Task<IActionResult> GetTransaksi()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Transaction?orderColumn=updated_at&orderDir=desc");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpGet("{id}")]
        // [Authorize(Policy = "Permission:transaksi:riwayat:read")]
        public async Task<IActionResult> DetailTransaksi(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Transaction/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("datatable")]
        // [Authorize(Policy = "Permission:transaksi:riwayat:read")]
        public async Task<IActionResult> GetTransaksiDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Transaction/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        // [Authorize(Policy = "Permission:transaksi:addtransaksi")]
        public async Task<IActionResult> AddTransaksi()
        {
            var form = await Request.ReadFormAsync();

            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Transaction", form, isMultipart: true);
            return await _apiService.HandleApiResponse(response);
        }

        // [HttpPost]
        // public async Task<IActionResult> AddTransaksi([FromForm] TransactionRequest model)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     try
        //     {
        //         var response = await _apiService.SendAuthorizedAsync(
        //             HttpMethod.Post,
        //             "api/Transaction",
        //             model,
        //             isMultipart: true
        //         );

        //         return await _apiService.HandleApiResponse(response);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return StatusCode(500, new { success = false, message = ex.Message });
        //     }
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> EditTransaksi(string id, [FromForm] TransactionRequest model)
        // {
        //     var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/Transaction/{id}", model, isMultipart: true);
        //     return await _apiService.HandleApiResponse(response);
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTransaksi(string id)
        {
            var form = await Request.ReadFormAsync();

            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/Transaction/{id}", form, isMultipart: true);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaksi(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/Transaction/{id}");
            return await _apiService.HandleApiResponse(response);
        }
    }
}
