using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIGAP.Controllers
{
    public class UserController : Controller
    {

        [Authorize(Policy = "Permission:user:read")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult adduser()
        {
            return View();
        }
        public IActionResult edituser()
        {
            return View();
        }
        public IActionResult gantijabatan()
        {
            return View();
        }
        public IActionResult editprofile()
        {
            return View();
        }
        public IActionResult ubahpassword()
        {
            return View();
        }

        [Authorize(Policy = "Permission:permission:read")]
        public IActionResult Permission()
        {
            return View();
        }
        public IActionResult addrole()
        {
            return View();
        }
        public IActionResult EditRole()
        {
            return View();
        }
        public IActionResult addpermissions()
        {
            return View();
        }
        public IActionResult editpermissions()
        {
            return View();
        }

        [Authorize(Policy = "Permission:role:read")]
        public IActionResult Role()
        {
            return View();
        }
        public IActionResult editrolejabatan()
        {
            return View();
        }
    }
}
