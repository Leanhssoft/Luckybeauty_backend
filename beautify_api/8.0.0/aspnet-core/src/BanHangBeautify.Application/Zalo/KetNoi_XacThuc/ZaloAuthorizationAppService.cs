using Abp.Domain.Repositories;
using BanHangBeautify.Configuration;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.POIFS.Crypt;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.KetNoi_XacThuc
{
    public class ZaloAuthorizationAppService : SPAAppServiceBase
    {
        private readonly IRepository<ZaloAuthorization, Guid> _zaloAuthorization;
        private readonly IConfiguration _config;
        public ZaloAuthorizationAppService(IRepository<ZaloAuthorization, Guid> zaloAuthorization, IConfiguration config)
        {
            _zaloAuthorization = zaloAuthorization;
            _config = config;
        }

        [HttpPost]
        public ZaloAuthorizationDto Insert(ZaloAuthorizationDto dto)
        {
            if (dto == null) { return new ZaloAuthorizationDto(); };
            ZaloAuthorization objNew = ObjectMapper.Map<ZaloAuthorization>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreationTime = DateTime.Now;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.IsDeleted = false;
            _zaloAuthorization.InsertAsync(objNew);
            var result = ObjectMapper.Map<ZaloAuthorizationDto>(objNew);
            return result;
        }
        [HttpPost]
        public async Task<string> Update(ZaloAuthorizationDto dto)
        {
            try
            {
                if (dto == null) { return "Data null"; };
                ZaloAuthorization objUp = await _zaloAuthorization.FirstOrDefaultAsync(dto.Id ?? Guid.Empty);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.CodeVerifier = dto.CodeVerifier;
                objUp.CodeChallenge = dto.CodeChallenge;
                objUp.AuthorizationCode = dto.AuthorizationCode;
                objUp.AccessToken = dto.AccessToken;
                objUp.RefreshToken = dto.RefreshToken;
                objUp.ExpiresToken = dto.ExpiresToken;
                objUp.LastModificationTime = DateTime.Now;
                objUp.LastModifierUserId = AbpSession.UserId;
                await _zaloAuthorization.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }

        [HttpGet]
        public async Task<string> XoaKetNoi(Guid Id)
        {
            try
            {
                ZaloAuthorization objUp = await _zaloAuthorization.FirstOrDefaultAsync(Id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.DeleterUserId = AbpSession.TenantId ?? 1;
                objUp.DeletionTime = DateTime.Now;
                objUp.IsDeleted = true;
                await _zaloAuthorization.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
        /// <summary>
        /// mỗi refreshToken chỉ dùng 1 lần, sau đó sẽ không dùng dc nữa
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<ZaloToken> GetNewAccessToken_fromRefreshToken(string refreshToken)
        {
            try
            {
                string appId = _config["Zalo:AppId"];
                string appSecret = _config["Zalo:AppSecret"];
                //var json = JsonSerializer.Serialize(new { app_id = appId, refresh_token = refreshToken, grant_type = "refresh_token" });

                // Tạo nội dung của body
                Dictionary<string, string> postParams = new()
                {
                    { "app_id", appId },
                    { "refresh_token", refreshToken },
                    { "grant_type", "refresh_token" }
                };

                var body = new FormUrlEncodedContent(postParams).ReadAsStringAsync().Result;
                // Chuyển đổi body thành chuỗi x-www-form-urlencoded
                var stringContent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpClient client = new();
                string url = $"https://oauth.zaloapp.com/v4/oa/access_token";

                using HttpRequestMessage request = new(HttpMethod.Post, url);
                request.Headers.Add("secret_key", appSecret);
                request.Content = stringContent;

                HttpResponseMessage response = client.SendAsync(request).Result;
                // Kiểm tra xem yêu cầu có thành công hay không
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var newToken = JsonSerializer.Deserialize<ZaloToken>(apiResponse);
                    return newToken;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<HttpResponseMessage> SendMessageToUser(string accessToken)
        {
            try
            {
                string appId = _config["Zalo:AppId"];
                string appSecret = _config["Zalo:AppSecret"];

                dynamic objJson = new ExpandoObject();
                objJson.recipient = new ExpandoObject();
                objJson.recipient.user_id = "6441788310775550433";

                objJson.message = new ExpandoObject();
                objJson.message.text = "send from c# nhuongdt";

                var json = JsonSerializer.Serialize(objJson);


                HttpClient client = new();
                const string url = "https://openapi.zalo.me/v3.0/oa/message/cs";
                var stringContent = new StringContent(json, Encoding.UTF8);
                stringContent.Headers.Add("Content-Type", "application/json");
                stringContent.Headers.Add("access_token", accessToken);
                HttpResponseMessage response = await client.PostAsync(url, stringContent);

                string htmltext = await response.Content.ReadAsStringAsync();
                foreach (var header in response.Headers)
                {
                    foreach (var value in header.Value)
                    {
                        Console.WriteLine($"{header.Key,25} : {value}");
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage();
            }
        }

        [HttpGet]
        public async Task<ZaloAuthorizationDto> GetForEdit()
        {
            ZaloAuthorization lastObj = _zaloAuthorization.GetAllList().OrderByDescending(x => x.CreationTime).FirstOrDefault();
            if (lastObj != null)
            {
                // tinh thoihan expire_in: tu CreateTime - DateNow
                var dataReturn = ObjectMapper.Map<ZaloAuthorizationDto>(lastObj);
                var totalSecond = (decimal)(DateTime.Now - dataReturn.CreationTime).Value.TotalSeconds;
                dataReturn.IsExpiresAccessToken = totalSecond > 9000;
                if (totalSecond > 9000)
                {
                    var newToken = await GetNewAccessToken_fromRefreshToken(dataReturn.RefreshToken);
                    if (newToken != null)
                    {
                        ZaloAuthorizationDto zaloToken = new ZaloAuthorizationDto
                        {
                            AccessToken = newToken.access_token,
                            RefreshToken = newToken.access_token,
                            ExpiresToken = newToken.expires_in,
                            CodeVerifier = "asdsde"// todo
                        };
                        var data = Insert(zaloToken);
                        return data;
                    }
                    else
                    {
                        return null;
                    }
                }
                return dataReturn;
            }
            return null;
        }

        private async Task WebRequest(string refresh_token)
        {
            const string WEBSERVICE_URL = "https://oauth.zaloapp.com/v4/oa/access_token";
            try
            {
                HttpClient client = new()
                {
                    BaseAddress = new Uri(WEBSERVICE_URL)
                };
                using HttpRequestMessage request = new(HttpMethod.Post, WEBSERVICE_URL);
                request.Headers.Add("refresh_token", refresh_token);
                request.Headers.Add("Session_key", refresh_token);
                request.Headers.Add("grant_type", "refresh_token");
                string json = JsonSerializer.Serialize(WEBSERVICE_URL);
                request.Content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
                using HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                //var webRequest = await client.SendAsync(WEBSERVICE_URL);
                //if (webRequest != null)
                //{
                //    webRequest.Method = "POST";
                //    webRequest.Timeout = 12000;
                //    webRequest.ContentType = "application/x-www-form-urlencoded";
                //    webRequest.Headers.Add("refresh_token", refresh_token);
                //    webRequest.Headers.Add("app_id", refresh_token);
                //    webRequest.Headers.Add("grant_type", "refresh_token");

                //using System.IO.Stream s = webRequest.GetResponse().GetResponseStream();
                //using System.IO.StreamReader sr = new System.IO.StreamReader(s);
                //    var jsonResponse = sr.ReadToEnd();
                //    Console.WriteLine(String.Format("Response: {0}", jsonResponse));
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
