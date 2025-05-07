using AutoMap.Base;
using AutoMapper;
using ExpenseTrack.Api.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema.Response;
using static ExpenseTrack.Api.Impl.CQRS.ExpenseDocument;

namespace ExpenseTrack.Api.Impl.Command
{
   public class ExpenseDocumentCommandHandler :
      IRequestHandler<CreateExpenseDocumentCommand, ApiResponse<ExpenseDocumentResponse>>,
     // IRequestHandler<UpdateExpenseDocumentCommand, ApiResponse>,
      IRequestHandler<DeleteExpenseDocumentCommand, ApiResponse>
        {
            private readonly MsSqlDbContext _context;
            private readonly IMapper _mapper;
            private readonly IWebHostEnvironment _env;

        public ExpenseDocumentCommandHandler(MsSqlDbContext context, IMapper mapper, IWebHostEnvironment env)
            {
                 _env = env;
                 _context = context;
                _mapper = mapper;
            }

        public async Task<ApiResponse<ExpenseDocumentResponse>> Handle(CreateExpenseDocumentCommand request, CancellationToken cancellationToken)
        {
            var file = request.File;

        if (file == null || file.Length == 0)
            return new ApiResponse<ExpenseDocumentResponse>("Dosya boş olamaz.");

        // 1. Dosya klasörünü hazırla
        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // 2. Dosya adı ve yol
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var fullPath = Path.Combine(uploadsFolder, fileName);

        // 3. Dosyayı diske yaz
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // 4. Entity oluştur
        var entity = new ExpenseDocument
        { 
            FileName = file.FileName,
            FileType = file.ContentType,
            FilePath = $"/uploads/{fileName}",
            UploadedAt = DateTime.Now,
            InsertedDate = DateTime.Now,
          //  InsertedUser = request.InsertedUser ?? "system",
            IsActive = true
        };

        await _context.ExpenseDocuments.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<ExpenseDocumentResponse>(entity);
        return new ApiResponse<ExpenseDocumentResponse>(response);
        }
       

        public async Task<ApiResponse> Handle(DeleteExpenseDocumentCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.ExpenseDocuments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entity == null)
                    return new ApiResponse("Belge bulunamadı");

                if (!entity.IsActive)
                    return new ApiResponse("Belge zaten silinmiş");

                entity.IsActive = false;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedUser = request.UpdatedUser ?? "system";

                await _context.SaveChangesAsync(cancellationToken);
                return new ApiResponse();
            }
        }

}

