using Abp.Runtime.Session;
using Abp.Webhooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook
{
    public class WebhookAppService : SPAAppServiceBase
    {
        private readonly IWebhookSubscriptionManager _webHookSubscriptionManager;

        private readonly HttpContext _http;
        private readonly IConfiguration _config;
        private readonly string _zaloAppId;
        private readonly string _zaloAppSecret;

        public WebhookAppService(IWebhookSubscriptionManager webHookSubscriptionManager, HttpContext http, IConfiguration config)
        {
            _webHookSubscriptionManager = webHookSubscriptionManager;
            _http = http;
            _config = config;
            _zaloAppId = _config["Zalo:AppId"];
            _zaloAppSecret = _config["Zalo:AppSecret"];
        }

        public async Task<string> ZaloHookSubscription()
        {
            var webhookSubscription = new WebhookSubscription()
            {
                TenantId = AbpSession.TenantId,
                WebhookUri = "http://localhost:44311/api/services/app/Webhook/RecieveMessageFromUser",
                Webhooks = new List<string>()
         {
            AppWebHookNames.ZOA_UserSendMessage, // ds các webhook có cùng thông tin nhận, gửi
           AppWebHookNames.ZOA_UserRecieveMessage
         },
                Headers = new Dictionary<string, string>()
         {
             { "Content-Type", "application/json" },
         }
            };
            await _webHookSubscriptionManager.AddOrUpdateSubscriptionAsync(webhookSubscription);
            return webhookSubscription.Secret;
        }

        [HttpPost]
        public async Task RecieveMessageFromUser()
        {
            var webHookSecret = await ZaloHookSubscription();
            using StreamReader reader = new(_http.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            if (!IsSignatureCompatible(webHookSecret, body))//read webhooksecret from user secret
            {
                throw new Exception("Unexpected Signature");
            }
            //It is certain that Webhook has not been modified.

        }

        private bool IsSignatureCompatible(string secret, string body) 
        {
            if (!_http.Request.Headers.ContainsKey("X-ZEvent-Signature"))// key zalo: X-ZEvent-Signature
            {
                return false;
            }
            // mac = sha256(appId + data + timeStamp + OAsecretKey)
            var receivedSignature = _http.Request.Headers["X-ZEvent-Signature"].ToString().Split("=");//will be something like "sha256=whs_XXXXXXXXXXXXXX"
                                                                                                               //It starts with hash method name (currently "sha256") then continue with signature. You can also check if your hash method is true.

            string computedSignature;
            switch (receivedSignature[0])
            {
                case "sha256":
                    var secretBytes = Encoding.UTF8.GetBytes(secret);
                    using (var hasher = new HMACSHA256(secretBytes))
                    {
                        var data = Encoding.UTF8.GetBytes(body);
                        computedSignature = BitConverter.ToString(hasher.ComputeHash(data));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return computedSignature == receivedSignature[1];
        }
    }
}
