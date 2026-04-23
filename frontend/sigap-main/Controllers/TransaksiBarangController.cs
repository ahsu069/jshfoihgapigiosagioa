using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIGAP.Controllers
{
    public class TransaksiBarangController : Controller
    {
        [Authorize(Policy = "Permission:transaksi:permintaan")]
        public IActionResult PermintaanBarang()
        {
            return View();
        }

        [Authorize(Policy = "Permission:transaksi:pemasukan")]
        public IActionResult PemasukanBarang()
        {
            return View();
        }       
    }
}
