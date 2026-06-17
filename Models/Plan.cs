using Microsoft.AspNetCore.Mvc;

namespace BanwithSale.Models
{
    public class Plan : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public int Id { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public int SpeedMbps { get; set; }
        public int DataGB { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

    }

}



