using Abp;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Notifications
{
    public class AppNotifier : BanHangBeautifyDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;
        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }
        public async Task SendMessageAsync(string notificationName, LocalizableMessageNotificationData notificationData, List<UserIdentifier> user,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(notificationName,
                notificationData,
                severity: severity,
                userIds: user.ToArray()
            );
        }
        protected async Task SendNotificationAsync(string notificationName, UserIdentifier user,
           LocalizableString localizableMessage, IDictionary<string, object> localizableMessageData = null,
           NotificationSeverity severity = NotificationSeverity.Info)
        {
            var notificationData = new LocalizableMessageNotificationData(localizableMessage);
            if (localizableMessageData != null)
            {
                foreach (var pair in localizableMessageData)
                {
                    notificationData[pair.Key] = pair.Value;
                }
            }

            await _notificationPublisher.PublishAsync(notificationName, notificationData, severity: severity,
                userIds: new[] { user });
        }
    }
}
