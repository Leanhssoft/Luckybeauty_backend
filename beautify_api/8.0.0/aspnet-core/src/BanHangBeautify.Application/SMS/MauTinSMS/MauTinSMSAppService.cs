using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.MauTinSMS
{
    public class MauTinSMSAppService : SPAAppServiceBase
    {
        public readonly IRepository<SMS_Template, Guid> _smsMauTin;
        public MauTinSMSAppService(IRepository<SMS_Template, Guid> smsMauTin)
        {
            _smsMauTin = smsMauTin;
        }

        [HttpPost]
        public MauTinSMSDto CreateMauTinSMS(MauTinSMSDto dto)
        {
            if (dto == null) { return new MauTinSMSDto(); };
            SMS_Template objNew = ObjectMapper.Map<SMS_Template>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = dto.TenantId;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            _smsMauTin.InsertAsync(objNew);
            var result = ObjectMapper.Map<MauTinSMSDto>(objNew);
            return result;
        }
        [HttpPost]
        public async Task<string> UpdateMauTinSMS(MauTinSMSDto dto)
        {
            try
            {
                if (dto == null) { return "Data null"; };
                SMS_Template objUp = await _smsMauTin.FirstOrDefaultAsync(dto.Id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.TenMauTin = dto.TenMauTin;
                objUp.NoiDungTinMau = dto.NoiDungTinMau;
                objUp.LaMacDinh = dto.LaMacDinh;
                objUp.TrangThai = dto.TrangThai;
                objUp.LastModifierUserId = AbpSession.UserId;
                objUp.LastModificationTime = DateTime.Now;
                await _smsMauTin.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
        [HttpGet]
        public async Task<List<MauTinSMSDto>> GetAllMauTinSMS()
        {
            var objUp = await _smsMauTin.GetAllListAsync();
            List<MauTinSMSDto> result = ObjectMapper.Map<List<MauTinSMSDto>>(objUp);
            return result;
        }
    }
}
