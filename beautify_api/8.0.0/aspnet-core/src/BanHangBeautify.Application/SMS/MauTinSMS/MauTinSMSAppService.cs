﻿using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.LichSuNap_ChuyenTien.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.MauTinSMS
{
    public class MauTinSMSAppService : SPAAppServiceBase
    {
        public readonly IRepository<SMS_Template, Guid> _smsMauTin;
        public readonly IMauTinSMSRepository _repoMauTinSMS;
        public MauTinSMSAppService(IRepository<SMS_Template, Guid> smsMauTin, IMauTinSMSRepository repoMauTinSMS)
        {
            _smsMauTin = smsMauTin;
            _repoMauTinSMS = repoMauTinSMS;
        }

        [HttpPost]
        public MauTinSMSDto CreateMauTinSMS(MauTinSMSDto dto)
        {
            if (dto == null) { return new MauTinSMSDto(); };
            SMS_Template objNew = ObjectMapper.Map<SMS_Template>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
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
                // find all mautin (same idLoaiTin) && reset IsDefault
                bool isDefaut = objUp.LaMacDinh ?? false;
                if (isDefaut)
                {
                    _smsMauTin.GetAllList(x => x.IdLoaiTin == dto.IdLoaiTin && (x.LaMacDinh ?? false)).ForEach(x => x.LaMacDinh = false);
                }
                await _smsMauTin.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
        [HttpGet]
        public async Task<MauTinSMSDto> GetMauTinSMS_byId(Guid id)
        {
            var objUp = await _repoMauTinSMS.GetMauTinSMS_byId(id);
            return objUp;
        }
        [HttpGet]
        public async Task<List<MauTinSMSDto>> GetAllMauTinSMS()
        {
            var objUp = await _smsMauTin.GetAllListAsync();
            List<MauTinSMSDto> result = ObjectMapper.Map<List<MauTinSMSDto>>(objUp);
            return result;
        }

        Func<byte?, string> GetLoaiTin2 = value =>
        {
            switch (value)
            {
                case 2: return "Lời chúc mừng sinh nhật";
                case 3: return "Nhắc nhở cuộc hẹn";
                case 4: return "Tin giao dịch";
                default: return "Loại tin khác";
            }
        };

        public string GetLoaiTin(byte? idLoaiTin)
        {

            return idLoaiTin switch
            {
                2 => "Tin sinh nhật",
                3 => "Tin lịch hẹn",
                4 => "Tin giao dịch",
                _ => "Loại tin khác",
            };
        }
        [HttpGet]
        public async Task<List<GroupMauTinSMSDto>> GetAllMauTinSMS_GroupLoaiTin()
        {
            var objUp = await _smsMauTin.GetAllListAsync();
            List<GroupMauTinSMSDto> data = objUp.GroupBy(x => new { x.IdLoaiTin }).Select(x => new GroupMauTinSMSDto
            {
                IdLoaiTin = x.Key.IdLoaiTin,
                LoaiTin = GetLoaiTin(x.Key.IdLoaiTin),
                LstDetail = ObjectMapper.Map<List<MauTinSMSDto>>(x)
            }).OrderBy(x=>x.IdLoaiTin).ToList();
            return data;
        }
    }
}
