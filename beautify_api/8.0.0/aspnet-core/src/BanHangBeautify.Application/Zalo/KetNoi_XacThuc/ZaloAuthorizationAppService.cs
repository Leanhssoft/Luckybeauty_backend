using Abp.Domain.Repositories;
using Azure.Core;
using BanHangBeautify.Configuration;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Zalo.DangKyThanhVien;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.KetNoi_XacThuc
{
    public class ZaloAuthorizationAppService : SPAAppServiceBase, IZaloAuthorization
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
                //objUp.OAId = dto.CodeVerifier;// todo
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

        [HttpGet]
        /// <summary>
        /// get infor user OA
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Zalo_KhachHangThanhVienDto> GetInforUser_ofOA(string accessToken, string userId)
        {
            HttpClient client = new();
            var requestData = new
            {
                user_id = userId,
            };
            string jsonData = JsonSerializer.Serialize(requestData);
            string url = $"https://openapi.zalo.me/v3.0/oa/user/detail?data={jsonData}";
            client.DefaultRequestHeaders.Add("access_token", accessToken);
            HttpResponseMessage response = await client.GetAsync(url);
            string htmltext = await response.Content.ReadAsStringAsync();
            // todo: data return null
            var dataReturn = JsonSerializer.Deserialize<ResultDataZaloCommon<Zalo_KhachHangThanhVienDto>>(htmltext);
            return dataReturn.data;
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

        /// <summary>
        /// !!! luôn tạo codeVerifier & codeChallenge tạo trước khi kết nối zalo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ZaloAuthorizationDto> Innit_orGetToken()
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
                    // hết hạn access token: tạo access token từ refresh_token
                    var newToken = await GetNewAccessToken_fromRefreshToken(dataReturn.RefreshToken);
                    if (newToken != null && newToken.access_token != null)
                    {
                        var codeVerifier = GenerateCodeVerifier();
                        var codeChallenge = GenerateCodeChallenge(codeVerifier);
                        ZaloAuthorizationDto zaloToken = new()
                        {
                            AccessToken = newToken.access_token,
                            RefreshToken = newToken.refresh_token,
                            ExpiresToken = newToken.expires_in,
                            CodeVerifier = codeVerifier,
                            CodeChallenge = codeChallenge,
                        };
                        var data = Insert(zaloToken);
                        return data;
                    }
                    else
                    {
                        // token refreh không đúng --> tạo mới lại
                        return await CreateCodeVerifier_andCodeChallenge(); ;
                    }
                }
                return dataReturn;// chưa hết hạn access token
            }
            else
            {
                // nếu chưa có data: {CodeVerifier, odeChalleng}: tạo trước
                return await CreateCodeVerifier_andCodeChallenge();
            }
        }

        static string GenerateCodeVerifier()
        {
            // Define the characters that can be used in the code verifier
            const string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

            // Set the length of the code verifier (should be between 43 and 128 characters)
            const int codeVerifierLength = 43;

            // Generate a random code verifier using a cryptographic random number generator
            byte[] randomBytes = new byte[codeVerifierLength];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the random bytes to characters based on the allowed characters
            char[] codeVerifier = new char[codeVerifierLength];
            for (int i = 0; i < codeVerifierLength; i++)
            {
                int charIndex = randomBytes[i] % allowedCharacters.Length;
                codeVerifier[i] = allowedCharacters[charIndex];
            }

            // Return the generated code verifier as a string
            return new string(codeVerifier);
        }

        static string GenerateCodeChallenge(string codeVerifier)
        {
            // Convert the code verifier to bytes
            byte[] codeVerifierBytes = Encoding.ASCII.GetBytes(codeVerifier);

            // Compute the SHA-256 hash of the code verifier
            byte[] hashBytes = SHA256.HashData(codeVerifierBytes);

            // Encode the hash with Base64
            string codeChallenge = Convert.ToBase64String(hashBytes);

            // URL encode the Base64 string (replace '+' with '-', '/' with '_')
            codeChallenge = codeChallenge.Replace("+", "-").Replace("/", "_");

            return codeChallenge;
        }
        [HttpGet]
        public async Task<ZaloAuthorizationDto> CreateCodeVerifier_andCodeChallenge()
        {
            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);
            ZaloAuthorizationDto zaloToken = new()
            {
                CodeVerifier = codeVerifier,
                CodeChallenge = codeChallenge,
            };
            return Insert(zaloToken);
        }
    }
}
