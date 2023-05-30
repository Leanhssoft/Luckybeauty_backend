using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using BanHangBeautify.HoaDon.NhanVienThucHien.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien
{
    //[AbpAuthorize(PermissionNames.Pages_NhanVienThucHien)]
    public class NhanVienThucHienAppService : SPAAppServiceBase
    {
        private readonly IRepository<BH_NhanVienThucHien, Guid> _nvThucHien;
        public NhanVienThucHienAppService(IRepository<BH_NhanVienThucHien, Guid> repository)
        {
            _nvThucHien = repository;
        }
        public async Task<bool> InsertListNVThucHien_DichVu(List<HoaDonChiTietDto> lstCTHD)
        {
            try
            {
                List<BH_NhanVienThucHien> lstNVTH = new List<BH_NhanVienThucHien>();
                foreach (var cthd in lstCTHD)
                {
                    foreach (var nvth in cthd.nhanVienThucHien)
                    {
                        BH_NhanVienThucHien nvNew = ObjectMapper.Map<BH_NhanVienThucHien>(nvth);
                        nvNew.Id = Guid.NewGuid();
                        nvNew.TenantId = AbpSession.TenantId ?? 1;
                        nvNew.CreatorUserId = AbpSession.UserId;
                        nvNew.CreationTime = DateTime.Now;
                        lstNVTH.Add(nvNew);
                    }
                }
                await _nvThucHien.InsertRangeAsync(lstNVTH);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateNVThucHienDichVu([FromBody] JObject data, Guid idHoaDonChiTiet)
        {
            try
            {
                List<NhanVienThucHienDto> lstNV = data["NhanViens"].ToObject<List<NhanVienThucHienDto>>();

                // upadate status & add again
                _nvThucHien.GetAllIncluding(x => x.IdHoaDonChiTiet == idHoaDonChiTiet).ToList()
                    .ForEach(x => { x.IsDeleted = true; x.DeletionTime = DateTime.Now; x.DeleterUserId = AbpSession.UserId; });

                List<BH_NhanVienThucHien> lstNVAdd = new();
                foreach (var item in lstNV)
                {
                    var objNew = ObjectMapper.Map<BH_NhanVienThucHien>(item);
                    objNew.Id = Guid.NewGuid();
                    objNew.CreationTime = DateTime.Now;
                    objNew.IdHoaDonChiTiet = idHoaDonChiTiet;
                    objNew.CreatorUserId = AbpSession.UserId;
                    objNew.TenantId = AbpSession.TenantId ?? 1;
                    objNew.IsDeleted = false;
                    lstNVAdd.Add(objNew);
                }
                await _nvThucHien.InsertRangeAsync(lstNVAdd);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<NhanVienThucHienDto> Delete(Guid id)
        {
            var data = await _nvThucHien.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession.UserId;
                await _nvThucHien.UpdateAsync(data);
                return ObjectMapper.Map<NhanVienThucHienDto>(data);
            }
            return new NhanVienThucHienDto();
        }

        public bool DeleteNVThucHienDichVu(Guid idHoaDonChiTiet)
        {
            try
            {
                _nvThucHien.GetAllIncluding(x => x.IdHoaDonChiTiet == idHoaDonChiTiet).ToList()
                  .ForEach(x => { x.IsDeleted = true; x.DeletionTime = DateTime.Now; x.DeleterUserId = AbpSession.UserId; });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        public bool DeleteNVThucHienHoaDon(Guid idHoaDon)
        {
            try
            {
                _nvThucHien.GetAllIncluding(x => x.IdHoaDon == idHoaDon).ToList()
                  .ForEach(x => { x.IsDeleted = true; x.DeletionTime = DateTime.Now; x.DeleterUserId = AbpSession.UserId; });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CreateOrEditNhanVienThucHienDto> GetForEdit(Guid id)
        {
            var data = await _nvThucHien.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditNhanVienThucHienDto>(data);
            }
            return new CreateOrEditNhanVienThucHienDto();
        }
        public async Task<PagedResultDto<NhanVienThucHienDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<NhanVienThucHienDto> result = new PagedResultDto<NhanVienThucHienDto>();
            var lstData = await _nvThucHien.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<NhanVienThucHienDto>>(lstData);
            return result;
        }
    }
}
