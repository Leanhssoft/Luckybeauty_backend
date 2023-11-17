using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using BanHangBeautify.SMS.Dto;
using System.Net.Http;
using System.Net;
using System;
using MimeKit;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using MailKit.Net.Smtp;
using BanHangBeautify.Configuration.Dto;
using Asd.AbpZeroTemplate.Configuration;
using BanHangBeautify.Configuration.Settings;
using System.Collections.Generic;
using Abp.Net.Mail;
using BanHangBeautify.Configuration.Common.Consts;
using System.Globalization;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using static BanHangBeautify.Configuration.Common.Consts.ConstSMS;
using static BanHangBeautify.Configuration.Common.CommonClass;
using Twilio.TwiML.Voice;
using System.Runtime.CompilerServices;
using BanHangBeautify.SMS.GuiTinNhan;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BanHangBeautify.SMS.ESMS
{
    public class ESMSAppService : SPAAppServiceBase, IESMS
    {
        static readonly HttpClient client = new();
        const string SMS_API_KEY = "4DAE81CC39F5FFB1AD0B9191E5D8E4";
        const string SMS_API_SECRETKEY = "5BD0408955AB2B465F1D903F596429";
        const string Zalo_API_KEY = "4DAE81CC39F5FFB1AD0B9191E5D8E4";
        const string Zalo_API_SECRETKEY = "5BD0408955AB2B465F1D903F596429";
        //public readonly ISettings _appSetting;
        //private readonly IEmailSender _emailSender;
        //public readonly IHeThongSMSRepository _repoSMS;
        //public readonly IHeThongSMSAppService _hethongAppService;

        public ESMSAppService()
        {
        }

        protected static async Task<string> SendGetRequest(string RequestUrl)
        {
            Uri address = new(RequestUrl);
            if (address != null)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(content);
                        return content;
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                    }
                }
                catch (WebException wex)
                {
                    if (wex.Response != null)
                    {
                        using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                        {
                            Console.WriteLine(
                                "The server returned '{0}' with the status code {1} ({2:d}).",
                                errorResponse.StatusDescription, errorResponse.StatusCode,
                                errorResponse.StatusCode);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> SendZalo(ZaloDto obj)
        {
            string url = @"http://rest.esms.vn/MainService.svc/json/SendZaloMessage_V4_post_json" + obj;
            return true;
        }

        public async Task<bool> GetListZaloOA()
        {
            string url = @"http://rest.esms.vn/MainService.svc/json/ZNS/GetListZOA?ApiKey=" + SMS_API_KEY + "&SecretKey=" + SMS_API_SECRETKEY;
            JObject ojb = JObject.Parse(url);
            return true;
        }
        [HttpPost]
        public async Task<ResultSMSDto> SendSMS_Json(ESMSDto obj)
        {
            string url = "http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone=" + obj.Phone
              + "&Content=" + obj.Content + "&Brandname=" + obj.Brandname + "&ApiKey="
              + SMS_API_KEY + "&SecretKey=" + SMS_API_SECRETKEY + "&IsUnicode=0&SmsType=2";
            string result = await SendGetRequest(url);
            JObject ojb = JObject.Parse(result);
            return new ResultSMSDto { MessageId = (string)ojb["SMSID"], MessageStatus = (int)ojb["CodeResult"] };
        }
    }
}
