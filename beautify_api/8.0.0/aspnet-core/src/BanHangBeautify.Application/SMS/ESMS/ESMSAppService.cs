using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using BanHangBeautify.SMS.Dto;
using System.Net.Http;
using System.Net;
using System;

namespace BanHangBeautify.SMS.ESMS
{
    public class ESMSAppService : SPAAppServiceBase, IESMS
    {
        static readonly HttpClient client = new();
        const string SMS_API_KEY = "4DAE81CC39F5FFB1AD0B9191E5D8E4";
        const string SMS_API_SECRETKEY = "5BD0408955AB2B465F1D903F596429";

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
