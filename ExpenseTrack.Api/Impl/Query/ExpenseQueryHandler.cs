using AutoMap.Base;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseTrack.Api;
using Schema.Response;
using static ExpenseTrack.Api.Impl.CQRS.Expense;
using Microsoft.AspNetCore.Http;
using ExpenseTrack.Api.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Threading;

public class ExpenseQueryHandler :
    IRequestHandler<GetAllExpensesQuery, ApiResponse<List<ExpenseResponse>>>,
    IRequestHandler<GetExpenseByIdQuery, ApiResponse<ExpenseResponse>>,
    IRequestHandler<GetExpensesByUserIdQuery, ApiResponse<List<ExpenseResponse>>>
{
    private readonly MsSqlDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExpenseQueryHandler(MsSqlDbContext context, IMapper mapper,IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<List<ExpenseResponse>>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
 {
        var currentEmail = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == currentEmail, cancellationToken);
        if (currentUser == null)
            return new ApiResponse<List<ExpenseResponse>>("Kullanıcı bulunamadı.");

        IQueryable<Expense> query = _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .Where(x => x.IsActive);

        // Admin ise tüm kayıtları görür
        if (currentUser.Role == UserRole.Personnel)
        {
            // Personel sadece kendi harcamalarını görür
            query = query.Where(x => x.UserId == currentUser.Id);
        }

        var expenses = await query.ToListAsync(cancellationToken);
        var response = _mapper.Map<List<ExpenseResponse>>(expenses);

        return new ApiResponse<List<ExpenseResponse>>(response);
    }

    public async Task<ApiResponse<ExpenseResponse>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (expense == null)
            return new ApiResponse<ExpenseResponse>("Kayıt bulunamadı.");

        var response = _mapper.Map<ExpenseResponse>(expense);
        return new ApiResponse<ExpenseResponse>(response);
    }

    public async Task<ApiResponse<List<ExpenseResponse>>> Handle(GetExpensesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _context.Expenses
            .Where(x => x.UserId == request.UserId && x.IsActive)
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .ToListAsync(cancellationToken);

        var response = _mapper.Map<List<ExpenseResponse>>(expenses);
        return new ApiResponse<List<ExpenseResponse>>(response);
    }

    public async Task<ApiResponse<List<ExpenseResponse>>> Handle(GetFilteredExpensesQuery request, CancellationToken cancellationToken)
    {
        var currentEmail = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == currentEmail, cancellationToken);
        if (currentUser == null)
            return new ApiResponse<List<ExpenseResponse>>("Kullanıcı bulunamadı.");

        if (currentUser.Role != UserRole.Personnel)
            return new ApiResponse<List<ExpenseResponse>>("Sadece personel filtreleyebilir.");

        var query = _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .Where(x => x.IsActive && x.UserId == currentUser.Id);

        if (request.Status.HasValue)
            query = query.Where(x => x.ExpenseStatus == request.Status.Value);

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.ExpenseCategoryId == request.CategoryId.Value);

        if (!string.IsNullOrEmpty(request.PaymentMethod))
            query = query.Where(x => x.PaymentMethod == request.PaymentMethod);

        if (request.StartDate.HasValue)
            query = query.Where(x => x.ExpenseDate >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(x => x.ExpenseDate <= request.EndDate.Value);

        if (request.MinAmount.HasValue)
            query = query.Where(x => x.Amount >= request.MinAmount.Value);

        if (request.MaxAmount.HasValue)
            query = query.Where(x => x.Amount <= request.MaxAmount.Value);

        var expenses = await query.ToListAsync(cancellationToken);
        var response = _mapper.Map<List<ExpenseResponse>>(expenses);

        return new ApiResponse<List<ExpenseResponse>>(response);
    }
}
