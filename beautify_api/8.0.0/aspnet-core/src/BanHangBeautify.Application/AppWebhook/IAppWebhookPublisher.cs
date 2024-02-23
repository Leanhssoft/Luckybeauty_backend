using BanHangBeautify.AppWebhook.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook
{
    public interface IAppWebhookPublisher
    {
        Task UserSendMessage(ZOA_InforUserSubmit userInfor, string zaloUserId);
        Task UserRecieveMessage(int tenantId);
    }
}
