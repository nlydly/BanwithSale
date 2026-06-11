
namespace BanwithSale.Models
{
    public class UserPlan
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PlanId { get; set; }
        public UserPlan Plan { get; set; } = null!;

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
