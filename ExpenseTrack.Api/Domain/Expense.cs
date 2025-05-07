using Base;

namespace ExpenseTrack.Api.Domain
{
    public class Expense:BaseEntity
    {
        public int UserId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string PaymentMethod { get; set; }
        public ExpenseStatus ExpenseStatus { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int? ProcessedByUserId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public User ProcessedByUser { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public ICollection<ExpenseDocument> Documents { get; set; }
        public PaymentTransaction Payment { get; set; }
    }
}
