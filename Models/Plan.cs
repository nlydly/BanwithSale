namespace BanwithSale.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public int SpeedMbps { get; set; }
        public int DataGB { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
