using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schema.Request;
using static ExpenseTrack.Api.Impl.CQRS.Expense;

namespace ExpenseTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Expense
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseRequest model)
        {
            var command = new CreateExpenseCommand
            {
                Expense = model,
                InsertedUser = User?.Identity?.Name ?? "system"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ExpenseRequest model)
        {
            var command = new UpdateExpenseCommand
            {
                Id = id,
                Expense = model,
                UpdatedUser = User?.Identity?.Name ?? "system"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteExpenseCommand
            {
                Id = id,
                UpdatedUser = User?.Identity?.Name ?? "system"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        // GET: api/expense
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllExpensesQuery());
            return Ok(result);
        }

        // GET: api/expense/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetExpenseByIdQuery { Id = id });
            return Ok(result);
        }

        // GET: api/expense/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _mediator.Send(new GetExpensesByUserIdQuery { UserId = userId });
            return Ok(result);
        }
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] GetFilteredExpensesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var command = new ApproveExpenseCommand
            {
                ExpenseId = id,
                ApprovedBy = User?.Identity?.Name ?? "admin"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] string reason)
        {
            var command = new RejectExpenseCommand
            {
                ExpenseId = id,
                RejectionReason = reason,
                RejectedBy = User?.Identity?.Name ?? "admin"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
