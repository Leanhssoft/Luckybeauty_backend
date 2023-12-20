using Abp.Runtime.Session;
using Abp.Webhooks;
using BanHangBeautify.Zalo.KetNoi_XacThuc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook
{
    public class WebhookAppService : SPAAppServiceBase, IWebhookAppService
    {
        private readonly IWebhookSubscriptionManager _webHookSubscriptionManager;

        private readonly IConfiguration _config;
        private readonly string _zaloAppId;
        private readonly string _zaloAppSecret;

        public WebhookAppService(IWebhookSubscriptionManager webHookSubscriptionManager, IConfiguration config)
        {
            _webHookSubscriptionManager = webHookSubscriptionManager;
            _config = config;
            _zaloAppId = _config["Zalo:AppId"];
            _zaloAppSecret = _config["Zalo:AppSecret"];
        }

        public async Task<string> ZaloHookSubscription()
        {
            var webhookSubscription = new WebhookSubscription()
            {
                WebhookUri = "https://localhost:44311/api/services/app/Webhook/RecieveMessageFromUser", // webhook endpoint
                Webhooks = new List<string>()
         {
            ConstAppWebHookNames.ZOA_UserSendMessage, // ds các webhook có cùng thông tin nhận, gửi
           ConstAppWebHookNames.ZOA_UserRecieveMessage
         },
                Headers = new Dictionary<string, string>()
         {
             { "Content-Type", "application/json" },
             { "appId", _zaloAppId},
             { "OAsecretKey", _zaloAppSecret },
         }
            };
            await _webHookSubscriptionManager.AddOrUpdateSubscriptionAsync(webhookSubscription);
            return webhookSubscription.Secret;
        }

        [HttpGet]
        public async Task SendMessage()
        {
            var timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var param = new
            {
                app_id = "360846524940903967",
                sender = new
                {
                    id = "246845883529197922",// id cua user gui tin (khach hang)
                },
                //user_id_by_app = "552177279717587730",
                recipient = new
                {
                    id = "4350973059872913745", // Id của Official Account nhận tin nhắn
                },
                event_name = "user_send_text",
                message = new
                {
                    text = "message",
                    msg_id = "96d3cdf3af150460909"
                },
                timestamp = timeStamp
            };
            var json = JsonSerializer.Serialize(param);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new();
            string url = $"https://localhost:44311/api/services/app/Webhook/RecieveMessageFromUser";
            using HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Content = stringContent;
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var newToken = JsonSerializer.Deserialize<ZaloToken>(apiResponse);
            }
        }

        /// <summary>
        /// test webhook recieve data
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<bool> RecieveMessageFromUser1(HttpContext httpContext)
        {
            var webHookSecret = await ZaloHookSubscription();

            using StreamReader reader = new(httpContext.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            if (!IsSignatureCompatible(httpContext, webHookSecret, body))//read webhooksecret from user secret
            {
                throw new Exception("Unexpected Signature");
            }
            //It is certain that Webhook has not been modified.
            return true;
        }

        [HttpPost]
        public async Task<bool> RecieveMessageFromUser()
        {
            //It is certain that Webhook has not been modified.
            var xx = await ZaloHookSubscription();
            return true;
        }

        private bool IsSignatureCompatible(HttpContext httpContext, string secret, string body)
        {
            if (httpContext.Request.Headers.ContainsKey("X-ZEvent-Signature"))// key zalo: X-ZEvent-Signature
            {
                return false;
            }
            // mac = sha256(appId + data + timeStamp + OAsecretKey)
            var receivedSignature = httpContext.Request.Headers["X-ZEvent-Signature"].ToString().Split("=");//will be something like "sha256=whs_XXXXXXXXXXXXXX"

            string computedSignature;
            switch (receivedSignature[0])
            {
                case "sha256":
                    var secretBytes = Encoding.UTF8.GetBytes(secret);// chuyen doi chuoi thanh byte
                    using (var hasher = new HMACSHA256(secretBytes))// tao ma Hash
                    {
                        var data = Encoding.UTF8.GetBytes(body);
                        // ComputeHash(data) Tính toán giá trị hash của dữ liệu --> tra ve mang byte
                        // 1 byte trong mang duoc chuyển đổi thành một chuỗi hex,
                        // và các chuỗi hex này được nối lại với nhau, thường được phân tách bằng dấu gạch nối.
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
