using System.Text;
using System.Text.Json;
using Lexa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers.Api
{
    [ApiController]
    [Route("api/StockGudang")]
    public class StockGudangAPIController : Controller
    {
        private readonly ApiService _apiService;

        public StockGudangAPIController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        // [Authorize(Policy = "Permission:barang:read")]
        public async Task<IActionResult> GetStockGudang()
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, "api/Item?is_deleted=false&orderColumn=nama_barang&orderDir=asc");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpGet("{id}")]
        // [Authorize(Policy = "Permission:barang:read")]
        public async Task<IActionResult> DetailStockGudang(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Item/{id}");
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost("datatable")]
        // [Authorize(Policy = "Permission:barang:read")]
        public async Task<IActionResult> GetStockGudangDatatable([FromBody] JsonElement datatableRequest)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Item/datatable", datatableRequest);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPost]
        // [Authorize(Policy = "Permission:barang:create")]
        public async Task<IActionResult> AddStockGudang([FromForm] StockGudangRequest request)
        {
            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(request.nama_barang ?? ""), "nama_barang");
            formData.Add(new StringContent(request.msl_barang.ToString()), "msl_barang");
            formData.Add(new StringContent(request.jumlah_barang.ToString()), "jumlah_barang");
            formData.Add(new StringContent(request.satuanbar_id ?? ""), "satuanbar_id");
            formData.Add(new StringContent(request.kategoribar_id.ToString()), "kategoribar_id");
            formData.Add(new StringContent(request.status_bar ?? ""), "status_bar");
            formData.Add(new StringContent(request.is_deleted.ToString()), "is_deleted");

            if (request.link_gambar_bar != null)
            {
                var streamContent = new StreamContent(request.link_gambar_bar.OpenReadStream());
                formData.Add(streamContent, "link_gambar_bar", request.link_gambar_bar.FileName);
            }

            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Item", formData, isMultipart: true);
            // var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Item", request);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpPut("{id}")]
        // [Authorize(Policy = "Permission:barang:update")]
        public async Task<IActionResult> EditStockGudang(string id, [FromForm] StockGudangRequest request)
        {
            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(request.nama_barang ?? ""), "nama_barang");
            formData.Add(new StringContent(request.msl_barang.ToString()), "msl_barang");
            formData.Add(new StringContent(request.jumlah_barang.ToString()), "jumlah_barang");
            formData.Add(new StringContent(request.satuanbar_id ?? ""), "satuanbar_id");
            formData.Add(new StringContent(request.kategoribar_id.ToString()), "kategoribar_id");
            formData.Add(new StringContent(request.status_bar ?? ""), "status_bar");
            formData.Add(new StringContent(request.is_deleted.ToString()), "is_deleted");

            if (request.link_gambar_bar != null)
            {
                var streamContent = new StreamContent(request.link_gambar_bar.OpenReadStream());
                formData.Add(streamContent, "link_gambar_bar", request.link_gambar_bar.FileName);
            }

            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/Item/{id}", formData, isMultipart: true);
            return await _apiService.HandleApiResponse(response);
        }

        [HttpDelete("{id}")]
        // [Authorize(Policy = "Permission:barang:delete")]
        public async Task<IActionResult> DeleteStockGudang(string id)
        {
            var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/Item/{id}");
            return await _apiService.HandleApiResponse(response);
        }
    }
}

// using System.Text.Json;
// using Lexa.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace Lexa.Controllers.Api
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class StockGudangAPIController : ControllerBase
//     {
//         private readonly ApiService _apiService;

//         public StockGudangAPIController(ApiService apiService)
//         {
//             _apiService = apiService;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetStockGudang()
//         {
//             var response = await _apiService.SendAuthorizedAsync(
//                 HttpMethod.Get, 
//                 "api/Item?orderColumn=nama_barang&orderDir=asc"
//             );

//             return await _apiService.HandleApiResponse(response);
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> DetailStockGudang(string id)
//         {
//             var response = await _apiService.SendAuthorizedAsync(HttpMethod.Get, $"api/Item/{id}");
//             return await _apiService.HandleApiResponse(response);
//         }

//         [HttpPost("datatable")]
//         public async Task<IActionResult> GetStockGudangDatatable([FromBody] JsonElement datatableRequest)
//         {
//             var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Item/datatable", datatableRequest);
//             return await _apiService.HandleApiResponse(response);
//         }

//         [HttpPost]
//         public async Task<IActionResult> AddStockGudang([FromForm] StockGudangRequest request)
//         {
//             var formData = BuildMultipartForm(request);
//             var response = await _apiService.SendAuthorizedAsync(HttpMethod.Post, "api/Item", formData, isMultipart: true);
//             return await _apiService.HandleApiResponse(response);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> EditStockGudang(string id, [FromForm] StockGudangRequest request)
//         {
//             var formData = BuildMultipartForm(request);
//             var response = await _apiService.SendAuthorizedAsync(HttpMethod.Put, $"api/Item/{id}", formData, isMultipart: true);
//             return await _apiService.HandleApiResponse(response);
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteStockGudang(string id)
//         {
//             var response = await _apiService.SendAuthorizedAsync(HttpMethod.Delete, $"api/Item/{id}");
//             return await _apiService.HandleApiResponse(response);
//         }

//         // Helper to build multipart form data
//         private MultipartFormDataContent BuildMultipartForm(StockGudangRequest request)
//         {
//             var formData = new MultipartFormDataContent
//             {
//                 { new StringContent(request.nama_barang ?? ""), "nama_barang" },
//                 { new StringContent(request.msl_barang.ToString()), "msl_barang" },
//                 { new StringContent(request.jumlah_barang.ToString()), "jumlah_barang" },
//                 { new StringContent(request.satuanbar_id ?? ""), "satuanbar_id" },
//                 { new StringContent(request.kategoribar_id.ToString()), "kategoribar_id" },
//                 { new StringContent(request.status_bar ?? ""), "status_bar" },
//                 { new StringContent(request.is_deleted.ToString()), "is_deleted" }
//             };

//             if (request.link_gambar_bar != null)
//             {
//                 var stream = new StreamContent(request.link_gambar_bar.OpenReadStream());
//                 formData.Add(stream, "link_gambar_bar", request.link_gambar_bar.FileName);
//             }

//             return formData;
//         }
//     }
// }
