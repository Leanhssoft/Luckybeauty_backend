using Abp.Domain.Services;
using Abp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Authorization.Delegation
{
    public interface IUserDelegationManager : IDomainService
    {
        Task<bool> HasActiveDelegationAsync(long sourceUserId, long targetUserId);

        bool HasActiveDelegation(long sourceUserId, long targetUserId);

        Task RemoveDelegationAsync(long userDelegationId, UserIdentifier currentUser);

        Task<UserDelegation> GetAsync(long userDelegationId);
    }
}
