//using Microsoft.AspNetCore.Mvc;
//using BanwithSale.Data;
//using BanwithSale.Models;
//using System.Linq;

//namespace BanwithSale.Controllers
//{
//    public class SellerController : Controller
//    {
//        private readonly AppDbContext _context;

//        public SellerController(AppDbContext context)
//        {
//            _context = context;
//        }

//        // Main Seller Dashboard
//        public IActionResult Index()
//        {
//            var userRole = HttpContext.Session.GetString("UserRole");
//            var userName = HttpContext.Session.GetString("UserName");

//            if (userRole != "Seller")
//            {
//                return RedirectToAction("Login", "Account");
//            }

//            ViewBag.UserName = userName;

//            int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");

//            var seller = _context.Users.Find(userId);

//            if (seller != null)
//            {
//                ViewBag.TotalEarnings = seller.TotalEarning;
//                ViewBag.ThisMonthEarnings = seller.ThisMonthsEarning;
//                ViewBag.ActivePlanGB = seller.ActivePlanGB;
//                ViewBag.UsedGB = seller.UseGB;
//            }
//            else
//            {
//                ViewBag.TotalEarnings = 0;
//                ViewBag.ThisMonthEarnings = 0;
//                ViewBag.ActivePlanGB = 0;
//                ViewBag.UsedGB = 0;
//            }

//            return View("~/Views/Seller/Index.cshtml");
//        }

//        // ================== CRUD for Seller ==================

//        // READ - My Listings / Plans
//        public IActionResult MyListings()
//        {
//            return View();
//        }

//        // CREATE - Add New Bandwidth Plan (Seller can add their own plan)
//        public IActionResult AddPlan()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult AddPlan(string planName, int speedMbps, int dataGB, decimal price)
//        {
//            // You can create a new Plan table later
//            // For now, just show success message
//            ViewBag.Message = "New Plan Added Successfully!";
//            return View();
//        }

//        // UPDATE - Update Seller Profile
//        public IActionResult EditProfile()
//        {
//            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
//            var seller = _context.Users.Find(userId);
//            return View(seller);
//        }

//        [HttpPost]
//        public IActionResult EditProfile(User user)
//        {
//            var existing = _context.Users.Find(user.Id);
//            if (existing != null)
//            {
//                existing.FullName = user.FullName;
//                existing.Phone = user.Phone;
//                existing.Address = user.Address;
//                _context.SaveChanges();
//            }
//            return RedirectToAction("Index");
//        }

//        public IActionResult Logout()
//        {
//            HttpContext.Session.Clear();
//            return RedirectToAction("Login", "Account");
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using BanwithSale.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BanwithSale.Controllers
{
    public class SellerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public SellerController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userName = HttpContext.Session.GetString("UserName");

            if (userRole != "Seller")
                return RedirectToAction("Login", "Account");

            ViewBag.UserName = userName;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var command = new SqlCommand("sp_GetSellerDashboard", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Role", userRole);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ViewBag.ActivePlanGB = reader["ActivePlanGB"] ?? "N/A";
                            ViewBag.TotalEarning = reader["TotalEarning"] ?? "N/A";
                            ViewBag.ThisMonthsEarning = reader["ThisMonthsEarning"] ?? "N/A";
                            //ViewBag.TotalBandwidthSold = reader["TotalBandwidthSold"] ?? "N/A";
                            //ViewBag.ActiveListings = reader["ActiveListings"] ?? "N/A";
                        }
                    }
                    connection.Close();
                }
            }
            return View("~/Views/Seller/Index.cshtml");
        }
    }
}