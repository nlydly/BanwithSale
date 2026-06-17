//using BanwithSale.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace BanwithSale.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly AppDbContext _context;
//        private readonly IConfiguration _configuration;
//        public HomeController(AppDbContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        public IActionResult Index()
//        {
//            var userRole = HttpContext.Session.GetString("UserRole");
//            var userName = HttpContext.Session.GetString("UserName");

//            if (userRole != "Buyer")
//            {
//                return RedirectToAction("Login", "Account");
//            }

//            ViewBag.UserName = userName;

//            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
//            {
//                using (var command = new SqlCommand("sp_GetSellerDashboard", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@Role", userRole);

//                    connection.Open();
//                    using (var reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            ViewBag.TotalSpent = 124500;
//                            ViewBag.ActivePlanGB = reader["ActivePlanGB"] ?? "N/A";
//                            //ViewBag.TotalDataUse = reader["TotalEarnings"] ?? "N/A";                          
//                        }
//                    }
//                    connection.Close(); 
//                }
//            }
//            int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");

//            var user = _context.Users.Find(userId);

//            return View("~/Views/Home/Index.cshtml");
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using BanwithSale.Data;
using Microsoft.EntityFrameworkCore;

namespace BanwithSale.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userIdStr) || userRole != "Buyer")
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdStr);

            // Get user data from database
            var user = _context.Users.Find(userId);

            if (user != null)
            {
                ViewBag.UserName = user.FullName;
                ViewBag.TotalEarnings = user.TotalEarning;
                ViewBag.ThisMonthEarnings = user.ThisMonthsEarning;
                ViewBag.ActivePlanGB = user.ActivePlanGB;
                ViewBag.UsedGB = user.UseGB;

            }
            else
            {
                ViewBag.UserName = "User";
                ViewBag.TotalEarnings = 0;
                ViewBag.ThisMonthEarnings = 0;
                ViewBag.ActivePlanGB = 500;
                ViewBag.UsedGB = 0;
            }

            return View();
        }
    }
}

