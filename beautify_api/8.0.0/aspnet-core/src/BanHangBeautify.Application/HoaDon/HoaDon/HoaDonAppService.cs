using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Common.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanHangBeautify.HoaDon.HoaDon.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using static BanHangBeautify.Common.CommonClass;
using BanHangBeautify.HoaDon.NhanVienThucHien;
using OfficeOpenXml.Style;

namespace BanHangBeautify.HoaDon.HoaDon
{
    //[AbpAuthorize(PermissionNames.Pages_HoaDon)]
    public class HoaDonAppService : SPAAppServiceBase
    {
        private readonly IRepository<BH_HoaDon, Guid> _hoaDonRepository;
        private readonly IRepository<BH_HoaDon_ChiTiet, Guid> _hoaDonChiTietRepository;
        private readonly IRepository<BH_HoaDon_Anh, Guid> _hoaDonAnhRepository;
        private readonly IRepository<BH_NhanVienThucHien, Guid> _nvThucHien;
        private readonly IHoaDonRepository _repoHoaDon;
        private readonly NhanVienThucHienAppService _nvthService;
        public HoaDonAppService(
            IRepository<BH_HoaDon, Guid> hoaDonRepository,
            IRepository<BH_NhanVienThucHien, Guid> nvThucHien,
            IRepository<BH_HoaDon_ChiTiet, Guid> hoaDonChiTietRepository,
            IRepository<BH_HoaDon_Anh, Guid> hoaDonAnhRepository,
            NhanVienThucHienAppService nvthService,
            IHoaDonRepository repoHoaDon
        )
        {
            _hoaDonRepository = hoaDonRepository;
            _nvThucHien = nvThucHien;
            _hoaDonChiTietRepository = hoaDonChiTietRepository;
            _hoaDonAnhRepository = hoaDonAnhRepository;
            _nvthService = nvthService;
            _repoHoaDon = repoHoaDon;
        }
        public async Task<CreateHoaDonDto> CreateHoaDon(CreateHoaDonDto dto)
        {
            List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
            BH_HoaDon objHD = ObjectMapper.Map<BH_HoaDon>(dto);

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;

            if (string.IsNullOrEmpty(objHD.MaHoaDon))
            {
                var maChungTu = await _repoHoaDon.FnGetMaHoaDon(AbpSession.TenantId ?? 1, dto.IdChiNhanh, dto.IdLoaiChungTu, dto.NgayLapHoaDon);
                objHD.MaHoaDon = maChungTu;
            }
            if (dto.HoaDonChiTiet != null)
            {
                List<BH_NhanVienThucHien> lstNVTH = new();
                foreach (var item in dto.HoaDonChiTiet)
                {
                    BH_HoaDon_ChiTiet ctNew = ObjectMapper.Map<BH_HoaDon_ChiTiet>(item);
                    ctNew.Id = Guid.NewGuid();
                    ctNew.IdHoaDon = objHD.Id;
                    ctNew.TenantId = AbpSession.TenantId ?? 1;
                    ctNew.CreatorUserId = AbpSession.UserId;
                    ctNew.CreationTime = DateTime.Now;
                    lstCTHoaDon.Add(ctNew);

                    foreach (var nvth in item.nhanVienThucHien)
                    {
                        BH_NhanVienThucHien nvNew = ObjectMapper.Map<BH_NhanVienThucHien>(nvth);
                        nvNew.Id = Guid.NewGuid();
                        nvNew.IdHoaDonChiTiet = ctNew.Id;
                        nvNew.TenantId = AbpSession.TenantId ?? 1;
                        nvNew.CreatorUserId = AbpSession.UserId;
                        nvNew.CreationTime = DateTime.Now;
                        lstNVTH.Add(nvNew);
                    }
                }
                await _hoaDonRepository.InsertAsync(objHD);
                await _hoaDonChiTietRepository.InsertRangeAsync(lstCTHoaDon);
                await _nvThucHien.InsertRangeAsync(lstNVTH);
            }

            var result = ObjectMapper.Map<CreateHoaDonDto>(objHD);
            result.HoaDonChiTiet = ObjectMapper.Map<List<HoaDonChiTietDto>>(lstCTHoaDon);
            return result;
        }

        public async Task<CreateHoaDonDto> CreateHoaDon2([FromBody] JObject data)
        {
            List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
            BH_HoaDon objHD = ObjectMapper.Map<BH_HoaDon>(data["hoadon"].ToObject<BH_HoaDon>());
            List<BH_HoaDon_ChiTiet> dataChiTietHD = ObjectMapper.Map<List<BH_HoaDon_ChiTiet>>(data["hoadonChiTiet"].ToObject<List<BH_HoaDon_ChiTiet>>());

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;

            if (string.IsNullOrEmpty(objHD.MaHoaDon))
            {
                var maChungTu = await _repoHoaDon.FnGetMaHoaDon(AbpSession.TenantId ?? 1, objHD.IdChiNhanh ?? null,
                    objHD.IdLoaiChungTu, objHD.NgayLapHoaDon);
                objHD.MaHoaDon = maChungTu;
            }
            foreach (var item in dataChiTietHD)
            {
                BH_HoaDon_ChiTiet ctNew = ObjectMapper.Map<BH_HoaDon_ChiTiet>(item);
                ctNew.Id = Guid.NewGuid();
                ctNew.IdHoaDon = objHD.Id;
                ctNew.TenantId = AbpSession.TenantId ?? 1;
                ctNew.CreatorUserId = AbpSession.UserId;
                ctNew.CreationTime = DateTime.Now;
                lstCTHoaDon.Add(ctNew);
                // toddo NVThucHien
            }
            await _hoaDonRepository.InsertAsync(objHD);
            await _hoaDonChiTietRepository.InsertRangeAsync(lstCTHoaDon);

            //objHD.BH_HoaDon_ChiTiet = lstCTHoaDon;
            var result = ObjectMapper.Map<CreateHoaDonDto>(objHD);
            result.HoaDonChiTiet = ObjectMapper.Map<List<HoaDonChiTietDto>>(lstCTHoaDon);
            return result;
        }

        public async Task<CreateHoaDonDto> UpdateHoaDon(CreateHoaDonDto objUp)
        {
            try
            {
                List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
                List<BH_NhanVienThucHien> lstNVTH = new();
                BH_HoaDon objOld = await _hoaDonRepository.FirstOrDefaultAsync(objUp.Id);

                if (string.IsNullOrEmpty(objUp.MaHoaDon))
                {
                    objUp.MaHoaDon = await _repoHoaDon.GetMaHoaDon(AbpSession.TenantId ?? 1, objUp.IdChiNhanh, objUp.IdLoaiChungTu, objUp.NgayLapHoaDon);
                }

                objOld = ObjectMapper.Map<BH_HoaDon>(objUp);
                objOld.LastModifierUserId = AbpSession.UserId;
                objOld.LastModificationTime = DateTime.Now;

                // remove all nvth of cthd + add again
                foreach (var item in objUp.HoaDonChiTiet)
                {
                    BH_HoaDon_ChiTiet ctUpdate = await _hoaDonChiTietRepository.FirstOrDefaultAsync(item.Id);
                    BH_HoaDon_ChiTiet ctUp = ObjectMapper.Map<BH_HoaDon_ChiTiet>(ctUpdate);
                    ctUp.CreatorUserId = AbpSession.UserId;
                    ctUp.CreationTime = DateTime.Now;
                    await _hoaDonChiTietRepository.UpdateAsync(ctUp);

                    _nvthService.DeleteNVThucHienDichVu(item.Id);

                    foreach (var nvth in item.nhanVienThucHien)
                    {
                        BH_NhanVienThucHien nvNew = ObjectMapper.Map<BH_NhanVienThucHien>(nvth);
                        nvNew.Id = Guid.NewGuid();
                        nvNew.IdHoaDonChiTiet = ctUp.Id;
                        nvNew.TenantId = AbpSession.TenantId ?? 1;
                        nvNew.CreatorUserId = AbpSession.UserId;
                        nvNew.CreationTime = DateTime.Now;
                        lstNVTH.Add(nvNew);
                    }
                }
                await _hoaDonRepository.UpdateAsync(objOld);
                await _nvThucHien.InsertRangeAsync(lstNVTH);

                var dataHD = ObjectMapper.Map<CreateHoaDonDto>(objUp);
                dataHD.HoaDonChiTiet = ObjectMapper.Map<List<HoaDonChiTietDto>>(lstCTHoaDon);
                return dataHD;

            }
            catch (Exception ex)
            {
                return new CreateHoaDonDto();
            }
        }
        [HttpPost]
        public async Task DeleteHoaDon(Guid id)
        {
            var hoaDon = _hoaDonRepository.FirstOrDefault(x => x.Id == id);
            if (hoaDon != null)
            {
                var hoaDonCTs = await _hoaDonChiTietRepository.GetAll().Where(x => x.IsDeleted == false && x.IdHoaDon == hoaDon.Id).ToListAsync();
                if (hoaDonCTs != null || hoaDonCTs.Count > 0)
                {
                    foreach (var item in hoaDonCTs)
                    {
                        item.IsDeleted = true;
                        item.DeleterUserId = AbpSession.UserId;
                        item.DeletionTime = DateTime.Now;
                        await _hoaDonChiTietRepository.UpdateAsync(item);
                    }
                }

                var hoaDonAnh = await _hoaDonAnhRepository.GetAll().Where(x => x.IdHoaDon == hoaDon.IdHoaDon && x.IsDeleted == false).ToListAsync();
                if (hoaDonAnh != null || hoaDonAnh.Count > 0)
                {
                    foreach (var item in hoaDonAnh)
                    {
                        item.IsDeleted = true;
                        item.DeleterUserId = AbpSession.UserId;
                        item.DeletionTime = DateTime.Now;
                        await _hoaDonAnhRepository.UpdateAsync(item);
                    }
                }
                hoaDon.IsDeleted = true;
                hoaDon.DeleterUserId = AbpSession.UserId;
                hoaDon.DeletionTime = DateTime.Now;
                await _hoaDonRepository.UpdateAsync(hoaDon);
            }
        }
        public async Task GetHoaDon(Guid id) { }
        public async Task GetllHoaDon() { }
    }
}
