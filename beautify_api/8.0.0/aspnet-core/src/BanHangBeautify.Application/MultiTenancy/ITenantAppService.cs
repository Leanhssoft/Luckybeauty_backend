using Abp.Application.Services;
using BanHangBeautify.MultiTenancy.Dto;

namespace BanHangBeautify.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {

    }
}

