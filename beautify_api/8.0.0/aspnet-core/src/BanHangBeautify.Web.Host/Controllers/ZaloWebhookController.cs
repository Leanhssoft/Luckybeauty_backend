using Abp.AspNetCore.Mvc.Authorization;
using Abp.Webhooks;
using BanHangBeautify.AppWebhook;
using BanHangBeautify.AppWebhook.Dto;
using BanHangBeautify.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt;
using NuGet.Packaging.Signing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Web.Host.Controllers
{
    //[AbpMvcAuthorize]
    [ApiController]
    [Route("api/zalo/webhook")]
    public class ZaloWebhookController : SPAControllerBase
    {
        private readonly IWebhookSubscriptionManager _webHookSubscriptionManager;
        private readonly IConfiguration _config;
        private readonly string _zaloAppId;
        private readonly string _zaloAppSecret;
        private readonly IAppWebhookPublisher _webhookPublisher;
        public ZaloWebhookController(IWebhookSubscriptionManager webHookSubscriptionManager, IConfiguration config,
            IAppWebhookPublisher webhookPublisher)
        {
            _webHookSubscriptionManager = webHookSubscriptionManager;
            _webhookPublisher = webhookPublisher;
            _config = config;
            _zaloAppId = _config["Zalo:AppId"];
            _zaloAppSecret = _config["Zalo:AppSecret"];
        }

        /// <summary>
        /// đăng ký webhook
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("subscription")]
        public async Task<string> ZaloHookSubscription()
        {
            var webhookSubscription = new WebhookSubscription()
            {
                TenantId = AbpSession.TenantId ?? 1,
                WebhookUri = "https://api.luckybeauty.vn/api/zalo/webhook/user-send-message", // webhook endpoint
                Webhooks = new List<string>()
         {
            ConstAppWebHookNames.ZOA_UserSendMessage, // ds các webhook có cùng thông tin nhận, gửi
           ConstAppWebHookNames.ZOA_UserSubmitInfo
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

        [HttpPost]
        [Route("user-send-message")]
        public async Task<IActionResult> UserSendMessage()
        {
            //try
            //{
            using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();

                if (!IsSignatureCompatible2(body))
                {
                    return StatusCode(500, "Unexpected Signature");
                }

                // Xử lý dữ liệu từ Zalo sau khi xác thực chữ ký
                var zaloEventData = JsonConvert.DeserializeObject<ZaloWebhookPayload>(body);

                // Xử lý sự kiện cụ thể
                switch (zaloEventData.EventName)
                {
                    case "user_send_text":
                        // Xử lý tin nhắn từ người dùng
                        var message = zaloEventData.Message;
                        break;
                    case "user_submit_info":
                        {
                            var inforUser = zaloEventData.InforUserSubmit;
                            // 1. check exist tbl DM_KhachHang (by Phone) & insert to DM_KhachHang
                            // 2. Insert into Zalo_KhachHangThanhVien
                            try
                            {
                                await _webhookPublisher.UserSendMessage(inforUser, zaloEventData.Sender.Id);
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                        break;
                    default:
                        // Xử lý cho các trường hợp khác nếu cần
                        break;
                }
            }

            return Ok();
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, ex.Message);
            //}
        }

        private bool IsSignatureCompatible2(string body)
        {
            if (!HttpContext.Request.Headers.ContainsKey("X-ZEvent-Signature"))
            {
                return false;
            }

            long timeStamp = 54390853474;
            string string_body = JsonConvert.SerializeObject(body);
            string raw_verify = $"{_zaloAppId}{string_body}{timeStamp}{_zaloAppSecret}";

            // mac = sha256(appId + data + timeStamp + OAsecretKey)
            string secret = HttpContext.Request.Headers["X-ZEvent-Signature"];
            var receivedSignature = secret.ToString().Split("=");

            string computedSignature;
            switch (receivedSignature[0])
            {
                case "mac":
                    // Chuyển đổi chuỗi thành mảng byte
                    byte[] bytes = Encoding.UTF8.GetBytes(raw_verify);
                    // Tạo đối tượng SHA256
                    byte[] hash = SHA256.HashData(bytes);
                    // Chuyển đổi mảng byte thành chuỗi hex
                    computedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
                    break;
                default:
                    throw new NotImplementedException();
            }
            return computedSignature == receivedSignature[1];
        }
        private bool IsSignatureCompatible(string body)
        {
            if (!HttpContext.Request.Headers.ContainsKey("X-ZEvent-Signature"))
            {
                return false;
            }

            // mac = sha256(appId + data + timeStamp + OAsecretKey)
            var receivedSignature = HttpContext.Request.Headers["X-ZEvent-Signature"].ToString().Split("=");//will be something like "sha256=whs_XXXXXXXXXXXXXX"

            string computedSignature;
            switch (receivedSignature[0])
            {
                case "sha256":
                    var secretBytes = Encoding.UTF8.GetBytes(_zaloAppSecret);// chuyen doi chuoi thanh byte
                    using (var hasher = new HMACSHA256(secretBytes))// tao ma Hash với khóa bí mật
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
