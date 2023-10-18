using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Notifications.Dto
{
    public class GetNotificationsOutput : PagedResultDto<UserCustomNotification>
    {
        public int UnreadCount { get; set; }

        public GetNotificationsOutput(int totalCount, int unreadCount, List<UserCustomNotification> notifications)
            : base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }
    }
    public class UserCustomNotification : EntityDto<Guid>, IUserIdentifier
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }
        public string Url { get; set; }
        public UserNotificationState State { get; set; }

        public NotificationCustomData Notification { set; get; }
    }
    public class NotificationCustomData : EntityDto<Guid>, IHasCreationTime
    {
        public string Content { set; get; }
        public NotificationSeverity Severity { get; set; }
        public DateTime CreationTime { set; get; }
        public string NotificationName { get; set; }

    }
    public class MessageNotification
    {
        public string SourceName { get; set; }
        public string Name { get; set; }
    }
}
