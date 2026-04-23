using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers
{
    //[Authorize(Policy = "")]
    public class ApprovalController : Controller
    {
        [Authorize(Policy = "Permission:approval:read")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DetailDataPermintaanBarang()
        {
            return View();
        }
    }
}
