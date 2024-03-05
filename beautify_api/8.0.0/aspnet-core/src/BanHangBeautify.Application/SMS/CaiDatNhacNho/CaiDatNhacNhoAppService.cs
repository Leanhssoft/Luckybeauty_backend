using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BanHangBeautify.SMS.CaiDatNhacNho
{
    public class CaiDatNhacNhoAppService : SPAAppServiceBase
    {
        public readonly IRepository<SMS_CaiDat_NhacNho, Guid> _caiDatNhacNho;
        public readonly IRepository<CaiDat_NhacNho_ChiTiet, Guid> _caiDatNhacNhoChiTiet;
        public CaiDatNhacNhoAppService(IRepository<SMS_CaiDat_NhacNho, Guid> caidatNhacNho, IRepository<CaiDat_NhacNho_ChiTiet, Guid> caiDatNhacNhoChiTiet)
        {
            _caiDatNhacNho = caidatNhacNho;
            _caiDatNhacNhoChiTiet = caiDatNhacNhoChiTiet;
        }

        [HttpPost]
        public CaiDatNhacNhoDto CreateCaiDatNhacNho(CaiDatNhacNhoDto dto)
        {
            if (dto == null) { return new CaiDatNhacNhoDto(); };
            SMS_CaiDat_NhacNho objNew = ObjectMapper.Map<SMS_CaiDat_NhacNho>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            _caiDatNhacNho.InsertAsync(objNew);
            var result = ObjectMapper.Map<CaiDatNhacNhoDto>(objNew);
            return result;
        }
        [HttpPost]
        public async Task<string> UpdateCaiDatNhacNho(CaiDatNhacNhoDto dto)
        {
            try
            {
                if (dto == null) { return "Data null"; };
                SMS_CaiDat_NhacNho objUp = await _caiDatNhacNho.FirstOrDefaultAsync(dto.Id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.IdLoaiTin = dto.IdLoaiTin;
                objUp.IdMauTin = dto.IdMauTin;
                objUp.NoiDungTin = dto.NoiDungTin;
                objUp.NhacTruocKhoangThoiGian = dto.NhacTruocKhoangThoiGian;
                objUp.LoaiThoiGian = dto.LoaiThoiGian;
                objUp.TrangThai = dto.TrangThai;
                objUp.LastModifierUserId = AbpSession.UserId;
                objUp.LastModificationTime = DateTime.Now;
                await _caiDatNhacNho.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }

        [HttpGet]
        public async Task<CaiDatNhacNhoDto> CaiDatNhacNho_UpdateTrangThai(Guid idCaiDatNhacNho, byte? trangThai = 0)
        {
            try
            {
                SMS_CaiDat_NhacNho objUp = await _caiDatNhacNho.FirstOrDefaultAsync(idCaiDatNhacNho);
                if (objUp == null)
                {
                    return null;
                }
                objUp.TrangThai = trangThai;
                objUp.LastModifierUserId = AbpSession.UserId;
                objUp.LastModificationTime = DateTime.Now;
                await _caiDatNhacNho.UpdateAsync(objUp);
                return ObjectMapper.Map<CaiDatNhacNhoDto>(objUp);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CaiDatNhacNhoChiTietDto> CreateCaiDatNhacNhoChiTiet(Guid idCaiDatNhacNho, CaiDatNhacNhoChiTietDto item)
        {
            CaiDat_NhacNho_ChiTiet ctNew = ObjectMapper.Map<CaiDat_NhacNho_ChiTiet>(item);
            ctNew.Id = Guid.NewGuid();
            ctNew.TenantId = AbpSession.TenantId ?? 1;
            ctNew.IdCaiDatNhacNho = idCaiDatNhacNho;
            ctNew.CreatorUserId = AbpSession.UserId;
            ctNew.CreationTime = DateTime.Now;
            await _caiDatNhacNhoChiTiet.InsertAsync(ctNew);
            return ObjectMapper.Map<CaiDatNhacNhoChiTietDto>(ctNew);
        }

        [HttpPost]
        public async Task<CaiDatNhacNhoChiTietDto> CreateOrUpdateCaiDatNhacNhoChiTiet(Guid idCaiDatNhacNho, CaiDatNhacNhoChiTietDto item)
        {
            var lst = _caiDatNhacNhoChiTiet.GetAllList(x => x.IdCaiDatNhacNho == idCaiDatNhacNho);
            if (lst != null && lst.Count > 0)
            {
                CaiDat_NhacNho_ChiTiet ctUpdate = await _caiDatNhacNhoChiTiet.FirstOrDefaultAsync(item.Id);
                if (ctUpdate != null)
                {
                    ctUpdate.HinhThucGui = item.HinhThucGui;
                    ctUpdate.TrangThai = item.TrangThai;
                    ctUpdate.LastModifierUserId = AbpSession.UserId;
                    ctUpdate.LastModificationTime = DateTime.Now;
                    await _caiDatNhacNhoChiTiet.UpdateAsync(ctUpdate);
                    return item;
                }
                else
                {
                    return await CreateCaiDatNhacNhoChiTiet(idCaiDatNhacNho, item);
                }
            }
            else
            {
                return await CreateCaiDatNhacNhoChiTiet(idCaiDatNhacNho, item);
            }
        }
        [HttpGet]
        public async Task<List<CaiDatNhacNhoDto>> GetAllCaiDat()
        {
            var data = await _caiDatNhacNho.GetAllListAsync();
            List<CaiDatNhacNhoDto> result = ObjectMapper.Map<List<CaiDatNhacNhoDto>>(data);
            foreach (var item in result)
            {
                var lstCT = await _caiDatNhacNhoChiTiet.GetAllListAsync(x => x.IdCaiDatNhacNho == item.Id);
                item.CaiDatNhacNhoChiTiets = ObjectMapper.Map<List<CaiDatNhacNhoChiTietDto>>(lstCT);
            }
            return result;
        }
        [HttpGet]
        public async Task<CaiDatNhacNhoDto> GetInforCaiDatNhacNho_byId(Guid idCaiDatNhacNho)
        {
            var objUp = await _caiDatNhacNho.FirstOrDefaultAsync(idCaiDatNhacNho);
            CaiDatNhacNhoDto result = ObjectMapper.Map<CaiDatNhacNhoDto>(objUp);
            return result;
        }
    }
}
