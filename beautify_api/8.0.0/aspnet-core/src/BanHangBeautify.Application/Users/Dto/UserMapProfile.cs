using AutoMapper;
using BanHangBeautify.Authorization.Users;

namespace BanHangBeautify.Users.Dto
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<UserDto, User>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore()).ReverseMap();

            CreateMap<CreateUserDto, User>();
            CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
            CreateMap<UpdateUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore()).ReverseMap();
        }
    }
}
