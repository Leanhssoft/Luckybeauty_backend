using Abp.Dependency;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SignalR.Notifications
{
    public class NotificationHub: Hub,ITransientDependency
    {
    }
}
