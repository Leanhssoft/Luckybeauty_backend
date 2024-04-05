using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Authorization.Delegation
{
    public interface IUserDelegationConfiguration
    {
        bool IsEnabled { get; set; }
    }
}
