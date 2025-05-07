using AutoMap.Base;
using ExpenseTrack.Api.Domain;
using MediatR;
using Schema.Request;
using Schema.Response;

namespace ExpenseTrack.Api.Impl.CQRS
{
    public class Expense
    {
        public class CreateExpenseCommand : IRequest<ApiResponse<ExpenseResponse>>
        {
            public ExpenseRequest Expense { get; set; }
            public string? InsertedUser { get; set; }
        }
        public class UpdateExpenseCommand : IRequest<ApiResponse>
        {
            public long Id { get; set; } // Güncellenecek Expense kaydının ID’si
            public ExpenseRequest Expense { get; set; }
            public string? UpdatedUser { get; set; }
        }
        public class DeleteExpenseCommand : IRequest<ApiResponse>
        {
            public int Id { get; set; }
            public string? UpdatedUser { get; set; }
        }
        public class GetAllExpensesQuery : IRequest<ApiResponse<List<ExpenseResponse>>> { }
        public class GetExpenseByIdQuery : IRequest<ApiResponse<ExpenseResponse>>
        {
            public int Id { get; set; }
        }
        public class GetExpensesByUserIdQuery : IRequest<ApiResponse<List<ExpenseResponse>>>
        {
            public int UserId { get; set; }
        }
        public class GetFilteredExpensesQuery : IRequest<ApiResponse<List<ExpenseResponse>>>
        {
            public ExpenseStatus? Status { get; set; }
            public int? CategoryId { get; set; }
            public string? PaymentMethod { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public decimal? MinAmount { get; set; }
            public decimal? MaxAmount { get; set; }
        }

        public class ApproveExpenseCommand : IRequest<ApiResponse>
        {
            public int ExpenseId { get; set; }
            public string? ApprovedBy { get; set; } // admin email veya username
        }
        public class RejectExpenseCommand : IRequest<ApiResponse>
        {
            public int ExpenseId { get; set; }
            public string RejectionReason { get; set; } = null!;
            public string? RejectedBy { get; set; } // admin
        }
    }
}
