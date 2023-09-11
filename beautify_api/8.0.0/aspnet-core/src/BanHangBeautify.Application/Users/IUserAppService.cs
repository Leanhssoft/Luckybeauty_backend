using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BanHangBeautify.Roles.Dto;
using BanHangBeautify.Users.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task DeActivate(EntityDto<long> user);
        Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
