using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.NhomHangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.NhomHangHoa
{
    public class NhomHangHoaAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_NhomHangHoa, Guid> _dmNhomHangHoa;

        public NhomHangHoaAppService(IRepository<DM_NhomHangHoa, Guid> dmNhomHangHoa)
        {
            _dmNhomHangHoa = dmNhomHangHoa;
        }
        public async Task<NhomHangHoaDto> GetNhomHangHoa_byID(Guid id)
        {
            var data = await _dmNhomHangHoa.GetAsync(id);
            var result = ObjectMapper.Map<NhomHangHoaDto>(data);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<NhomHangHoaDto>> GetNhomDichVu()
        {
            PagedResultDto<NhomHangHoaDto> result = new();
            var lst =  _dmNhomHangHoa.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.TenNhomHang);

            result.Items = ObjectMapper.Map<List<NhomHangHoaDto>>(lst);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public NhomHangHoaDto CreateNhomHangHoa(NhomHangHoaDto dto)
        {
            if (dto == null) { return new NhomHangHoaDto(); };
            DM_NhomHangHoa objNew = ObjectMapper.Map<DM_NhomHangHoa>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            _dmNhomHangHoa.InsertAsync(objNew);
            var result = ObjectMapper.Map<NhomHangHoaDto>(objNew);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<string> UpdateNhomHangHoa(NhomHangHoaDto dto)
        {
            try
            {
                if (dto == null) { return "Data null"; };
                DM_NhomHangHoa objUp = await _dmNhomHangHoa.FirstOrDefaultAsync(dto.Id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.MaNhomHang = dto.MaNhomHang;
                objUp.TenNhomHang = dto.TenNhomHang;
                objUp.LaNhomHangHoa = dto.LaNhomHangHoa;
                objUp.Color = dto.Color;
                objUp.MoTa = dto.MoTa;
                objUp.LastModifierUserId = AbpSession.UserId;
                objUp.LastModificationTime = DateTime.Now;
                await _dmNhomHangHoa.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
    }
}
