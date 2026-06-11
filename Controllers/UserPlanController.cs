using Microsoft.AspNetCore.Mvc;
using BanwithSale.Data;
using BanwithSale.Models;

namespace BanwithSale.Controllers
{
    public class UserPlanController : Controller
    {
        private readonly AppDbContext _context;

        public UserPlanController(AppDbContext context)
        {
            _context = context;
        }

        // READ - Buyer sees their purchased plans
        public IActionResult MyPlans()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("MyPlans","UserPlan");

            int userId = int.Parse(userIdStr);

            var myPlans = _context.UserPlans
                .Where(up => up.UserId == userId && up.IsActive)
                .Select(up => new
                {
                    up.Id,
                    up.PlanId,
                    //up.Plan.SpeedMbps,
                    //up.Plan.DataGB,
                    //up.Plan.Price,
                    up.StartDate,
                    up.EndDate,
                    up.IsActive
                })
                .ToList();

            ViewBag.MyPlans = myPlans;
            return View();
        }
        // CREATE - Buy a Plan
        [HttpPost]
        public IActionResult BuyPlan(int planId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdStr);

            var plan = _context.UserPlans.Find(planId);
            if (plan == null) return BadRequest();

            var userPlan = new UserPlan
            {
                UserId = userId,
                PlanId = planId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),   // 30 days validity
                IsActive = true
            };

            _context.UserPlans.Add(userPlan);
            _context.SaveChanges();

            TempData["Success"] = "Plan purchased successfully!";
            return RedirectToAction("MyPlans");
        }

        // READ - All Active Plans (for Buyer to choose)
        public IActionResult AvailablePlans()
        {
            var plans = _context.UserPlans.Where(p => p.IsActive).ToList();
            return View(plans);
        }

        // DELETE - Cancel / Deactivate Plan
        public IActionResult CancelPlan(int id)
        {
            var userPlan = _context.UserPlans.Find(id);
            if (userPlan != null)
            {
                userPlan.IsActive = false;
                _context.SaveChanges();
            }
            return RedirectToAction("MyPlans");
        }
    }
}
