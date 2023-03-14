using Abp.Application.Services;
using BanHangBeautify.Sessions.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
