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

namespace BanHangBeautify.SMS.ESMS
{
    public class ESMSAppService : SPAAppServiceBase, IESMS
    {
        static readonly HttpClient client = new();
        const string SMS_API_KEY = "4DAE81CC39F5FFB1AD0B9191E5D8E4";
        const string SMS_API_SECRETKEY = "5BD0408955AB2B465F1D903F596429";
        public readonly ISettings _appSetting;
        private readonly IEmailSender _emailSender;
        public readonly IHeThongSMSRepository _repoSMS;

        public ESMSAppService(ISettings appSetting, IHeThongSMSRepository repoSMS)
        {
            _appSetting = appSetting;
            _repoSMS = repoSMS;
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

        [HttpPost]
        public async Task<bool> SenEmail(List<CustomerBasicDto> lstCustomer, string noiDungTin)
        {
            try
            {
                // get email setting
                EmailSettingsEditDto emailSetting = await _appSetting.GetEmailSettingsAsync();
                foreach (var item in lstCustomer)
                {
                    var email = new MimeMessage();

                    email.From.Add(new MailboxAddress(emailSetting.DefaultFromDisplayName, emailSetting.SmtpUserName));
                    email.To.Add(new MailboxAddress(item.TenKhachHang, item.Email));

                    email.Subject = "nhuongdt test email sending";
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                    {
                        Text = noiDungTin + " happy birth day " + item.TenKhachHang

                    };
                    using var smtp = new SmtpClient();
                    smtp.Connect(emailSetting.SmtpHost, emailSetting.SmtpPort, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate(emailSetting.SmtpUserName, emailSetting.SmtpPassword);

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        [HttpGet]
        public async Task<bool> SenEmail_ToListCustomer(ParamSearch input, byte? idLoaiTin, EmailDto objEmail)
        {
            EmailSettingsEditDto emailSetting = await _appSetting.GetEmailSettingsAsync();
            var data = await _repoSMS.GetListCustomer_byIdLoaiTin(input, idLoaiTin);
            var lstCustomer = ObjectMapper.Map<List<PageKhachHangSMSDto>>(data.Items);
            foreach (var item in lstCustomer)
            {
                var noidung = ReplaceContent(item, objEmail.NoiDungEmail);
                await _emailSender.SendAsync(emailSetting.SmtpUserName, objEmail.TieuDeEmail, noidung, true);
            }
            return true;
        }
        [HttpGet]
        public async Task<bool> SendSMS_ToListCustomer(ParamSearch input, byte? idLoaiTin, EmailDto objEmail)
        {
            var data = await _repoSMS.GetListCustomer_byIdLoaiTin(input, idLoaiTin);
            var lstCustomer = ObjectMapper.Map<List<PageKhachHangSMSDto>>(data.Items);
            foreach (var item in lstCustomer)
            {
                var noidung = ReplaceContent(item, objEmail.NoiDungEmail);
                ESMSDto obj = new()
                {
                    Phone = item.SoDienThoai,
                    Brandname = "Open24.vn",// todo brandname
                    Content = noidung,
                };
                await SendSMS_Json(obj);
            }
            return true;
        }

        [HttpGet]
        protected string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin)
        {
            var ss = noiDungTin.Replace("{TenKhachHang}", cutomer.TenKhachHang);
            _ = noiDungTin.Replace("{NgaySinh}", cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            _ = noiDungTin.Replace("{BookingDate}", cutomer.BookingDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            _ = noiDungTin.Replace("{ThoiGianHen}", cutomer.ThoiGianHen);
            _ = noiDungTin.Replace("{DichVuHen}", cutomer.TenHangHoa);
            _ = noiDungTin.Replace("{MaGiaoDich}", cutomer.MaHoaDon);
            _ = noiDungTin.Replace("{NgayGiaoDich}", cutomer.NgayLapHoaDon?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
            return ss;
        }
    }
}
