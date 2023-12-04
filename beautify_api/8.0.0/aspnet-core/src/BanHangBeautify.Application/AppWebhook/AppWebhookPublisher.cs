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
        public async Task NewUserRegisteredAsync(int tenantId, object data)
        {
            await _webHookPublisher.PublishAsync(AppWebHookNames.ZOA_UserRecieveMessage,
                new
                {
                    TenantId = tenantId
                }
            );

            await _webHookPublisher.PublishAsync(AppWebHookNames.ZOA_UserRecieveMessage, data,
              headers: new WebhookHeader()
              {
                  UseOnlyGivenHeaders = false,
                  Headers = new Dictionary<string, string>()
                  {
                      {"Key1", "Value1"},
                      {"Key3", "Value3"}
                  }
              }
            );
        }

        public async Task OnMyDataChanged(int tenantId)
        {
            await _webHookPublisher.PublishAsync(AppWebHookNames.ZOA_UserSendMessage, tenantId);
        }
    }
}
