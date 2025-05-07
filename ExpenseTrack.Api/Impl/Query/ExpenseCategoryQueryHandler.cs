using AutoMap.Base;
using AutoMapper;
using ExpenseTrack.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema.Response;
using static ExpenseTrack.Api.Impl.CQRS.ExpenseCategory;


public class ExpenseCategoryQueryHandler :
    IRequestHandler<GetAllExpenseCategoriesQuery, ApiResponse<List<ExpenseCategoryResponse>>>,
    IRequestHandler<GetExpenseCategoryByIdQuery, ApiResponse<ExpenseCategoryResponse>>
{
    private readonly MsSqlDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseCategoryQueryHandler(MsSqlDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ExpenseCategoryResponse>>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.ExpenseCategories
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        var response = _mapper.Map<List<ExpenseCategoryResponse>>(entities);
        return new ApiResponse<List<ExpenseCategoryResponse>>(response);
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ExpenseCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity == null)
            return new ApiResponse<ExpenseCategoryResponse>("Kategori bulunamadı");

        var response = _mapper.Map<ExpenseCategoryResponse>(entity);
        return new ApiResponse<ExpenseCategoryResponse>(response);
    }
}
