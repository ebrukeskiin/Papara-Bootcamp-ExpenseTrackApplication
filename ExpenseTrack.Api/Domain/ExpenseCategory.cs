using Base;

namespace ExpenseTrack.Api.Domain
{
    public class ExpenseCategory:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
     
        // Navigation properties
        public ICollection<Expense> Expenses { get; set; }
    }
}
