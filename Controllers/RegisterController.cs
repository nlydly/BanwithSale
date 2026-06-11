using Microsoft.AspNetCore.Mvc;

namespace BanwithSale.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View("~/Views/Register/Index.cshtml");
        }

    }
}
