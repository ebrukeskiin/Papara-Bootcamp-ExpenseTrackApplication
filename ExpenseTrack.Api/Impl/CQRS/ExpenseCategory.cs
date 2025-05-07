using AutoMap.Base;
using MediatR;
using Schema.Request;
using Schema.Response;

namespace ExpenseTrack.Api.Impl.CQRS
{
    public class ExpenseCategory
    {
        public record GetAllExpenseCategoriesQuery : IRequest<ApiResponse<List<ExpenseCategoryResponse>>>;
        public record GetExpenseCategoryByIdQuery(int Id) : IRequest<ApiResponse<ExpenseCategoryResponse>>;
        public class CreateExpenseCategoryCommand : IRequest<ApiResponse<ExpenseCategoryResponse>>
        {
            public ExpenseCategoryRequest Category { get; set; }
            public string? InsertedUser { get; set; }
        }

        public class UpdateExpenseCategoryCommand : IRequest<ApiResponse>
        {
            public int Id { get; set; }
            public ExpenseCategoryRequest Category { get; set; }
            public string? UpdatedUser { get; set; }
        }

        public class DeleteExpenseCategoryCommand : IRequest<ApiResponse>
        {
            public int Id { get; set; }
            public string? UpdatedUser { get; set; }
        }
    }
}
