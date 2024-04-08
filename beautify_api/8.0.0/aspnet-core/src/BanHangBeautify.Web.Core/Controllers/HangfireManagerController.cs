using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Dependency;
using BanHangBeautify.NhacNho.NhacNhoHoatDong;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Controllers
{
    [Route("api/[controller]/[action]")]
    [AbpAuthorize]
    public class HangfireManagerController: SPAControllerBase
    {
        private readonly INhacNhoHoatDongService _nhacNhoHoatDongService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public HangfireManagerController(
            INhacNhoHoatDongService nhacNhoHoatDongService,
            IBackgroundJobClient backgroundJobClient, 
            IRecurringJobManager recurringJobManager)
        {
            _nhacNhoHoatDongService = nhacNhoHoatDongService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }
        [HttpPost]
        public async Task RemindActivity()
        {
            _recurringJobManager.AddOrUpdate<INhacNhoHoatDongService>("SendDailyReminderEmail",
                x=> x.SendEmailRemindActivity(), // Chỉ định phương thức cụ thể
                Cron.Daily);
        }
        
    }
}
