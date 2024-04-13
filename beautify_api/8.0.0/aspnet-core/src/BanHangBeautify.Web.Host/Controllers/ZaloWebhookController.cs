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

                // Xử lý dữ liệu từ Zalo sau khi xác thực chữ ký
                var zaloEventData = JsonConvert.DeserializeObject<ZaloWebhookPayload>(body);

                if (!IsSignatureCompatible(body, zaloEventData.Timestamp))
                {
                    string string_body = JsonConvert.SerializeObject(body);
                    string raw_verify = $"{_zaloAppId}{string_body}{zaloEventData.Timestamp}{_zaloAppSecret}";

                    // mac = sha256(appId + data + timeStamp + OAsecretKey)
                    string secret = HttpContext.Request.Headers["X-ZEvent-Signature"];
                    var receivedSignature = secret.ToString().Split("=");
                    if (receivedSignature.Length > 1)
                    {
                        return StatusCode(500, receivedSignature[0] + receivedSignature[1]);
                    }
                    return StatusCode(500, string_body);
                }

                // Xử lý sự kiện cụ thể
                switch (zaloEventData.EventName)
                {
                    case "user_send_text":
                        {
                            // Xử lý tin nhắn từ người dùng
                            var message = zaloEventData.Message;
                            // check exists userid
                            // if not exists: insert to zalo_khachang thanhvien + khachhang
                        }
                        break;
                    case "user_submit_info":
                        {
                            try
                            {
                                var inforUser = zaloEventData.InforUserSubmit;
                                var idKhachHangZOA = await _webhookPublisher.AddUpdate_ZaloKhachHangThanhVien(zaloEventData.Sender.Id);
                                if (idKhachHangZOA != null)
                                {
                                    await _webhookPublisher.AddNewCustomer_ShareInfor(idKhachHangZOA ?? Guid.Empty, inforUser);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        break;
                    case "follow":
                    case "unfollow":
                        {
                            try
                            {
                                await _webhookPublisher.AddUpdate_ZaloKhachHangThanhVien(zaloEventData.Sender.Id);
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

        private bool IsSignatureCompatible2(string body, long timeStamp)
        {
            if (!HttpContext.Request.Headers.ContainsKey("X-ZEvent-Signature"))
            {
                return false;
            }

            //DateTime currentTime = DateTime.Now;
            //long timeStamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

            string string_body = JsonConvert.SerializeObject(body);
            string raw_verify = $"{_zaloAppId}{string_body.Trim()}{timeStamp}{_zaloAppSecret}";

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
                    computedSignature = BitConverter.ToString(hash).Replace("-", "");
                    break;
                default:
                    throw new NotImplementedException();
            }
            return computedSignature == receivedSignature[1];
        }
        private bool IsSignatureCompatible(string body, long timeStamp)
        {
            if (!HttpContext.Request.Headers.ContainsKey("X-ZEvent-Signature"))
            {
                return false;
            }

            string string_body = JsonConvert.SerializeObject(body);
            string raw_verify = $"{_zaloAppId}{string_body.Trim()}{timeStamp}{_zaloAppSecret}";

            // mac = sha256(appId + data + timeStamp + OAsecretKey)
            string secret = HttpContext.Request.Headers["X-ZEvent-Signature"];
            var receivedSignature = secret.ToString().Split("=");
            string computedSignature;
            switch (receivedSignature[0])
            {
                case "mac":
                    // Convert the input string to a byte array and compute the hash.
                    byte[] inputBytes = Encoding.UTF8.GetBytes(raw_verify);
                    byte[] hashBytes = SHA256.HashData(inputBytes);

                    // Convert the byte array to a hexadecimal string.
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        builder.Append(hashBytes[i].ToString("x2"));
                    }
                    computedSignature = builder.ToString();
                    break;
                default:
                    throw new NotImplementedException();
            }
            return computedSignature == receivedSignature[1];
        }
    }
}
