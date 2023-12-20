using Abp.Domain.Services;
using Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook
{
    public class AppWebhookPublisher : DomainService //, IAppWebhookPublisher
    {
        private readonly IWebhookPublisher _webHookPublisher;
        public AppWebhookPublisher(IWebhookPublisher webHookPublisher)
        {
            _webHookPublisher = webHookPublisher; 
        }

        /// <summary>
        /// chi nhung tenant co ket noi zalo OA thi moi public
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task RecieveMessageFromUser(int tenantId)
        {
            await _webHookPublisher.PublishAsync(ConstAppWebHookNames.ZOA_UserRecieveMessage, tenantId);
        }

        public async Task KhachHangGuiTinNhan(int tenantId)
        {
            await _webHookPublisher.PublishAsync(ConstAppWebHookNames.ZOA_UserSendMessage, tenantId);
        }
    }
}
