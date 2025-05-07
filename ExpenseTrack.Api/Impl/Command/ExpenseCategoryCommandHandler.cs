using AutoMap.Base;
using AutoMapper;
using ExpenseTrack.Api.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema.Response;
using static ExpenseTrack.Api.Impl.CQRS.ExpenseCategory;

namespace ExpenseTrack.Api.Impl.Command
{
        public class ExpenseCategoryCommandHandler :
        IRequestHandler<CreateExpenseCategoryCommand, ApiResponse<ExpenseCategoryResponse>>,
        IRequestHandler<UpdateExpenseCategoryCommand, ApiResponse>,
        IRequestHandler<DeleteExpenseCategoryCommand, ApiResponse>
        {
           private readonly MsSqlDbContext _context;
           private readonly IMapper _mapper;

        public ExpenseCategoryCommandHandler(MsSqlDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<ExpenseCategory>(request.Category);
                entity.InsertedDate = DateTime.Now;
            //entity.InsertedUser =User.Identity?.Name;
            entity.InsertedUser = string.IsNullOrWhiteSpace(request.InsertedUser)
  ? "system"
  : request.InsertedUser;
            entity.IsActive = true;

                await _context.ExpenseCategories.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<ExpenseCategoryResponse>(entity);
                return new ApiResponse<ExpenseCategoryResponse>(response);
            }

            public async Task<ApiResponse> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.ExpenseCategories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entity == null)
                    return new ApiResponse("Kategori bulunamadı");

                if (!entity.IsActive)
                    return new ApiResponse("Kategori pasif durumda");

                entity.Name = request.Category.Name;
                entity.Description = request.Category.Description;
                entity.UpdatedDate = DateTime.Now;
                //entity.UpdatedUser = request.UpdatedUser ?? "system";

                await _context.SaveChangesAsync(cancellationToken);
                return new ApiResponse();
            }

            public async Task<ApiResponse> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.ExpenseCategories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entity == null)
                    return new ApiResponse("Kategori bulunamadı");

                if (!entity.IsActive)
                    return new ApiResponse("Kategori zaten pasif");

                entity.IsActive = false;
                entity.UpdatedDate = DateTime.Now;
               //entity.UpdatedUser = request.UpdatedUser ?? "system";

                await _context.SaveChangesAsync(cancellationToken);
                return new ApiResponse();
            }
        }
}

