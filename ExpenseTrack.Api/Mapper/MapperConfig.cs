using AutoMapper;
using ExpenseTrack.Api.Domain;
using Schema.Request;
using Schema.Response;


namespace ExpenseTrack.Api.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<ExpenseCategoryRequest, ExpenseCategory>();
        CreateMap<ExpenseCategory, ExpenseCategoryResponse>();

        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Hash işlemi handler’da yapılır
            .ForMember(dest => dest.Role, opt => opt.Ignore())         // Rol sistemde atanır
            .ForMember(dest => dest.IBAN, opt => opt.Ignore());        // IBAN sistemde atanır

        CreateMap<ExpenseDocumentRequest, ExpenseDocument>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<ExpenseDocument, ExpenseDocumentResponse>();

        CreateMap<ExpenseRequest, Expense>();
        CreateMap<Expense, ExpenseResponse>();
    }
}
