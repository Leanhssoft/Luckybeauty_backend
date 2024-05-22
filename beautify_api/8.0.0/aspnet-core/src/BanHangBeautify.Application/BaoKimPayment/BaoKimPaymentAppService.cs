using Azure.Core;
using BanHangBeautify.BaoKimPayment;
using BanHangBeautify.CryptographyHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoKim
{
    public class BaoKimPaymentAppService : SPAAppServiceBase, IBaoKimPaymentAppService
    {
        public const string PARTNER_CODE = "SSOFT";
        private static string PUBLIC_KEY;
        private static string PRIVATE_KEY;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BaoKimPaymentAppService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            PUBLIC_KEY = File.ReadAllText(Path.Combine(_hostEnvironment.WebRootPath, @"Sercret\RSA_PUBLIC_KEY.txt"));
            PRIVATE_KEY = File.ReadAllText(Path.Combine(_hostEnvironment.WebRootPath, @"Sercret\RSA_PRIVATE_KEY.txt"));
        }

        //public static string CreateSignature(string data)
        //{
        //    return CryptographyHelper.CryptographyHelper.SHA256WithRSAEncrypt(data, PRIVATE_KEY);
        //}  
        public string CreateSignature(string data)
        {
            return CryptographyHelper.CryptographyHelper.Sha1WithRSAEncrypt1(data, PRIVATE_KEY);
        }

        public bool VerifySignature(string content, string signature)
        {
            return CryptographyHelper.CryptographyHelper.Verify(content, signature, PUBLIC_KEY);
        }

        public async Task<BankInfor> CreateQRCode()
        {
            BaoKim_CreateQRDto request = new();
            var dtNow = DateTime.Now.ToString("yyyyMMddhhmmssSSS");
            request.RequestId = string.Concat(PARTNER_CODE, dtNow);
            request.RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            request.PartnerCode = PARTNER_CODE;
            request.Operation = "9001";
            request.CreateType = 2;
            request.AccName = "SSOFT";
            request.OrderId = dtNow;

            string url = "https://devtest.baokim.vn/Sandbox/Collection/V2";
            var body = JsonConvert.SerializeObject(request);
            StringContent dataPost = new(body, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            try
            {
                var sign = CreateSignature(body);
                client.DefaultRequestHeaders.Add("Signature", sign);

                var response = await client.PostAsync(url, dataPost);
                string result = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    var res = JsonConvert.DeserializeObject<BaoKim_ResponseQRDto>(result);
                    if (res.ResponseCode == 200 && res.AccountInfo?.BANK != null)
                    {
                        return res.AccountInfo?.BANK;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public async Task<BankInfor> UpdateQRCode(string accNo)
        {
            BaoKim_CreateQRDto request = new();
            var dtNow = DateTime.Now.ToString("yyyyMMddhhmmss");
            request.RequestId = string.Concat(PARTNER_CODE, dtNow);
            request.RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            request.PartnerCode = PARTNER_CODE;
            request.Operation = "9002";
            request.AccNo = accNo;

            string url = "https://devtest.baokim.vn/Sandbox/Collection/V2";
            var body = JsonConvert.SerializeObject(request);
            StringContent dataPost = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var sign = CreateSignature(body);
                    client.DefaultRequestHeaders.Add("Signature", sign);

                    var response = await client.PostAsync(url, dataPost);
                    string result = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(result))
                    {
                        var res = JsonConvert.DeserializeObject<BaoKim_ResponseQRDto>(result);
                        if (res.ResponseCode == 200 && res.AccountInfo?.BANK != null)
                        {
                            return res.AccountInfo?.BANK;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                }
                return null;
            }

        }
        public async Task<BankInfor> TraCuuThongTinQR()
        {
            BaoKim_CreateQRDto request = new();
            var dtNow = DateTime.Now.ToString("yyyyMMddhhmmss");
            request.RequestId = string.Concat(PARTNER_CODE, dtNow);
            request.RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            request.PartnerCode = PARTNER_CODE;
            request.Operation = "9003";
            request.AccNo = "963336007079254";

            string url = "https://devtest.baokim.vn/Sandbox/Collection/V2";
            var body = JsonConvert.SerializeObject(request);
            StringContent dataPost = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var sign = CreateSignature(body);
                    client.DefaultRequestHeaders.Add("Signature", sign);

                    var response = await client.PostAsync(url, dataPost);
                    string result = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(result))
                    {
                        var res = JsonConvert.DeserializeObject<BaoKim_ResponseQRDto>(result);
                        if (res.ResponseCode == 200 && res.AccountInfo?.BANK != null)
                        {
                            return res.AccountInfo?.BANK;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                }
                return null;
            }

        }
        public async Task<BankInfor> TraCuuThongGiaoDich(Guid idHoaDon)
        {
            BaoKim_CreateQRDto request = new();
            var dtNow = DateTime.Now.ToString("yyyyMMddhhmmss");
            request.RequestId = string.Concat(PARTNER_CODE, dtNow);
            request.RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            request.PartnerCode = PARTNER_CODE;
            request.Operation = "9004";
            request.ReferenceId = string.Concat(PARTNER_CODE, idHoaDon);

            string url = "https://devtest.baokim.vn/Sandbox/Collection/V2";
            var body = JsonConvert.SerializeObject(request);
            StringContent dataPost = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var sign = CreateSignature(body);
                    client.DefaultRequestHeaders.Add("Signature", sign);

                    var response = await client.PostAsync(url, dataPost);
                    string result = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(result))
                    {
                        var res = JsonConvert.DeserializeObject<BaoKim_ResponseQRDto>(result);
                        if (res.ResponseCode == 200 && res.AccountInfo?.BANK != null)
                        {
                            return res.AccountInfo?.BANK;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                }
                return null;
            }
        }
        [HttpGet]
        public async Task<string> TaoGiaoDich (string accNo, string amount)
        {
            HttpClient client = new();
            string url = $@"https://devtest.baokim.vn/collection/core/Sandbox/Collection/createTrans?accNo={accNo}&partnerCode={PARTNER_CODE}&transAmount={amount}";
            HttpResponseMessage response = await client.GetAsync(url);
            string htmltext = await response.Content.ReadAsStringAsync();
            var dataMes = JsonConvert.DeserializeObject<MessageResponse>(htmltext);
            return dataMes.message;
        }
        [HttpGet]
        public async Task<string> ThongBaoGiaoDich()
        {
            HttpClient client = new();
            string url = $@"https://devtest.baokim.vn/collection/woori_send/Sandbox/Collection/partner";
            HttpResponseMessage response = await client.GetAsync(url);
            string htmltext = await response.Content.ReadAsStringAsync();
            //var dataMes = JsonConvert.DeserializeObject<MessageResponse>(htmltext);
            return htmltext;
        }
        /// <summary>
        /// nếu thanh toan baokim ok: return idHoadon để lưu hóa đơn
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idHoaDon"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<bool> GuiLaiThongTinGiaoDich_BaoKim(ResponseThongBaoGiaoDich data, Guid idHoaDon)
        {
            string dataSign = $@"200|Yêu cầu thành công|{idHoaDon}|{data.AccNo}|{data.AffTransDebt}";
            var sign = CreateSignature(dataSign);

            RequestThongBaoGiaoDich request = new()
            {
                ResponseCode = 200,
                ResponseMessage = "Yêu cầu thành công",
                ReferenceId = idHoaDon.ToString(),
                AccNo = data.AccNo,
                Signature = sign,
            };

            //string url = "https://devtest.baokim.vn/Sandbox/Collection/V2";
            string url = "https://devtest.baokim.vn/collection/woori_send/Sandbox/Collection/partner";
            var body = JsonConvert.SerializeObject(request);
            StringContent dataPost = new(body, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            try
            {
                var response = await client.PostAsync(url, dataPost);
                string result = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
