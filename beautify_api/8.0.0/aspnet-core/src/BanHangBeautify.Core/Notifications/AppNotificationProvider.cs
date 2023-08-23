using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Notifications
{
    public class AppNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    "App.NewUserRegistered",
                    displayName: new LocalizableString("NewUserRegisteredNotificationDefinition", "MyLocalizationSourceName"),
                    permissionDependency: new SimplePermissionDependency("App.Pages.UserManagement")
                    )
                );
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SPAConsts.LocalizationSourceName);
        }
    }
}
