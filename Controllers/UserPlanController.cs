using Microsoft.AspNetCore.Mvc;
using BanwithSale.Data;
using BanwithSale.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BanwithSale.Controllers
{
    public class UserPlanController : Controller
    {
        private readonly AppDbContext _context;

        public UserPlanController(AppDbContext context)
        {
            _context = context;
        }

        // READ - My Plans (Buyer Dashboard)
        public IActionResult MyPlans()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdStr);

            var myPlans = _context.UserPlans
                .Include(up => up.Plan)           // Must include the related Plan
                .Where(up => up.UserId == userId && up.IsActive)
                .Select(up => new
                {
                    up.Id,
                    PlanName = up.Plan.PlanName,
                    SpeedMbps = up.Plan.SpeedMbps,
                    DataGB = up.Plan.DataGB,
                    Price = up.Plan.Price,
                    up.StartDate,
                    up.EndDate,
                    up.IsActive
                })
                .ToList();

            ViewBag.MyPlans = myPlans;
            return View("~/Views/UserPlan/MyPlans.cshtml");
        }

        // Available Plans for Buying
        public IActionResult AvailablePlans()
        {
            var plans = _context.Plans.Where(p => p.IsActive).ToList();
            return View(plans);
        }

        // Buy a Plan
        [HttpPost]
        public IActionResult BuyPlan(int planId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdStr);

            var userPlan = new UserPlan
            {
                UserId = userId,
                PlanId = planId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsActive = true
            };

            _context.MyPlans.Add(userPlan);
            _context.SaveChanges();

            TempData["Success"] = "Plan purchased successfully!";
            return RedirectToAction("MyPlans");
        }

        // Cancel Plan
        public IActionResult CancelPlan(int id)
        {
            var userPlan = _context.MyPlans.Find(id);
            if (userPlan != null)
            {
                userPlan.IsActive = false;
                _context.SaveChanges();
            }
            return RedirectToAction("MyPlans");
        }
    }
}