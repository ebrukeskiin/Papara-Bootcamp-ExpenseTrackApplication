using Base;

namespace ExpenseTrack.Api.Domain
{
    public class User:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string IBAN { get; set; }
        public UserRole Role { get; set; }
       
        // Navigation
        public ICollection<Expense> Expenses { get; set; }
    }
}
