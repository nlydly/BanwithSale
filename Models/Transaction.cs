using System.ComponentModel.DataAnnotations;

namespace BanwithSale.Models
{
    public class Transaction
    {
        [Key]                                    
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;     
        public string? Description { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}