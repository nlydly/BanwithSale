namespace BanwithSale.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public decimal TotalEarning { get; set; } = 0;
        public decimal ThisMonthsEarning { get; set; } = 0;
        public int ActivePlanGB { get; set; } = 500;
        public int UseGB { get; set; } = 0;
    }
}