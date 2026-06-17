using Microsoft.AspNetCore.Mvc;
using BanwithSale.Data;
using BanwithSale.Models;
using System.Linq;

namespace BanwithSale.Controllers
{
    public class SellerController : Controller
    {
        private readonly AppDbContext _context;

        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userName = HttpContext.Session.GetString("UserName");

            if (userRole != "Seller")
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = userName;

            int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");

            var seller = _context.Users.Find(userId);

            if (seller != null)
            {
                ViewBag.TotalEarnings = seller.TotalEarning;
                ViewBag.ThisMonthEarnings = seller.ThisMonthsEarning;
                ViewBag.UsedGB = seller.UseGB;
                ViewBag.ActivePlanGB = seller.ActivePlanGB;
            }

            // Recent Sales
            ViewBag.RecentSales = _context.Transactions
                .Where(t => t.UserId == userId && t.Type == "Sale")
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToList();

            return View("~/Views/Seller/Index.cshtml");
        }
    }
}