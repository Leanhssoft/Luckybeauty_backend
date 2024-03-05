using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook.Dto
{
    public class ZOA_Sender
    {
        public string Id { get; set; }
    }

    public class ZOA_Recipient
    {
        public string Id { get; set; }
    }

    public class ZOA_Message
    {
        public string Text { get; set; }
        [JsonProperty("msg_id")]
        public string MsgId { get; set; }
    }
    public class ZOA_InforUserSubmit
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }// tinh thanh 
        public string District { get; set; }// quan huyen
    }
    public class ZaloWebhookPayload
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("sender")]
        public ZOA_Sender Sender { get; set; }// Id của User gửi tin nhắn

        [JsonProperty("user_id_by_app")]
        public string UserIdByApp { get; set; }

        [JsonProperty("recipient")]
        public ZOA_Recipient Recipient { get; set; } // Id của Official Account nhận tin nhắn

        [JsonProperty("event_name")]
        public string EventName { get; set; }

        [JsonProperty("message")]
        public ZOA_Message Message { get; set; }
        [JsonProperty("info")]
        public ZOA_InforUserSubmit InforUserSubmit { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
