using Base;

namespace ExpenseTrack.Api.Domain
{
    public class ExpenseDocument:BaseEntity
    {
        public int ExpenseId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        // Navigation properties
        public Expense Expense { get; set; }
    }
}
