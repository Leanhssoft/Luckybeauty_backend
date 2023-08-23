using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Notifications.Dto
{
    public class GetNotificationSettingsOutput
    {
        public bool ReceiveNotifications { get; set; }

        public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
    }
    public class NotificationSubscriptionWithDisplayNameDto : NotificationSubscriptionDto
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
