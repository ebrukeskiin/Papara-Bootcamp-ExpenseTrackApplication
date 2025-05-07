using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Response
{
    public class ExpenseResponse
    {
        public long Id { get; set; }

        public int UserId { get; set; }

        public string UserFullName { get; set; }

        public int ExpenseCategoryId { get; set; }

        public string CategoryName { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime ExpenseDate { get; set; }

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
