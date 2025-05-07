using Base;

namespace ExpenseTrack.Api.Domain
{
    public class PaymentTransaction:BaseEntity
    {
        public int ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Navigation properties
        public Expense Expense { get; set; }
    }
}
