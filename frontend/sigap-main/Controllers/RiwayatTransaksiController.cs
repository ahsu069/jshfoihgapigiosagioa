using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers
{
    public class RiwayatTransaksiController : Controller
    {
        [Authorize(Policy = "Permission:riwayat:transaksi:read")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Permission:riwayat:stock:read")]
        public IActionResult RiwayatStock()
        {
            return View();
        }

        public IActionResult DetailRiwayatTransaksi()
        {
            return View();
        }

        public IActionResult DetailRiwayatStock()
        {
            return View();
        }
    }
}
