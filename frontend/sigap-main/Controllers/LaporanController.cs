using Microsoft.AspNetCore.Mvc;

namespace SIGAP.Controllers
{
    public class LaporanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RiwayatTransaksiLaporan()
        {
            return View();
        }
        public IActionResult RiwayatStockLaporan()
        {
            return View();
        }
    }
}
