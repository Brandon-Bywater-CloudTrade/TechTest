using AutoMapper;
using UserManagement.Data.Entities;
using UserManagement.Web.Models.Logs;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Mappings;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserListItemViewModel>().ReverseMap();
        CreateMap<User, CreateUserViewModel>().ReverseMap();
        CreateMap<User, UserViewItemViewModel>().ReverseMap();
        CreateMap<User, EditUserItemViewModel>().ReverseMap();
        CreateMap<User, DeleteUserItemViewModel>().ReverseMap();

        CreateMap<LoggingEntry, LogEntryViewModel>().ReverseMap();
        CreateMap<LoggingEntry, LogEntryViewItemViewModel>().ReverseMap();
    }
}
