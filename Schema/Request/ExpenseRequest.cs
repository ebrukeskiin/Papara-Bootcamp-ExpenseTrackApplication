using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Request
{
    public class ExpenseRequest
    {
        public int UserId { get; set; }

        public int ExpenseCategoryId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime ExpenseDate { get; set; }
    }
}
