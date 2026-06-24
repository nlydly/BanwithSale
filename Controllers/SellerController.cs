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

            // Recent Sales Fixed Line
            ViewBag.RecentSales = _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToList();

            return View("~/Views/Seller/Index.cshtml");
        }

        // DELETE Transaction
        public IActionResult DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Edit Transaction
        public IActionResult EditTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
                return RedirectToAction("Index");

            return View(transaction);
        }

        // POST: Save Edited Transaction
        [HttpPost]
        public IActionResult EditTransaction(Transaction transactions)  // ← should be singular "transaction" ideally
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Transactions.Find(transactions.Id);
                if (existing != null)
                {
                    existing.Amount = transactions.Amount;
                    existing.Description = transactions.Description;
                    existing.TransactionDate = transactions.TransactionDate;
                    // add other properties you need

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(transactions);
        }

        // ALTERNATIVE DELETE (kept for fallback compatibility)
        public IActionResult Delete(int id)
        {
            var transactions = _context.Transactions.Find(id);
            if (transactions != null)
            {
                _context.Transactions.Remove(transactions);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}