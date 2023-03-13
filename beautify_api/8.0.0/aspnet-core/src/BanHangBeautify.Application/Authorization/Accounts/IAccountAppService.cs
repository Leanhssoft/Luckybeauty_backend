using System.Threading.Tasks;
using Abp.Application.Services;
using BanHangBeautify.Authorization.Accounts.Dto;

namespace BanHangBeautify.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
