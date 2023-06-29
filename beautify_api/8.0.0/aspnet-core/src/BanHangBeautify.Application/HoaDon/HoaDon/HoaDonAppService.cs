﻿using Abp.Authorization;
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
using Abp.Application.Services.Dto;
using NPOI.OpenXmlFormats.Wordprocessing;

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

                    if (item.nhanVienThucHien != null)
                    {
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
                }
                await _hoaDonRepository.InsertAsync(objHD);
                await _hoaDonChiTietRepository.InsertRangeAsync(lstCTHoaDon);
                if (lstNVTH != null && lstNVTH.Count > 0)
                {
                    await _nvThucHien.InsertRangeAsync(lstNVTH);
                }
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
        [HttpPost]
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

                    if (item.nhanVienThucHien != null)
                    {
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
        /// <summary>
        /// chỉ cập nhật thông tin hóa đơn, không cập nhật chi tiết hóa đơn
        /// </summary>
        /// <param name="objUp"></param>
        public async Task<CreateHoaDonDto> Update_InforHoaDon(CreateHoaDonDto objUp)
        {
            BH_HoaDon objOld = await _hoaDonRepository.FirstOrDefaultAsync(objUp.Id);
            if (objOld != null)
            {
                if (string.IsNullOrEmpty(objUp.MaHoaDon))
                {
                    objOld.MaHoaDon = await _repoHoaDon.GetMaHoaDon(AbpSession.TenantId ?? 1, objUp.IdChiNhanh, objUp.IdLoaiChungTu, objUp.NgayLapHoaDon);
                }
                else
                {
                    objOld.MaHoaDon = objUp.MaHoaDon;
                }
                objOld.NgayLapHoaDon = objUp.NgayLapHoaDon;
                objOld.NgayApDung = objUp.NgayApDung;
                objOld.NgayHetHan = objUp.NgayHetHan;
                objOld.IdChiNhanh = objUp.IdChiNhanh;
                objOld.IdKhachHang = objUp.IdKhachHang;
                objOld.IdNhanVien = objUp.IdNhanVien;
                objOld.IdPhong = objUp.IdViTriPhong;
                objOld.IdHoaDon = objUp.IdHoaDon;
                objOld.TongTienHangChuaChietKhau = objUp.TongTienHangChuaChietKhau;
                objOld.PTChietKhauHang = objUp.PTChietKhauHang;
                objOld.TongChietKhauHangHoa = objUp.TongChietKhauHangHoa;
                objOld.TongTienHang = objUp.TongTienHang;
                objOld.PTThueHD = objUp.PTThueHD;
                objOld.TongTienThue = objUp.TongTienThue;
                objOld.TongTienHDSauVAT = objUp.TongTienHDSauVAT;
                objOld.PTGiamGiaHD = objUp.PTGiamGiaHD;
                objOld.TongGiamGiaHD = objUp.TongGiamGiaHD;
                objOld.ChiPhiTraHang = objUp.ChiPhiTraHang;
                objOld.TongThanhToan = objUp.TongThanhToan;
                objOld.ChiPhiHD = objUp.ChiPhiHD;
                objOld.ChiPhi_GhiChu = objUp.ChiPhi_GhiChu;
                objOld.DiemGiaoDich = objUp.DiemGiaoDich;
                objOld.GhiChuHD = objUp.GhiChuHD;
                objOld.TrangThai = objUp.TrangThai;
                objOld.LastModifierUserId = AbpSession.UserId;
                objOld.LastModificationTime = DateTime.Now;

                await _hoaDonRepository.UpdateAsync(objOld);
                var dataHD = ObjectMapper.Map<CreateHoaDonDto>(objOld);
                return dataHD;
            }
            return new CreateHoaDonDto();
        }
        /// <summary>
        /// chỉ cập nhật chi tiết hóa đơn
        /// </summary>
        /// <param name="lstCT"></param>
        /// <param name="idHoadon"></param>
        /// <returns></returns>
        public async Task Update_ChiTietHoaDon(List<HoaDonChiTietDto> lstCT, Guid idHoadon)
        {
            var userID = AbpSession.UserId;
            List<BH_NhanVienThucHien> lstNVTH = new();

            #region Delete ct if not exist
            // comapre old & new
            var lstOld = await _hoaDonChiTietRepository.GetAllListAsync(x => x.IdHoaDon == idHoadon);
            var ctDelete = (from ctOld in lstOld
                            join ctNew in lstCT on ctOld.Id equals ctNew.Id into delete
                            from de in delete.DefaultIfEmpty()
                            where de == null
                            select ctOld.Id);
            // update ct with TrangThai = 0 if delete
            (await _hoaDonChiTietRepository.GetAllListAsync(x => ctDelete.Contains(x.Id))).ToList().ForEach(x =>
            { x.TrangThai = 0; x.DeleterUserId = userID; x.DeletionTime = DateTime.Now; });
            #endregion

            foreach (var item in lstCT)
            {
                BH_HoaDon_ChiTiet ctUpdate = await _hoaDonChiTietRepository.FirstOrDefaultAsync(item.Id);
                if (ctUpdate != null)
                {
                    ctUpdate.STT = item.STT;
                    ctUpdate.SoLuong = item.STT;
                    ctUpdate.IdDonViQuyDoi = item.IdDonViQuyDoi;
                    ctUpdate.IdChiTietHoaDon = item.IdChiTietHoaDon;
                    ctUpdate.DonGiaTruocCK = item.DonGiaTruocCK;
                    ctUpdate.ThanhTienTruocCK = item.ThanhTienTruocCK;
                    ctUpdate.PTChietKhau = item.PTChietKhau;
                    ctUpdate.TienChietKhau = item.TienChietKhau;
                    ctUpdate.DonGiaSauCK = item.DonGiaSauCK;
                    ctUpdate.ThanhTienSauCK = item.ThanhTienSauCK;
                    ctUpdate.PTThue = item.PTThue;
                    ctUpdate.TienThue = item.TienThue;
                    ctUpdate.DonGiaSauVAT = item.DonGiaSauVAT;
                    ctUpdate.ThanhTienSauVAT = item.ThanhTienSauVAT;
                    ctUpdate.GhiChu = item.GhiChu;
                    ctUpdate.TrangThai = item.TrangThai;
                    ctUpdate.LastModifierUserId = userID;
                    ctUpdate.LastModificationTime = DateTime.Now;
                    await _hoaDonChiTietRepository.UpdateAsync(ctUpdate);

                    _nvthService.DeleteNVThucHienDichVu(item.Id);

                    if (item.nhanVienThucHien != null)
                    {
                        foreach (var nvth in item.nhanVienThucHien)
                        {
                            BH_NhanVienThucHien nvNew = ObjectMapper.Map<BH_NhanVienThucHien>(nvth);
                            nvNew.Id = Guid.NewGuid();
                            nvNew.IdHoaDonChiTiet = ctUpdate.Id;
                            nvNew.TenantId = AbpSession.TenantId ?? 1;
                            nvNew.CreatorUserId = userID;
                            nvNew.CreationTime = DateTime.Now;
                            lstNVTH.Add(nvNew);
                        }
                    }
                }
                else
                {
                    // insert new ct
                    BH_HoaDon_ChiTiet ctNew = ObjectMapper.Map<BH_HoaDon_ChiTiet>(item);
                    ctNew.Id = Guid.NewGuid();
                    ctNew.IdHoaDon = idHoadon;
                    ctNew.CreatorUserId = AbpSession.UserId;
                    ctNew.CreationTime = DateTime.Now;
                    await _hoaDonChiTietRepository.InsertAsync(ctNew);

                    if (item.nhanVienThucHien != null)
                    {
                        foreach (var nvth in item.nhanVienThucHien)
                        {
                            BH_NhanVienThucHien nvNew = ObjectMapper.Map<BH_NhanVienThucHien>(nvth);
                            nvNew.Id = Guid.NewGuid();
                            nvNew.IdHoaDonChiTiet = ctNew.Id;
                            nvNew.TenantId = AbpSession.TenantId ?? 1;
                            nvNew.CreatorUserId = userID;
                            nvNew.CreationTime = DateTime.Now;
                            lstNVTH.Add(nvNew);
                        }
                    }
                }
            }
            if (lstNVTH.Count > 0)
            {
                await _nvThucHien.InsertRangeAsync(lstNVTH);
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
                        item.TrangThai = 0;
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
                hoaDon.TrangThai = 0;
                await _hoaDonRepository.UpdateAsync(hoaDon);
            }
        }
        public async Task<List<PageHoaDonDto>> GetInforHoaDon_byId(Guid id)
        {
            return await _repoHoaDon.GetInforHoaDon_byId(id);
        }
        public async Task<List<PageHoaDonChiTietDto>> GetChiTietHoaDon_byIdHoaDon(Guid idHoaDon)
        {
            return await _repoHoaDon.GetChiTietHoaDon_byIdHoaDon(idHoaDon);
        }

        [HttpPost]
        public async Task<PagedResultDto<PageHoaDonDto>> GetListHoaDon(HoaDonRequestDto param)
        {
            return await _repoHoaDon.GetListHoaDon(param, AbpSession.TenantId ?? 1);
        }
    }
}
