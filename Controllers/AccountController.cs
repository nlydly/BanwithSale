using Microsoft.AspNetCore.Mvc;
using BanwithSale.Data;
using BanwithSale.Models;

namespace BanwithSale.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Login Page
        public IActionResult Login()
        {
            return View("~/Views/Login/Index.cshtml");
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);

                if (user.Role == "Seller")
                    return RedirectToAction("Index", "Seller");
                else
                    return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Wrong email or password!";
            return View("~/Views/Login/Index.cshtml");
        }


        // GET: Register Page
        public IActionResult Register()
        {
            return View("~/Views/Register/Index.cshtml");
        }

        // POST: Register - Save to Database
        [HttpPost]
        public IActionResult Register(string fullName, string email, string password, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                ViewBag.Error = "Please select Buyer or Seller";
                return View("~/Views/Register/Index.cshtml");
            }
            
            // Check if email already exists
            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "This email is already registered!";
                return View("~/Views/Register/Index.cshtml");
            }

            // Create new user
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Role = role,
                TotalEarning = 0,
                ThisMonthsEarning = 0,
                ActivePlanGB = 500,
                UseGB = 0
            };

            // Save to Database
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Auto Login
            HttpContext.Session.SetString("UserId", newUser.Id.ToString());
            HttpContext.Session.SetString("UserName", newUser.FullName);
            HttpContext.Session.SetString("UserRole", newUser.Role);

            // Redirect based on role
            if (role == "Seller")
                return RedirectToAction("Index", "Seller");
            else
                return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}