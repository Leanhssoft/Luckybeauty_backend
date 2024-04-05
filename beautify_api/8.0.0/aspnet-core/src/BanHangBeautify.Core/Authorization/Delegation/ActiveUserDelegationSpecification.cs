using Abp.Specifications;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Authorization.Delegation
{
    public class ActiveUserDelegationSpecification : Specification<UserDelegation>
    {
        public long SourceUserId { get; }

        public long TargetUserId { get; }

        public ActiveUserDelegationSpecification(long sourceUserId, long targetUserId)
        {
            SourceUserId = sourceUserId;
            TargetUserId = targetUserId;
        }

        public override Expression<Func<UserDelegation, bool>> ToExpression()
        {
            var now = Clock.Now;
            return (e) => (e.SourceUserId == SourceUserId &&
                           e.TargetUserId == TargetUserId &&
                           e.StartTime <= now && e.EndTime >= now);
        }
    }
}
