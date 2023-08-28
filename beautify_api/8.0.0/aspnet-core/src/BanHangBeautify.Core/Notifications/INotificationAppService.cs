using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Notifications;
using BanHangBeautify.Notifications.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Notifications
{
    public interface INotificationAppService: IApplicationService
    {
        Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input);

        Task SetAllNotificationsAsRead();

        Task SetNotificationAsRead(EntityDto<Guid> input);

        Task<GetNotificationSettingsOutput> GetNotificationSettings();

        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);

        Task DeleteNotification(EntityDto<Guid> input);

        Task DeleteAllUserNotifications(DeleteAllUserNotificationsInput input);
        Task SendMessageAsync(string notificationName, LocalizableMessageNotificationData notificationData, List<UserIdentifier> user,
            NotificationSeverity severity = NotificationSeverity.Info);
    }
}
