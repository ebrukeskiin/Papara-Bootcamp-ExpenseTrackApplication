using AutoMap.Base;
using MediatR;
using Schema.Request;
using Schema.Response;

namespace ExpenseTrack.Api.Impl.CQRS
{
    public class ExpenseDocument
    {
        public class CreateExpenseDocumentCommand : IRequest<ApiResponse<ExpenseDocumentResponse>>
        {
            public IFormFile File { get; set; }
            public int ExpenseId { get; set; } // gelen ekran veya urlden alınır (elle değil)
        }
        public class DeleteExpenseDocumentCommand : IRequest<ApiResponse>
        {
            public long Id { get; set; }
            public string? UpdatedUser { get; set; }
        }
        public class UpdateExpenseDocumentCommand : IRequest<ApiResponse>
        {
            public long Id { get; set; }
            public ExpenseDocumentRequest Document { get; set; }
            public string? UpdatedUser { get; set; }
        }
    }
}
