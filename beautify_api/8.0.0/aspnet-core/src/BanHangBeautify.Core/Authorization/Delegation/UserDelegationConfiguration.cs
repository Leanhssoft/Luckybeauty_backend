using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Authorization.Delegation
{
    public class UserDelegationConfiguration : IUserDelegationConfiguration
    {
        public bool IsEnabled { get; set; }

        public UserDelegationConfiguration()
        {
            IsEnabled = true;
        }
    }
}
