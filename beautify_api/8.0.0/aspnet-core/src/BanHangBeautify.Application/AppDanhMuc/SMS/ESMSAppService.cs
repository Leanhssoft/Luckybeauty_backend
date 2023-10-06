using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.POIFS.Crypt;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using Twilio.TwiML.Voice;

namespace BanHangBeautify.AppDanhMuc.SMS
{
    public class ESMSAppService : SPAAppServiceBase
    {
        static readonly HttpClient client = new();
        const string SMS_API_KEY = "4DAE81CC39F5FFB1AD0B9191E5D8E4";
        const string SMS_API_SECRETKEY = "5BD0408955AB2B465F1D903F596429";
        public ESMSAppService()
        {

        }

        public static async Task<string> SendGetRequest(string RequestUrl)
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

        public class ESMSDto
        {
            // giá trị các tham số này là do ESMS cung cấp
            public string Phone { get; set; }// sdt người nhận
            public string Content { get; set; }// nội dung tin nhắn
            public string Brandname { get; set; }
            //public int SmsType { get; set; }  //2. CSKH, 24.Zalo ưu tiên, 25. Zalo thường
            //public DateTime? SendDate { get; set; } = null;// đặt lịch gửi tin (ngày..): hiện tại chưa dùng
            //public string RequestId { get; set; }// ID Tin nhắn của đối tác, dùng để kiểm tra ID này đã được hệ thống esms tiếp nhận trước đó hay chưa (hiện tại chưa dùng)
            //public int? Sandbox { get; set; }// 1.Gửi thật, 1.Thử nghiệm (không gửi, mà chỉ tạo tin nhắn)
        }

        public class ResultSMSDto
        {
            public string MessageId { get; set; }
            public int MessageStatus { get; set; }// 100. thanh cong
        }
    }
}
