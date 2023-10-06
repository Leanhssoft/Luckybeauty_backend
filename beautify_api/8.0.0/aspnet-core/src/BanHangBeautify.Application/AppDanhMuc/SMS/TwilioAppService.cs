using Abp.Configuration;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NPOI.POIFS.Crypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Http;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Conversations.V1;
using Twilio.Rest.Verify.V2.Service;
using Twilio.TwiML.Messaging;
using Twilio.Types;

namespace BanHangBeautify.AppDanhMuc.SMS
{
    public class TwilioAppService : SPAAppServiceBase
    {
        //private readonly IConfigurationRoot _appConfiguration;
        public TwilioAppService()
        {
        }
        [HttpPost]
        public async Task<List<TestTwilio>> SendSMS(string mess, List<string> toArrPhone)
        {
            List<TestTwilio> result = new();
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            TwilioClient.Init(accountSid, authToken);

            // get phone sender from tblBrandName
            foreach (var item in toArrPhone)
            {
                var message = await MessageResource.CreateAsync(
                body: mess,
                from: new PhoneNumber("+14698888263"),
                to: new PhoneNumber(item),
                statusCallback: new Uri("https://localhost:44311/api/services/app/HangHoa/GetDetailProduct?idDonViQuyDoi=B3536236-1FA4-43A7-81EE-462033D93BE8")
                );
                result.Add(new TestTwilio { MessageId = message.Sid, Status = message.Status, Price = message.Price });
            }
            return result;
        }


        public class TestTwilio
        {
            public string MessageId { get; set; }
            public MessageResource.StatusEnum Status { get; set; }
            public string Price { get; set; }
        }
    }
}
