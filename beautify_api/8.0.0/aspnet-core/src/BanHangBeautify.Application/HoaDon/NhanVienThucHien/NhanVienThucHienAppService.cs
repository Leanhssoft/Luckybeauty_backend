using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Repository;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using BanHangBeautify.HoaDon.NhanVienThucHien.Dto;
using BanHangBeautify.HoaDon.NhanVienThucHien.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.NhanVienThucHien
{
    [AbpAuthorize]
    public class NhanVienThucHienAppService : SPAAppServiceBase
    {
        private readonly IRepository<BH_NhanVienThucHien, Guid> _nvThucHien;
        private readonly INhanVienThucHienRepository _nvthRepository;

        public NhanVienThucHienAppService(IRepository<BH_NhanVienThucHien, Guid> repository, INhanVienThucHienRepository nvthRepository)
        {
            _nvThucHien = repository;
            _nvthRepository = nvthRepository;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVienThucHien_Delete)]
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
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVienThucHien_Delete)]
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
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVienThucHien_Delete)]
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
        [HttpGet]
        public async Task<List<CreateOrEditNhanVienThucHienDto>> GetNhanVienThucHien_byIdHoaDon(Guid idHoaDon, Guid? idQuyHoaDon = null)
        {
            return await _nvthRepository.GetNhanVienThucHien_byIdHoaDon(idHoaDon, idQuyHoaDon);
        }
        [HttpGet]
        public async Task<List<CreateOrEditNhanVienThucHienDto>> GetNhanVienThucHien_byIdHoaDonChiTiet(Guid idHoaDonChiTiet)
        {
            return await _nvthRepository.GetNhanVienThucHien_byIdHoaDonChiTiet(idHoaDonChiTiet);
        }
        /// <summary>
        /// update all nvth by idHoaDon
        /// </summary>
        /// <param name="idHoaDon"></param>
        /// <param name="lstNV"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UpdateNhanVienThucHienn_byIdHoaDon(Guid idHoaDon, List<CreateOrEditNhanVienThucHienDto> lstNV = null)
        {
            var lstNVTH = await _nvThucHien.GetAllListAsync(x => x.IdHoaDon == idHoaDon);
            if (_nvThucHien != null && _nvThucHien.Count() > 0)
            {
                lstNVTH.ToList().ForEach(x => { x.IsDeleted = true; x.DeletionTime = DateTime.Now; x.DeleterUserId = AbpSession.UserId; });
            }
            return await _nvthRepository.UpdateNhanVienThucHien_byIdHoaDon(AbpSession.TenantId, idHoaDon, lstNV);
        }
        /// <summary>
        /// only update nvth by IdQuyHoaDon (theo thucthu)
        /// </summary>
        /// <param name="idHoaDon"></param>
        /// <param name="idQuyHoaDon"></param>
        /// <param name="lstNV"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UpdateNVThucHien_byIdQuyHoaDon(Guid idHoaDon, Guid idQuyHoaDon, List<CreateOrEditNhanVienThucHienDto> lstNV = null)
        {
            try
            {
                var lstNVTH = await _nvThucHien.GetAllListAsync(x => x.IdQuyHoaDon == idQuyHoaDon && x.LoaiChietKhau == 1);
                if (_nvThucHien != null && _nvThucHien.Count() > 0)
                {
                    lstNVTH.ToList().ForEach(x => { x.IsDeleted = true; x.DeletionTime = DateTime.Now; x.DeleterUserId = AbpSession.UserId; });
                }
                foreach (var nv in lstNV)
                {
                    var objNew = ObjectMapper.Map<BH_NhanVienThucHien>(nv);
                    objNew.Id = Guid.NewGuid();
                    objNew.TenantId = AbpSession.TenantId ?? 1;
                    objNew.CreationTime = DateTime.Now;
                    objNew.IdHoaDon = idHoaDon;
                    objNew.IdQuyHoaDon = idQuyHoaDon;
                    objNew.CreatorUserId = AbpSession.UserId;
                    objNew.IsDeleted = false;
                    await _nvThucHien.InsertAsync(objNew);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<bool> UpdateNVThucHien_byIdHoaDonChiTiet(Guid idHoaDonChiTiet, List<CreateOrEditNhanVienThucHienDto> lstNV = null)
        {
            try
            {
                var lstNVTH = await _nvThucHien.GetAllListAsync(x => x.IdHoaDonChiTiet == idHoaDonChiTiet);
                if (_nvThucHien != null && _nvThucHien.Count() > 0)
                {
                    lstNVTH.ToList().ForEach(x => { x.IsDeleted = true; x.DeletionTime = DateTime.Now; x.DeleterUserId = AbpSession.UserId; });
                }
                foreach (var nv in lstNV)
                {
                    var objNew = ObjectMapper.Map<BH_NhanVienThucHien>(nv);
                    objNew.Id = Guid.NewGuid();
                    objNew.TenantId = AbpSession.TenantId ?? 1;
                    objNew.CreationTime = DateTime.Now;
                    objNew.IdHoaDonChiTiet = idHoaDonChiTiet;
                    objNew.CreatorUserId = AbpSession.UserId;
                    objNew.IsDeleted = false;
                    await _nvThucHien.InsertAsync(objNew);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpGet]
        public async Task<bool> UpdateTienChietKhau_forNhanVien_whenUpdateCTHD(Guid idHoaDonChiTiet, double soLuoongOld)
        {
            try
            {
                return await _nvthRepository.UpdateTienChietKhau_forNhanVien_whenUpdateCTHD(idHoaDonChiTiet, soLuoongOld);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
