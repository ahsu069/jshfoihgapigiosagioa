using Microsoft.AspNetCore.Mvc;

namespace Lexa.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("error")]
        public IActionResult Error()
        {
            ViewBag.Error = TempData["Error"] ?? null;
            return View();
        }

        [HttpGet("error403")]
        public IActionResult Error403()
        {
            return View();
        }

        [HttpGet("error404")]
        public IActionResult Error404()
        {
            return View();
        }

        [HttpGet("error500")]
        public IActionResult Error500()
        {
            return View();
        }

    }
}