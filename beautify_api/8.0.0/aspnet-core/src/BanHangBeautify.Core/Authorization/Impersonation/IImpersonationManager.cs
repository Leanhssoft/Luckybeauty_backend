using Abp.Domain.Services;
using BanHangBeautify.Authorization.Impersonation.Dto;
using System.Threading.Tasks;
namespace BanHangBeautify.Authorization.Impersonation
{
    public interface IImpersonationManager : IDomainService
    {
        Task<UserAndIdentity> GetImpersonatedUserAndIdentity(string impersonationToken);

        Task<string> GetImpersonationToken(long userId, int? tenantId);

        Task<string> GetBackToImpersonatorToken();
    }
}
