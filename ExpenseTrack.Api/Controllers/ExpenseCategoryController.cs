using AutoMap.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schema.Request;
using Schema.Response;
using static ExpenseTrack.Api.Impl.CQRS.ExpenseCategory;

namespace ExpenseTrack.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IMediator mediator;
        public ExpenseCategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("GetAll")]
        public async Task<ApiResponse<List<ExpenseCategoryResponse>>> GetAll()
        {
            var operation = new GetAllExpenseCategoriesQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("GetById/{id}")]
        public async Task<ApiResponse<ExpenseCategoryResponse>> GetByIdAsync([FromRoute] int id)
        {
            var operation = new GetExpenseCategoryByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<ExpenseCategoryResponse>> Post([FromBody] ExpenseCategoryRequest expense)
        {
            var command = new CreateExpenseCategoryCommand
            {
                Category = expense,
                InsertedUser = User?.Identity?.Name ?? "system"
            };
            var result = await mediator.Send(command);
            return result;
        }
        // PUT: api/ExpenseCategory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseCategoryRequest model)
        {
            var command = new UpdateExpenseCategoryCommand
            {
                Id = id,
                Category = model,
                UpdatedUser = User.Identity?.Name ?? "system"
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/ExpenseCategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteExpenseCategoryCommand
            {
                Id = id,
                UpdatedUser = User.Identity?.Name ?? "system"
            };

            var result = await mediator.Send(command);
            return Ok(result);
        }

    }
}
