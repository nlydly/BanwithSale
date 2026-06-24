using BanwithSale.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanwithSale.Controllers
{
    public class DashBoardContoller : Controller
    {
        public DbSet<User> users { get; set; }

        public IActionResult Index()
        {
            return View();
        }  


       

    }
}
