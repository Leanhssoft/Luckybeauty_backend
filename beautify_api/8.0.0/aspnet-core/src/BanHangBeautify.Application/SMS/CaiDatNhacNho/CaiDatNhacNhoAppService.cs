using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.CaiDatNhacNho
{
    public class CaiDatNhacNhoAppService : SPAAppServiceBase
    {
        public readonly IRepository<SMS_CaiDat_NhacNho, Guid> _caiDatNhacNho;
        public readonly ICaiDatNhacNhoRepository _repoCaiDatNhacNho;
        public CaiDatNhacNhoAppService(IRepository<SMS_CaiDat_NhacNho, Guid> caidatNhacNho, ICaiDatNhacNhoRepository repoCaiDatNhacNho)
        {
            _caiDatNhacNho = caidatNhacNho;
            _repoCaiDatNhacNho = repoCaiDatNhacNho;
        }

        [HttpPost]
        public async Task<CaiDatNhacNhoDto> CreateCaiDatNhacNho(CaiDatNhacNhoDto dto)
        {
            if (dto == null) { return new CaiDatNhacNhoDto(); };

            // vì data cũ chưa có cột: hinhThucGui --> xóa
            await RemoveCaiDatNhacNho(dto.IdLoaiTin, dto.HinhThucGui);

            SMS_CaiDat_NhacNho objNew = ObjectMapper.Map<SMS_CaiDat_NhacNho>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            await _caiDatNhacNho.InsertAsync(objNew);
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
                objUp.NhacTruocKhoangThoiGian = dto.NhacTruocKhoangThoiGian;
                objUp.LoaiThoiGian = dto.LoaiThoiGian;
                objUp.TrangThai = dto.TrangThai;
                objUp.HinhThucGui = dto.HinhThucGui;
                objUp.IdMauTin = dto.IdMauTin;
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
        public async Task RemoveCaiDatNhacNho(byte idLoaiTin, byte? hinhThucGui)
        {
            var data = _caiDatNhacNho.GetAllList().Where(x => x.IdLoaiTin == idLoaiTin && x.HinhThucGui == hinhThucGui);
            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    await _caiDatNhacNho.HardDeleteAsync(item);
                }
            }
        }

        [HttpGet]
        public async Task<List<CaiDatNhacNhoDto>> GetAllCaiDat()
        {
            var data = await _repoCaiDatNhacNho.GetAllCaiDatNhacNho();
            return data;
        }
        [HttpGet]
        public async Task<List<CaiDatNhacNho_GroupLoaiTinDto>> GetAllCaiDatNhacNho_GroupLoaiTin()
        {
            List<CaiDatNhacNho_GroupLoaiTinDto> data = await _repoCaiDatNhacNho.GetAllCaiDatNhacNho_GroupLoaiTin();
            return data;
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
