using System.Threading.Tasks;
using Abp.Application.Services;
using BanHangBeautify.Sessions.Dto;

namespace BanHangBeautify.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
