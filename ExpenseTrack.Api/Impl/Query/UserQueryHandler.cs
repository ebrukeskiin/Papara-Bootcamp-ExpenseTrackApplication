using AutoMap.Base;
using AutoMapper;
using ExpenseTrack.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schema.Response;
using static ExpenseTrack.Api.Impl.CQRS.User;

public class UserQueryHandler :
    IRequestHandler<GetAllUsersQuery, ApiResponse<List<UserResponse>>>,
    IRequestHandler<GetUserByIdQuery, ApiResponse<UserResponse>>
{
    private readonly MsSqlDbContext _context;
    private readonly IMapper _mapper;

    public UserQueryHandler(MsSqlDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Where(u => u.IsActive)
            .ToListAsync(cancellationToken);

        var response = _mapper.Map<List<UserResponse>>(users);
        return new ApiResponse<List<UserResponse>>(response);
    }

    public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id && u.IsActive, cancellationToken);

        if (user == null)
            return new ApiResponse<UserResponse>("Kullanıcı bulunamadı");

        var response = _mapper.Map<UserResponse>(user);
        return new ApiResponse<UserResponse>(response);
    }
}
