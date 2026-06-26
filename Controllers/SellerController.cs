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
        public IActionResult EditTransaction(Transaction transactions) 
        {
            if (ModelState.IsValid)
            {
                var existing = _context.Transactions.Find(transactions.Id);
                if (existing != null)
                {
                    existing.Amount = transactions.Amount;
                    existing.Description = transactions.Description;
                    existing.TransactionDate = transactions.TransactionDate;

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(transactions);
        }

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

        // GET: Seller/CreateTransaction
        [HttpGet]
        public IActionResult CreateTransaction()
        {
            // Simply displays the blank input form page
            return View();
        }

        //// POST: Seller/CreateTransaction
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateTransaction(string description, decimal amount, DateTime transactionDate)
        //{
        //    if (string.IsNullOrEmpty(description) || amount <= 0)
        //    {
        //        ModelState.AddModelError("", "Please fill in all fields with valid data.");
        //        return View();
        //    }

        //    // 1. Create a brand new model instance using your database class type
        //    var newTransaction = new Transaction
        //    {
        //        Description = description,
        //        Amount = amount,
        //        TransactionDate = transactionDate
        //        // Note: The database automatically assigns a brand new, unique 'Id' 
        //    };

        //    try
        //    {
        //        // 2. Queue the new record into Entity Framework tracking
        //        _context.Transactions.Add(newTransaction);

        //        // 3. Command the database to execute a SQL INSERT instruction and save it permanently
        //        await _context.SaveChangesAsync();

        //        // 4. Send the user straight back to their dashboard to see the new row listed
        //        return RedirectToAction("Index", "Seller");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Database error occurred while adding data. Try again.");
        //        return View();
        //    }


        // POST: Seller/CreateTransaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(int userId, string description, decimal amount, DateTime transactionDate)
        {
            // 1. Basic validation check
            if (userId <= 0 || string.IsNullOrEmpty(description) || amount <= 0)
            {
                ModelState.AddModelError("", "Please fill in all fields with valid data.");
                return View();
            }

            // 2. Safety Check: Verify that the User ID typed actually exists in your Users table
            var userExists = _context.Users.Any(u => u.Id == userId);
            if (!userExists)
            {
                ModelState.AddModelError("", $"Error: User ID {userId} does not exist in the database. Please check the ID number.");
                return View();
            }

            // 3. Construct your database transaction mapping using the explicit UserID
            var newTransaction = new Transaction
            {
                UserId = userId,          // Captures the User ID from your input box!
                Description = description,
                Amount = amount,
                TransactionDate = transactionDate
            };

            try
            {
                // 4. Save directly into your Transactions table mapping
                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();

                // 5. Success! Bounce back to the main seller dashboard where the Recent Activity auto-updates
                return RedirectToAction("Index", "Seller");
            }
            catch (Exception ex)
            {
                // Catches any remaining structural database errors and prints out the exact reason
                ModelState.AddModelError("", "Database error: " + (ex.InnerException?.Message ?? ex.Message));
                return View();
            }
        }


    }


}
