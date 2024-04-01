using Abp;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Notifications
{
    public interface IAppNotifier
    {
        Task SendMessageAsync(string notificationName, LocalizableMessageNotificationData notificationData, List<UserIdentifier> user,
            NotificationSeverity severity = NotificationSeverity.Info);
    }
}
