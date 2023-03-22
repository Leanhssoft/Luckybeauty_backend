using Abp.Application.Services;
using BanHangBeautify.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
