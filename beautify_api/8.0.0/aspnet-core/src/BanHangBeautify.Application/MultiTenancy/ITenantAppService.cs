using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BanHangBeautify.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
        public Task<PagedResultDto<TenantInfoActivityDto>> GetTenantStatusActivity(PagedTenantResultRequestDto input);
        public Task<PagedResultDto<TenantHistoryActivityDto>> GetTenantHistoryActivity(PagedRequestDto input,int tenantId);
    }
}

