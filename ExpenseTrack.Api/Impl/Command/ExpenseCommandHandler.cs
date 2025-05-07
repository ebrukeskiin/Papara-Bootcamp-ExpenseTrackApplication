using AutoMapper;
using ExpenseTrack.Api.Domain;
using ExpenseTrack.Api;
using MediatR;
using Schema.Response;
using AutoMap.Base;
using static ExpenseTrack.Api.Impl.CQRS.Expense;
using Microsoft.EntityFrameworkCore;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ApiResponse<ExpenseResponse>>,
    IRequestHandler<UpdateExpenseCommand, ApiResponse>,
    IRequestHandler<DeleteExpenseCommand, ApiResponse>,
    IRequestHandler<ApproveExpenseCommand, ApiResponse>,
    IRequestHandler<RejectExpenseCommand, ApiResponse>
{
    private readonly MsSqlDbContext _context;
    private readonly IMapper _mapper;

    public CreateExpenseCommandHandler(MsSqlDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ExpenseResponse>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Expense>(request.Expense);

        entity.ExpenseStatus = ExpenseStatus.Pending; // Varsayılan durum
        entity.InsertedDate = DateTime.Now;
        entity.InsertedUser = request.InsertedUser ?? "system";
        entity.IsActive = true;

        await _context.Expenses.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<ExpenseResponse>(entity);
        return new ApiResponse<ExpenseResponse>(response);
    }
    public async Task<ApiResponse> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Expenses.FindAsync(request.Id);
        if (entity == null)
            return new ApiResponse("Güncellenecek harcama bulunamadı.");

        if (!entity.IsActive)
            return new ApiResponse("Bu harcama zaten pasif durumda.");

        // Güncelleme alanları
        entity.Amount = request.Expense.Amount;
        entity.Description = request.Expense.Description;
        entity.ExpenseDate = request.Expense.ExpenseDate;
        entity.ExpenseCategoryId = request.Expense.ExpenseCategoryId;
        entity.Location = request.Expense.Location;
        entity.PaymentMethod = request.Expense.PaymentMethod;

        entity.UpdatedDate = DateTime.Now;
        entity.UpdatedUser = request.UpdatedUser ?? "system";

        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Expenses.FindAsync(request.Id);
        if (entity == null)
            return new ApiResponse("Silinecek harcama bulunamadı.");

        if (!entity.IsActive)
            return new ApiResponse("Bu harcama zaten silinmiş.");

        entity.IsActive = false;
        entity.UpdatedDate = DateTime.Now;
        entity.UpdatedUser = request.UpdatedUser ?? "system";

        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(ApproveExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses.FindAsync(request.ExpenseId);
        if (expense == null || !expense.IsActive)
            return new ApiResponse("Masraf kaydı bulunamadı.");

        if (expense.ExpenseStatus != ExpenseStatus.Pending)
            return new ApiResponse("Bu kayıt zaten işlenmiş.");

        expense.ExpenseStatus = ExpenseStatus.Approved;
        expense.ProcessedAt = DateTime.Now;

        var approver = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.ApprovedBy, cancellationToken);
        expense.ProcessedByUserId = approver?.Id;

        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(RejectExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses.FindAsync(request.ExpenseId);
        if (expense == null || !expense.IsActive)
            return new ApiResponse("Masraf kaydı bulunamadı.");

        if (expense.ExpenseStatus != ExpenseStatus.Pending)
            return new ApiResponse("Bu kayıt zaten işlenmiş.");

        if (string.IsNullOrWhiteSpace(request.RejectionReason))
            return new ApiResponse("Red nedeni boş olamaz.");

        expense.ExpenseStatus = ExpenseStatus.Rejected;
        expense.RejectionReason = request.RejectionReason;
        expense.ProcessedAt = DateTime.Now;

        var rejecter = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.RejectedBy, cancellationToken);
        expense.ProcessedByUserId = rejecter?.Id;

        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }


}

