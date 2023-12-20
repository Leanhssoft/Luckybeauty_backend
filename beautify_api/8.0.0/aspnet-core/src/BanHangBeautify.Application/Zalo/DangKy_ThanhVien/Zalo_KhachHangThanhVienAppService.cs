using Abp.Domain.Repositories;
using Abp.Webhooks;
using BanHangBeautify.AppWebhook;
using BanHangBeautify.Entities;
using BanHangBeautify.Zalo.DangKyThanhVien;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BanHangBeautify.Zalo.DangKy_ThanhVien
{
    public class Zalo_KhachHangThanhVienAppService : SPAAppServiceBase
    {
        private readonly IRepository<Zalo_KhachHangThanhVien, Guid> _zaloKhachHangThanhVien;
        private readonly IConfiguration _config;

        public Zalo_KhachHangThanhVienAppService(IRepository<Zalo_KhachHangThanhVien, Guid> zaloKhachHangThanhVien, IConfiguration config)
        {
            _zaloKhachHangThanhVien = zaloKhachHangThanhVien;
            _config = config;
        }
        [HttpPost]
        public Zalo_KhachHangThanhVienDto DangKyThanhVienZOA(Zalo_KhachHangThanhVienDto dto)
        {
            if (dto == null) { return new Zalo_KhachHangThanhVienDto(); };
            Zalo_KhachHangThanhVien objNew = ObjectMapper.Map<Zalo_KhachHangThanhVien>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreationTime = DateTime.Now;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.IsDeleted = false;
            _zaloKhachHangThanhVien.InsertAsync(objNew);
            var result = ObjectMapper.Map<Zalo_KhachHangThanhVienDto>(objNew);
            return result;
        }

        /// <summary>
        /// test webhook send data
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task KhachHangGuiTinNhan()
        {
            // X-ZEvent-Signature:mac = sha256(appId + data + timeStamp + OAsecretKey)
            string appId = _config["Zalo:AppId"];
            string appSecret = _config["Zalo:AppSecret"];
            string oaId = _config["Zalo:OaId"];
            long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var jsonData = new
            {
                app_id = appId,
                event_name = "user_send_text",
                timeStamp = timestamp,
                sender = new
                {
                    id = "6441788310775550433", // id của User gửi tin nhắn               
                },
                recipient = new
                {
                    id = oaId,// Id của Official Account nhận tin nhắn            
                },
                message = new
                {
                    text = "test send mes to webhook",
                    msg_id = "96d3cdf3af150460909" // gán tạm id lung tung
                }
            };

            // X-ZEvent-Signature:mac = sha256(appId + data + timeStamp + OAsecretKey)
            string jsonPayload = JsonConvert.SerializeObject(jsonData);
            string mac = sha256($"{appId}{jsonPayload}{timestamp}{appSecret}");

            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");
            var apiUrl = "https://localhost:44311/api/services/app/Webhook/RecieveMessageFromUser";

            try
            {
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("X-ZEvent-Signature", mac);
                client.DefaultRequestHeaders.Add("OAsecretKey", appSecret);

                // Make an HTTP POST request with the JSON payload
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response: " + responseBody);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            //await this.NotifyAsync(AppWebHookNames.ZOA_UserSendMessage, new { P1 = "p1" });
        }

        private string sha256(string body)
        {
            string computedSignature;
            var secretBytes = Encoding.UTF8.GetBytes(body);// chuyen doi chuoi thanh byte
            using (var hasher = new HMACSHA256(secretBytes))// tao ma Hash
            {
                var data = Encoding.UTF8.GetBytes(body);
                // ComputeHash(data) Tính toán giá trị hash của dữ liệu --> tra ve mang byte
                // 1 byte trong mang duoc chuyển đổi thành một chuỗi hex,
                // và các chuỗi hex này được nối lại với nhau, thường được phân tách bằng dấu gạch nối.
                computedSignature = BitConverter.ToString(hasher.ComputeHash(data));
            }
            return computedSignature;
        }
    }
}
