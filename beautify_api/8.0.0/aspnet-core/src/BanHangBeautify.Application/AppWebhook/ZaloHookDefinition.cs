using Abp.Application.Features;
using Abp.Localization;
using Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook
{
    public class ZaloHookProvider : WebhookDefinitionProvider
    {
        public override void SetWebhooks(IWebhookDefinitionContext context)
        {
            context.Manager.Add(new WebhookDefinition(
                name: ConstAppWebHookNames.ZOA_UserSendMessage,
                displayName: L(ConstAppWebHookNames.ZOA_UserSendMessage)
            )); 
            context.Manager.Add(new WebhookDefinition(
                name: ConstAppWebHookNames.ZOA_UserRecieveMessage,
                displayName: L(ConstAppWebHookNames.ZOA_UserRecieveMessage)
            )); 
            context.Manager.Add(new WebhookDefinition(
                name: ConstAppWebHookNames.ZOA_UserSubmitInfo,
                displayName: L("UserSubmitInfor")
            ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SPAConsts.LocalizationSourceName);
        }
    }
}
