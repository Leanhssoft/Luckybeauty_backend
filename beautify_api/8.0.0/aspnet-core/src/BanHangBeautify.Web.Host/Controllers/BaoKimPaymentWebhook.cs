using Abp.Webhooks;
using BanHangBeautify.AppWebhook.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using BanHangBeautify.BaoKimPayment;
using BanHangBeautify.BaoKim;
using System.Collections.Generic;
using System.Data;
using MailKit.Search;

namespace BanHangBeautify.Web.Host.Controllers
{
    [Route("api/baokim-payment/webhook")]
    public class BaoKimPaymentWebhook : BanHangBeautify.Controllers.SPAControllerBase
    {
        private readonly IBaoKimPaymentAppService _baokimPaymentAppService;
        public BaoKimPaymentWebhook(IBaoKimPaymentAppService baokimPaymentAppService)
        {
            _baokimPaymentAppService = baokimPaymentAppService;
        }
        [HttpPost]
        [Route("transaction-notification")]
        public async Task<bool> ThongBaoGiaoDich()
        {
            using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<ResponseThongBaoGiaoDich>(body);
                var dataSign = $@"{data.RequestId}|{data.RequestTime}|{data.PartnerCode}|{data.AccNo}|{data.ClientIdNo}{data.TransId}|{data.TransAmount}|{data.TransTime}|{data.BefTransDebt}|{data.AffTransDebt}|{data.AccountType}|{data.OrderId}";
                //var sign = _baokimPaymentAppService.CreateSignature(dataSign);

                if (!_baokimPaymentAppService.VerifySignature(dataSign, data.Signature))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
