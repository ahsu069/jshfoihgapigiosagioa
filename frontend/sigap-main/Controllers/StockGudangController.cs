using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers
{
    public class StockGudangController : Controller
    {
        [Authorize(Policy = "Permission:barang:read")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DetailBarang()
        {
            return View();
        }

        public IActionResult TambahBarang()
        {
            return View();
        }

        public IActionResult EditBarang()
        {
            return View();
        }

        [Authorize(Policy = "Permission:kategori_barang:read")]
        public IActionResult KelolaKategori()
        {
            return View();
        }
    }
}
