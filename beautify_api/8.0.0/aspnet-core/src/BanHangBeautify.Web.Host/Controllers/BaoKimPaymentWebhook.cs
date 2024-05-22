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
using BanHangBeautify.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BanHangBeautify.Web.Host.Controllers
{
    [Route("api/baokim-payment/webhook")]
    public class BaoKimPaymentWebhook : BanHangBeautify.Controllers.SPAControllerBase
    {
        private readonly IBaoKimPaymentAppService _baokimPaymentAppService;
        private readonly IInvoiceHub _invoiceHubContext;
        public BaoKimPaymentWebhook(IBaoKimPaymentAppService baokimPaymentAppService, IInvoiceHub invoiceHubContext)
        {
            _baokimPaymentAppService = baokimPaymentAppService;
            _invoiceHubContext = invoiceHubContext;
        }
        [HttpPost]
        [Route("transaction-notification")]
        public async Task<ResponseThongBaoGiaoDich> ThongBaoGiaoDich()
        {
            using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<ResponseThongBaoGiaoDich>(body);
                var dataSign = $@"{data.RequestId}|{data.RequestTime}|{data.PartnerCode}|{data.AccNo}|{data.ClientIdNo}{data.TransId}|{data.TransAmount}|{data.TransTime}|{data.BefTransDebt}|{data.AffTransDebt}|{data.AccountType}|{data.OrderId}";
                //var sign = _baokimPaymentAppService.CreateSignature(dataSign);

                if (!_baokimPaymentAppService.VerifySignature(dataSign, data.Signature))
                {
                    return null;
                }
                await _invoiceHubContext.ThongBaoGiaDich_fromBaoKim(data);
                return data;
            }
        }
        [HttpPost]
        [Route("cassio-notification")]
        public async Task<CassoResponseThongBaoGiaoDich> Cassio_ThongBaoGiaoDich()
        {
            using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
            {
                var secure_token = string.Empty; ;
                if (!HttpContext.Request.Headers.ContainsKey("secure-token"))
                {
                    return null;
                }
                secure_token = HttpContext.Request.Headers["secure-token"];
                if(secure_token != "d93b17ea89b94ecfb242d03b8cde71de")
                {
                    return null;
                }

                var body = await reader.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CassoResponseThongBaoGiaoDich>(body);
                return data;
            }
        }
    }
}
