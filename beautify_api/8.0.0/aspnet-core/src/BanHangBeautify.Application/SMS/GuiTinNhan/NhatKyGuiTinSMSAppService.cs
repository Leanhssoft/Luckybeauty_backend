using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.GuiTinNhan
{
    public class NhatKyGuiTinSMSAppService : SPAAppServiceBase
    {
        private readonly IRepository<SMS_NhatKy_GuiTin, Guid> _nkyGuiTinSMS;
        private readonly IHeThongSMSRepository _repoSMS;

        public NhatKyGuiTinSMSAppService(IRepository<SMS_NhatKy_GuiTin, Guid> nkyGuiTinSMS, IHeThongSMSRepository repoSMS)
        {
            _nkyGuiTinSMS = nkyGuiTinSMS;
            _repoSMS = repoSMS;
        }

        [HttpPost]
        public async Task<NhatKyGuiTinSMSDto> ThemMoi_NhatKyGuiTinSMS(NhatKyGuiTinSMSDto input)
        {
            SMS_NhatKy_GuiTin data = ObjectMapper.Map<SMS_NhatKy_GuiTin>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _nkyGuiTinSMS.InsertAsync(data);
            NhatKyGuiTinSMSDto result = ObjectMapper.Map<NhatKyGuiTinSMSDto>(data);
            return result;
        }
        [HttpPost]
        public async Task<bool> ThemMoi_NhatKyGuiTin_TrongKhoangThoiGian(NhatKyGuiTinSMSDto input)
        {
            var data = await _repoSMS.InsertNhatKyGuiTinSMS(input, AbpSession.TenantId ?? 1);
            return data > 0; ;
        }
    }
}
