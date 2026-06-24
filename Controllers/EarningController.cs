using Microsoft.AspNetCore.Mvc;
using BanwithSale.Data;

namespace BanwithSale.Controllers
{
    public class EarningsController : Controller
    {
        private readonly AppDbContext _context;

        public EarningsController(AppDbContext context)
        {
            _context = context;
        }

        // READ - Show Earnings Dashboard
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (userRole != "Seller" || string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdStr);

            var seller = _context.Users.Find(userId);

            // Get earnings data from database
            ViewBag.UserName = seller?.FullName;
            ViewBag.TotalEarnings = seller?.TotalEarning ?? 0;
            ViewBag.ThisMonthEarnings = seller?.ThisMonthsEarning ?? 0;

            //// Get recent transactions (Sales)
            ViewBag.RecentSales = _context.Transactions
                .Where(t => t.UserId == userId && t.Type == "Sale")
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .ToList();

            return View();
        }
    }
}