using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schema.Request;
using static ExpenseTrack.Api.Impl.CQRS.User;

namespace ExpenseTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
            return Ok(result);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRequest model)
        {
            var command = new CreateUserCommand
            {
                User = model,
                InsertedUser = User?.Identity?.Name ?? "system"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UserRequest model)
        {
            var command = new UpdateUserCommand
            {
                Id = id,
                User = model,
                UpdatedUser = User?.Identity?.Name ?? "system"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteUserCommand
            {
                Id = id,
                UpdatedUser = User?.Identity?.Name ?? "system"
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
