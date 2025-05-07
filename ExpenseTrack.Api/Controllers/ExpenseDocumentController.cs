using ExpenseTrack.Api.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schema.Request;
using static ExpenseTrack.Api.Impl.CQRS.ExpenseDocument;

[ApiController]
[Route("api/[controller]")]
public class ExpenseDocumentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _env;

    public ExpenseDocumentController(IMediator mediator, IWebHostEnvironment env)
    {
        _mediator = mediator;
        _env = env;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] int expenseId, [FromForm] IFormFile file)
    {
        var command = new CreateExpenseDocumentCommand
        {
            ExpenseId = expenseId,
            File = file
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    
}
