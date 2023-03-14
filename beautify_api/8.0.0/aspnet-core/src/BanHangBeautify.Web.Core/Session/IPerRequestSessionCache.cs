using BanHangBeautify.Sessions.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
