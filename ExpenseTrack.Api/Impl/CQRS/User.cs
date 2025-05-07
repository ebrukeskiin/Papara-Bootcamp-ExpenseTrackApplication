using AutoMap.Base;
using MediatR;
using Schema.Request;
using Schema.Response;

namespace ExpenseTrack.Api.Impl.CQRS
{
    public class User
    {
        public class CreateUserCommand : IRequest<ApiResponse<UserResponse>>
        {
            public UserRequest User { get; set; }
            public string? InsertedUser { get; set; }
        }
        public class UpdateUserCommand : IRequest<ApiResponse>
        {
            public long Id { get; set; }
            public UserRequest User { get; set; }
            public string? UpdatedUser { get; set; }
        }
        public class DeleteUserCommand : IRequest<ApiResponse>
        {
            public long Id { get; set; }
            public string? UpdatedUser { get; set; }
        }
        public class GetAllUsersQuery : IRequest<ApiResponse<List<UserResponse>>>
        {
        }
        public class GetUserByIdQuery : IRequest<ApiResponse<UserResponse>>
        {
            public long Id { get; set; }
        }
    }
}
