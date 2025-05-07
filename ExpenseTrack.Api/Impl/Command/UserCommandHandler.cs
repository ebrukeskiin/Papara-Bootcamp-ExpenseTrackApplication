using AutoMap.Base;
using AutoMapper;
using ExpenseTrack.Api;
using ExpenseTrack.Api.Common.Helpers;
using ExpenseTrack.Api.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schema.Request;
using Schema.Response;
using static ExpenseTrack.Api.Impl.CQRS.User;

public class UserCommandHandler :
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<UpdateUserCommand, ApiResponse>,
    IRequestHandler<DeleteUserCommand, ApiResponse>
{
    private readonly MsSqlDbContext _context;
    private readonly IMapper _mapper;

    public UserCommandHandler(MsSqlDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<User>(request.User);

        // Şifre hashle
        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.User.Password);

        // IBAN otomatik oluştur
        entity.IBAN = IbanGenerator.GenerateRandomIban();

        entity.Role = UserRole.Personnel;

        entity.InsertedDate = DateTime.Now;
        entity.InsertedUser = request.InsertedUser ?? "system";
        entity.IsActive = true;

        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<UserResponse>(entity);
        return new ApiResponse<UserResponse>(response);
    }
    public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("User not found");

        if (!entity.IsActive)
            return new ApiResponse("User is not active");

        entity.FirstName = request.User.FirstName;
        entity.LastName = request.User.LastName;
        entity.Email = request.User.Email;
       // entity.Role = request.User.Role;

        if (!string.IsNullOrWhiteSpace(request.User.Password))
        {
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.User.Password);
        }

        entity.UpdatedDate = DateTime.Now;
        entity.UpdatedUser = request.UpdatedUser ?? "system";

        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("User not found");

        if (!entity.IsActive)
            return new ApiResponse("User already deleted");

        entity.IsActive = false;
        entity.UpdatedDate = DateTime.Now;
        entity.UpdatedUser = request.UpdatedUser ?? "system";

        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    // CreateUserCommand burada da olabilir (önceki yanıtla birleşebilir)
}

