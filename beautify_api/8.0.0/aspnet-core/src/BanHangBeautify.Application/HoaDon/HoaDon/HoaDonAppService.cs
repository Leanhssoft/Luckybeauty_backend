using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.HoaDon.HoaDon.Repository;
using BanHangBeautify.HoaDon.HoaDonChiTiet.Dto;
using BanHangBeautify.HoaDon.NhanVienThucHien;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanHangBeautify.Zalo.GuiTinNhan;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.NhatKyHoatDong;
using Microsoft.AspNetCore.SignalR;
using BanHangBeautify.SignalR;
using NPOI.OpenXmlFormats.Shared;
using NPOI.OpenXmlFormats.Wordprocessing;
using static BanHangBeautify.AppCommon.CommonClass;
using NPOI.HPSF;
using BanHangBeautify.NewFolder;
using System.IO;
using OfficeOpenXml;
using BanHangBeautify.HoaDon.LoaiChungTu;
using BanHangBeautify.KhachHang.KhachHang;
using BanHangBeautify.HangHoa.HangHoa;
using BanHangBeautify.Migrations;

namespace BanHangBeautify.HoaDon.HoaDon
{
    [AbpAuthorize]
    public class HoaDonAppService : SPAAppServiceBase
    {
        private readonly IRepository<BH_HoaDon, Guid> _hoaDonRepository;
        private readonly IRepository<BH_HoaDon_ChiTiet, Guid> _hoaDonChiTietRepository;
        private readonly IRepository<BH_HoaDon_Anh, Guid> _hoaDonAnhRepository;
        private readonly IRepository<BH_NhanVienThucHien, Guid> _nvThucHien;
        private readonly NhanVienThucHienAppService _nvthService;
        private readonly IHoaDonRepository _repoHoaDon;
        private readonly ILoaiChungTuAppService _loaiChungTuService;
        private readonly IKhachHangAppService _khachHangService;
        private readonly IHangHoaAppService _hangHoaAppService;
        private readonly IExcelBase _excelBase;
        private readonly IHubContext<InvoiceHub> _invoiceHubContext;

        public HoaDonAppService(
            IRepository<BH_HoaDon, Guid> hoaDonRepository,
            IRepository<BH_HoaDon_ChiTiet, Guid> hoaDonChiTietRepository,
            IRepository<BH_HoaDon_Anh, Guid> hoaDonAnhRepository,
            IRepository<BH_NhanVienThucHien, Guid> nvThucHien,
            NhanVienThucHienAppService nvthService,
            IHoaDonRepository repoHoaDon,
             ILoaiChungTuAppService loaiChungTuService,
             IKhachHangAppService khachHangService,
             IHangHoaAppService hangHoaAppService,
            IExcelBase excelBase,
             IHubContext<InvoiceHub> invoiceHubContext
        )
        {
            _hoaDonRepository = hoaDonRepository;
            _hoaDonChiTietRepository = hoaDonChiTietRepository;
            _hoaDonAnhRepository = hoaDonAnhRepository;
            _nvThucHien = nvThucHien;
            _nvthService = nvthService;
            _repoHoaDon = repoHoaDon;
            _excelBase = excelBase;
            _khachHangService = khachHangService;
            _hangHoaAppService = hangHoaAppService;
            _invoiceHubContext = invoiceHubContext;
            _loaiChungTuService = loaiChungTuService;
        }

        [HttpPost]
        /// <summary>
        /// only insert to BH_HoaDon
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<CreateHoaDonDto> InsertBH_HoaDon(CreateHoaDonDto dto)
        {
            BH_HoaDon objHD = ObjectMapper.Map<BH_HoaDon>(dto);

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;
            objHD.NgayLapHoaDon = ObjectHelper.AddTimeNow_forDate(objHD.NgayLapHoaDon);

            if (string.IsNullOrEmpty(objHD.MaHoaDon))
            {
                var maChungTu = await _repoHoaDon.FnGetMaHoaDon(AbpSession.TenantId ?? 1, dto.IdChiNhanh, dto.IdLoaiChungTu, dto.NgayLapHoaDon);
                objHD.MaHoaDon = maChungTu;
            }
            await _hoaDonRepository.InsertAsync(objHD);
            var result = ObjectMapper.Map<CreateHoaDonDto>(objHD);
            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_HoaDon_Create)]
        public async Task<CreateHoaDonDto> CreateHoaDon(CreateHoaDonDto dto)
        {
            List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
            BH_HoaDon objHD = ObjectMapper.Map<BH_HoaDon>(dto);

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;
            objHD.NgayLapHoaDon = ObjectHelper.AddTimeNow_forDate(objHD.NgayLapHoaDon);

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
            await _invoiceHubContext.Clients.All.SendAsync("ReceiveInvoiceListReload", AbpSession.TenantId.HasValue ? AbpSession.TenantId.Value.ToString() : "null");
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_HoaDon_Edit)]
        public async Task<CreateHoaDonDto> UpdateHoaDon(CreateHoaDonDto objUp)
        {
            try
            {
                List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
                List<BH_NhanVienThucHien> lstNVTH = new();
                BH_HoaDon objOld = await _hoaDonRepository.FirstOrDefaultAsync(objUp.Id);

                if (string.IsNullOrEmpty(objUp.MaHoaDon))
                {
                    objUp.MaHoaDon = await _repoHoaDon.FnGetMaHoaDon(AbpSession.TenantId ?? 1, objUp.IdChiNhanh, objUp.IdLoaiChungTu, objUp.NgayLapHoaDon);
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
        [AbpAuthorize(PermissionNames.Pages_HoaDon_Edit)]
        [HttpPost]
        public async Task<CreateHoaDonDto> Update_InforHoaDon(CreateHoaDonDto objUp)
        {
            BH_HoaDon objOld = await _hoaDonRepository.FirstOrDefaultAsync(objUp.Id);
            if (objOld != null)
            {
                if (string.IsNullOrEmpty(objUp.MaHoaDon))
                {
                    objOld.MaHoaDon = await _repoHoaDon.FnGetMaHoaDon(AbpSession.TenantId ?? 1, objUp.IdChiNhanh, objUp.IdLoaiChungTu, objUp.NgayLapHoaDon);
                }
                else
                {
                    objOld.MaHoaDon = objUp.MaHoaDon;
                }
                objOld.NgayLapHoaDon = ObjectHelper.AddTimeNow_forDate(objUp.NgayLapHoaDon);
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

        [HttpGet]
        public async Task<CreateHoaDonDto> UpdateTongTienHoaDon_ifChangeCTHD(Guid idHoaDon)
        {
            var lstCTHD = _hoaDonChiTietRepository.GetAll().Where(x => x.IdHoaDon == idHoaDon).ToList();
            if (lstCTHD.Any())
            {
                double tongTienHangChuaCK = 0, tongChietKhauHang = 0, tongTienHang = 0, tongTienThue = 0, tongTienHDSauVAT = 0;
                foreach (var item in lstCTHD)
                {
                    tongTienHangChuaCK += item?.ThanhTienTruocCK ?? 0;
                    tongChietKhauHang += (item?.SoLuong ?? 0) * (item?.TienChietKhau ?? 0);
                    tongTienHang += item?.ThanhTienSauCK ?? 0;
                    tongTienThue += (item?.SoLuong ?? 0) * (item?.TienThue ?? 0);
                    tongTienHDSauVAT += item?.ThanhTienSauVAT ?? 0;
                }
                BH_HoaDon hoadon = _hoaDonRepository.Get(idHoaDon);
                if (hoadon != null)
                {
                    hoadon.TongTienHangChuaChietKhau = tongTienHangChuaCK;
                    hoadon.TongChietKhauHangHoa = tongChietKhauHang;
                    hoadon.TongTienHang = tongTienHang;
                    hoadon.TongTienThue = tongTienThue;
                    hoadon.TongTienHDSauVAT = tongTienHDSauVAT;
                    hoadon.TongThanhToan = tongTienHDSauVAT - hoadon.TongGiamGiaHD;
                    await _hoaDonRepository.UpdateAsync(hoadon);

                    var dataHD = ObjectMapper.Map<CreateHoaDonDto>(hoadon);
                    return dataHD;
                }
            }
            return null;
        }
        /// <summary>
        /// chỉ cập nhật chi tiết hóa đơn
        /// </summary>
        /// <param name="lstCT"></param>
        /// <param name="idHoadon"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_HoaDon_Edit)]
        [HttpPost]
        public async Task<List<HoaDonChiTietDto>> Update_ChiTietHoaDon(List<HoaDonChiTietDto> lstCT, Guid idHoadon)
        {
            var userID = AbpSession.UserId;
            List<BH_NhanVienThucHien> lstNVTH = new();
            List<BH_HoaDon_ChiTiet> ctAfter = new();

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
            { x.TrangThai = TrangThaiHoaDonConst.DA_HUY; x.IsDeleted = true; x.DeleterUserId = userID; x.DeletionTime = DateTime.Now; });
            #endregion

            // keep ctOld & add ctNew
            foreach (var item in lstCT)
            {
                BH_HoaDon_ChiTiet ctUpdate = await _hoaDonChiTietRepository.FirstOrDefaultAsync(item.Id);
                if (ctUpdate != null)
                {
                    ctUpdate.STT = item?.STT ?? 0;
                    ctUpdate.SoLuong = item.SoLuong;
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
                    ctAfter.Add(ctUpdate);

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
                    ctAfter.Add(ctNew);

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
            return ObjectMapper.Map<List<HoaDonChiTietDto>>(ctAfter);
        }

        [HttpPost]
        public async Task<HoaDonChiTietDto> CreateOrUpdateCTHD_byIdChiTiet(HoaDonChiTietDto cthd)
        {
            var ctUpdate = await _hoaDonChiTietRepository.FirstOrDefaultAsync(cthd.Id);
            if (ctUpdate != null)
            {
                ctUpdate.STT = cthd.STT ?? 0;
                ctUpdate.IdDonViQuyDoi = cthd.IdDonViQuyDoi;
                ctUpdate.IdChiTietHoaDon = cthd.IdChiTietHoaDon;
                ctUpdate.SoLuong = cthd.SoLuong;
                ctUpdate.PTChietKhau = cthd.PTChietKhau;
                ctUpdate.TienChietKhau = cthd.TienChietKhau;
                ctUpdate.PTThue = cthd.PTThue;
                ctUpdate.TienThue = cthd.TienThue;
                ctUpdate.DonGiaTruocCK = cthd.DonGiaTruocCK;
                ctUpdate.DonGiaSauCK = cthd.DonGiaSauCK;
                ctUpdate.DonGiaSauVAT = cthd.DonGiaSauVAT;
                ctUpdate.ThanhTienTruocCK = cthd.ThanhTienTruocCK;
                ctUpdate.ThanhTienSauCK = cthd.ThanhTienSauCK;
                ctUpdate.ThanhTienSauVAT = cthd.ThanhTienSauVAT;
                ctUpdate.GhiChu = cthd.GhiChu;
                ctUpdate.LastModificationTime = DateTime.Now;
                ctUpdate.LastModifierUserId = AbpSession.UserId;
                await _hoaDonChiTietRepository.UpdateAsync(ctUpdate);
                return cthd;
            }
            else
            {
                BH_HoaDon_ChiTiet ctNew = ObjectMapper.Map<BH_HoaDon_ChiTiet>(cthd);
                ctNew.Id = Guid.NewGuid();
                ctNew.TenantId = AbpSession.TenantId ?? 1;
                ctNew.CreationTime = DateTime.Now;
                ctNew.CreatorUserId = AbpSession.UserId;
                await _hoaDonChiTietRepository.InsertAsync(ctNew);
                var result = ObjectMapper.Map<HoaDonChiTietDto>(ctNew);
                return result;
            }
        }
        [AbpAuthorize(PermissionNames.Pages_HoaDon_Delete)]
        [HttpGet]
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
                        item.TrangThai = TrangThaiHoaDonConst.DA_HUY;
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
                hoaDon.TrangThai = TrangThaiHoaDonConst.DA_HUY;
                await _hoaDonRepository.UpdateAsync(hoaDon);
            }
        }

        [HttpGet]
        public async Task<bool> KhoiPhucHoaDon(Guid idHoaDon)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var hoaDon = _hoaDonRepository.FirstOrDefault(x => x.Id == idHoaDon);
                if (hoaDon != null)
                {
                    var lastDeleteTime = ConvertHelper.ConverDateTimeToString(hoaDon.DeletionTime, "yyyy-MM-dd HH:mm:ss");
                    var hoaDonCTs = await _hoaDonChiTietRepository.GetAll().Where(x => x.IdHoaDon == hoaDon.Id).ToListAsync();
                    if (hoaDonCTs != null || hoaDonCTs.Count > 0)
                    {
                        // only get cthd was delete lastest
                        var lstCT = hoaDonCTs.Where(x => ConvertHelper.ConverDateTimeToString(x.DeletionTime, "yyyy-MM-dd HH:mm:ss") == lastDeleteTime);
                        foreach (var item in lstCT)
                        {
                            item.IsDeleted = false;
                            item.LastModifierUserId = AbpSession.UserId;
                            item.LastModificationTime = DateTime.Now;
                            item.TrangThai = TrangThaiHoaDonConst.HOAN_THANH;
                            await _hoaDonChiTietRepository.UpdateAsync(item);
                        }
                    }

                    var hoaDonAnh = await _hoaDonAnhRepository.GetAll().Where(x => x.IdHoaDon == hoaDon.IdHoaDon).ToListAsync();
                    if (hoaDonAnh != null || hoaDonAnh.Count > 0)
                    {
                        foreach (var item in hoaDonAnh)
                        {
                            item.IsDeleted = false;
                            item.LastModifierUserId = AbpSession.UserId;
                            item.LastModificationTime = DateTime.Now;
                            await _hoaDonAnhRepository.UpdateAsync(item);
                        }
                    }
                    hoaDon.IsDeleted = false;
                    hoaDon.LastModifierUserId = AbpSession.UserId;
                    hoaDon.LastModificationTime = DateTime.Now;
                    hoaDon.TrangThai = TrangThaiHoaDonConst.HOAN_THANH;
                    await _hoaDonRepository.UpdateAsync(hoaDon);
                    return true;
                }
                return false;
            }
        }
        [HttpGet]
        public async Task<bool> UpdateCustomer_toHoaDon(Guid idHoaDon, Guid? idKhachHangNew = null)
        {
            var hoaDon = _hoaDonRepository.FirstOrDefault(x => x.Id == idHoaDon);
            if (hoaDon != null)
            {
                hoaDon.IdKhachHang = idKhachHangNew == Guid.Empty ? null : idKhachHangNew;
                await _hoaDonRepository.UpdateAsync(hoaDon);
                return true;
            }
            return false;
        }

        [HttpPost]
        public async Task Delete_MultipleHoaDon(List<Guid> lstId)
        {
            _hoaDonRepository.GetAll().Where(x => lstId.Contains(x.Id)).ToList().ForEach(x =>
            {
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
                x.TrangThai = TrangThaiHoaDonConst.DA_HUY;
            });
            _hoaDonChiTietRepository.GetAll().Where(x => lstId.Contains(x.IdHoaDon)).ToList().ForEach(x =>
            {
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
                x.TrangThai = TrangThaiHoaDonConst.DA_HUY;
            });
            _hoaDonAnhRepository.GetAll().Where(x => lstId.Contains(x.IdHoaDon)).ToList().ForEach(x =>
            {
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
            });
        }
        [HttpPost]
        public async Task<bool> DeleteMultipleCTHD(List<Guid> lstId)
        {
            try
            {
                _hoaDonChiTietRepository.GetAll().Where(x => lstId.Contains(x.Id)).ToList().ForEach(x =>
                {
                    x.IsDeleted = true;
                    x.DeleterUserId = AbpSession.UserId;
                    x.DeletionTime = DateTime.Now;
                    x.TrangThai = TrangThaiHoaDonConst.DA_HUY;
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpGet]
        public async Task<List<PageHoaDonDto>> GetInforHoaDon_byId(Guid id)
        {
            return await _repoHoaDon.GetInforHoaDon_byId(id);
        }
        [HttpGet]
        public bool CheckExists_MaHoaDon(string maHoaDon)
        {
            var data = _hoaDonRepository.GetAll().Where(x => x.MaHoaDon.Contains(maHoaDon)).Count() > 0;
            return data;
        }
        [HttpGet]
        public async Task<List<PageHoaDonChiTietDto>> GetChiTietHoaDon_byIdHoaDon(Guid idHoaDon)
        {
            return await _repoHoaDon.GetChiTietHoaDon_byIdHoaDon(idHoaDon);
        }
        [HttpGet]
        public PageHoaDonChiTietDto GetChiTietHoaDon_byIdChiTiet(Guid idChiTiet)
        {
            return _repoHoaDon.GetChiTietHoaDon_byIdChiTiet(idChiTiet);
        }

        [HttpPost]
        public async Task<PagedResultDto<PageHoaDonDto>> GetListHoaDon(HoaDonRequestDto param)
        {
            if (param != null)
            {
                param.IdUserLogin = AbpSession?.UserId ?? 1;
            }
            return await _repoHoaDon.GetListHoaDon(param, AbpSession.TenantId ?? 1);
        }
        [HttpPost]
        public async Task<dynamic> GetChiTiet_SuDungGDV_ofCustomer(ParamSearchNhatKyGDV param)
        {
            List<ChiTietSuDungGDV> lst = await _repoHoaDon.GetChiTiet_SuDungGDV_ofCustomer(param);
            var data = lst.GroupBy(x => new { x.MaHoaDon, x.IdHoaDon, x.NgayLapHoaDon }).Select(x => new
            {
                x.Key.IdHoaDon,
                x.Key.MaHoaDon,
                x.Key.NgayLapHoaDon,
                chitiets = x.ToList()
            }).ToList();
            return data;
        }
        [HttpPost]
        public async Task<PagedResultDto<ChiTietNhatKySuDungGDVDto>> GetNhatKySuDungGDV_ofKhachHang(ParamSearchNhatKyGDV param)
        {
            PagedResultDto<ChiTietNhatKySuDungGDVDto> lst = await _repoHoaDon.GetNhatKySuDungGDV_ofKhachHang(param);
            return lst;
        }
        [HttpGet]
        public bool CheckCustomer_hasGDV(Guid customerId)
        {
            var data = _hoaDonRepository.GetAllList().Where(x => x.IdLoaiChungTu == LoaiChungTuConst.GDV
            && x.TrangThai == TrangThaiHoaDonConst.HOAN_THANH && x.IdKhachHang == customerId).Count();
            return data > 0;
        }
        [HttpGet]
        public async Task<bool> CheckGDV_DaSuDung(Guid idGoiDV)
        {
            return await _repoHoaDon.CheckGDV_DaSuDung(idGoiDV);
        }
        [HttpGet]
        public async Task<bool> CheckChiTietGDV_DaSuDung(Guid idChiTietGDV)
        {
            return await _repoHoaDon.CheckChiTietGDV_DaSuDung(idChiTietGDV);
        }
        [HttpGet]
        public async Task<double> GetSoDuTheGiaTri_ofKhachHang(Guid idKhachHang, DateTime? toDate = null)
        {
            return await _repoHoaDon.GetSoDuTheGiaTri_ofKhachHang(idKhachHang, toDate);
        }
        [HttpGet]
        public async Task<bool> CheckTheGiaTri_DaSuDung(Guid idTheGiaTri)
        {
            return await _repoHoaDon.CheckTheGiaTri_DaSuDung(idTheGiaTri);
        }
        public async Task<List<ExcelErrorDto>> CheckData_FileImportTonDauGDV(FileUpload file)
        {
            List<ExcelErrorDto> lstErr = new();
            try
            {
                using MemoryStream stream = new MemoryStream(file.File);
                using var package = new ExcelPackage();
                package.Load(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                // cột A: mã khách hàng (cột thứ 1), đọc từ dòng số 3 đến dòng rowCount
                var errDuplicate = ObjectHelper.Excel_CheckDuplicateData(worksheet, "A", 1, 3, rowCount);
                if (errDuplicate.Count > 0)
                {
                    foreach (var item in errDuplicate)
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = item.RowNumber,
                            TenTruongDuLieu = "Mã khách hàng",
                            GiaTriDuLieu = item.GiaTriDuLieu,
                            DienGiai = "Mã khách hàng bị trùng lặp",
                            LoaiErr = 1,
                        });
                    }
                }
                for (int i = 3; i <= rowCount; i++)
                {
                    bool rowEmpty = true;
                    string maKhachHang = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string maGDV = worksheet.Cells[i, 2].Value?.ToString().Trim();
                    string hanSuDung = worksheet.Cells[i, 3].Value?.ToString().Trim();
                    string maDichVu = worksheet.Cells[i, 4].Value?.ToString().Trim();
                    string soluong = worksheet.Cells[i, 5].Value?.ToString().Trim();
                    string dataType_soluong = worksheet.Cells[i, 5].Value?.GetType()?.ToString().Trim();
                    string dongia = worksheet.Cells[i, 6].Value?.ToString().Trim();
                    string dataType_dongia = worksheet.Cells[i, 6].Value?.GetType()?.ToString()?.Trim();
                    string ghichu = worksheet.Cells[i, 7].Value?.ToString();

                    // nếu dòng trống: bỏ qua và nhảy sang dòng tiếp theo
                    if (!string.IsNullOrEmpty(maDichVu) || !string.IsNullOrEmpty(soluong))
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }

                    if (!string.IsNullOrEmpty(maKhachHang))
                    {
                        var checkExists = await _khachHangService.CheckExistMaKhachHang(maKhachHang);
                        if (!checkExists)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Mã khách hàng",
                                GiaTriDuLieu = maKhachHang,
                                DienGiai = "Mã khách hàng chưa có trên hệ thống",
                                LoaiErr = 1,
                            });
                        }
                    }

                    if (!string.IsNullOrEmpty(maGDV))
                    {
                        var checkExists = CheckExists_MaHoaDon(maGDV);
                        if (checkExists)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Mã gói dịch vụ",
                                GiaTriDuLieu = maGDV,
                                DienGiai = "Mã gói dịch vụ đã tồn tại trên hệ thống",
                                LoaiErr = 1,
                            });
                        }
                    }

                    if (string.IsNullOrEmpty(maDichVu))
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = i,
                            TenTruongDuLieu = "Mã dịch vụ",
                            GiaTriDuLieu = maDichVu,
                            DienGiai = "Mã dịch vụ không được để trống",
                            LoaiErr = 1,
                        });
                    }
                    else
                    {
                        var checkExists = await _hangHoaAppService.CheckExistsMaHangHoa(maDichVu);
                        if (!checkExists)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Mã dịch vụ",
                                GiaTriDuLieu = maDichVu,
                                DienGiai = "Mã dịch vụ chưa có trên hệ thống",
                                LoaiErr = 1,
                            });
                        }
                    }

                    if (string.IsNullOrEmpty(soluong))
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = i,
                            TenTruongDuLieu = "Số lượng",
                            GiaTriDuLieu = soluong,
                            DienGiai = "Số lượng không được để trống",
                            LoaiErr = 1,
                        });
                    }
                    else
                    {
                        bool isNumber = ObjectHelper.Excel_CheckNumber(dataType_soluong);
                        if (!isNumber)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Số lượng",
                                GiaTriDuLieu = soluong,
                                DienGiai = "Số lượng không phải dạng số",
                                LoaiErr = 1,
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(dongia))
                    {
                        bool isNumber = ObjectHelper.Excel_CheckNumber(dataType_dongia);
                        if (!isNumber)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Đơn giá",
                                GiaTriDuLieu = dongia,
                                DienGiai = "Đơn giá không phải dạng số",
                                LoaiErr = 1,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstErr.Add(new ExcelErrorDto
                {
                    RowNumber = -1,
                    TenTruongDuLieu = "Exception",
                    GiaTriDuLieu = "",
                    DienGiai = ex.Message.ToString(),
                    LoaiErr = -1,
                });
            }
            return lstErr;
        }
        public async Task<List<ExcelErrorDto>> ImportFileTonDauGDV(FileUpload file, Guid idChiNhanh)
        {
            List<ExcelErrorDto> lstErr = new();
            try
            {
                using MemoryStream stream = new MemoryStream(file.File);
                using var package = new ExcelPackage();
                package.Load(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                List<BH_HoaDon> lstHD = new List<BH_HoaDon>();
                List<BH_HoaDon_ChiTiet> lstCTHD = new List<BH_HoaDon_ChiTiet>();
                double maxMaHoaDon = await _repoHoaDon.GetMaxNumber_ofMaHoaDon(AbpSession.TenantId ?? 1, idChiNhanh, LoaiChungTuConst.DCTGT, DateTime.Now);

                for (int i = 3; i <= rowCount; i++)
                {
                    bool rowEmpty = true;
                    string maKhachHang = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string maGDV = worksheet.Cells[i, 2].Value?.ToString().Trim();
                    string hanSuDung = worksheet.Cells[i, 3].Value?.ToString().Trim();
                    DateTime? hanSuDungNew = !string.IsNullOrEmpty(hanSuDung) ? Convert.ToDateTime(hanSuDung) : null;
                    string maDichVu = worksheet.Cells[i, 4].Value?.ToString().Trim();
                    string soluong = worksheet.Cells[i, 5].Value?.ToString().Trim();
                    double soLuongNew = double.Parse(soluong);
                    string dongia = worksheet.Cells[i, 6].Value?.ToString().Trim();
                    string ghichu = worksheet.Cells[i, 7].Value?.ToString();

                    // nếu dòng trống: bỏ qua và nhảy sang dòng tiếp theo
                    if (!string.IsNullOrEmpty(maDichVu) || !string.IsNullOrEmpty(soluong))
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }


                    Guid? idKhachHang = null;
                    Guid idHoaDon = Guid.NewGuid();
                    string maHoaDon = maGDV;
                    if (string.IsNullOrEmpty(maKhachHang))
                    {
                        // get gdv last
                        var gdvLast = lstHD.OrderByDescending(x => x.NgayLapHoaDon).FirstOrDefault();
                        if (gdvLast != null)
                        {
                            idKhachHang = gdvLast.IdKhachHang;
                            if (string.IsNullOrEmpty(maGDV))
                            {
                                idHoaDon = gdvLast.Id;
                                goto addChiTietHoaDon;
                            }
                            else
                            {
                                goto addHoaDon;
                            }
                        }
                    }
                    else
                    {
                        idKhachHang = await _khachHangService.GetIdKhachHang_byMaKhachHang(maKhachHang);
                        goto addHoaDon;
                    }

                addHoaDon:
                    {
                        if (string.IsNullOrEmpty(maHoaDon))
                        {
                            maHoaDon = await _loaiChungTuService.GetMaChungTuNew_fromMaxMaChungTu(maxMaHoaDon, LoaiChungTuConst.GDV);
                            maxMaHoaDon += 1;
                        }

                        if (idKhachHang != Guid.Empty)
                        {
                            BH_HoaDon newObj = new()
                            {
                                Id = idHoaDon,
                                TenantId = AbpSession?.TenantId ?? 1,
                                IdLoaiChungTu = LoaiChungTuConst.GDV,
                                IdChiNhanh = idChiNhanh,
                                IdKhachHang = idKhachHang,
                                MaHoaDon = maHoaDon,
                                NgayLapHoaDon = DateTime.Now,
                                NgayHetHan = hanSuDungNew,
                                TongTienHang = 0,
                                TongTienHDSauVAT = 0,
                                TongThanhToan = 0,
                                TongTienHangChuaChietKhau = 0,
                                LaHoaDonDauKy = true,
                                GhiChuHD = "Import tồn đầu gói dịch vụ",
                                TrangThai = TrangThaiHoaDonConst.HOAN_THANH,
                                CreatorUserId = AbpSession.UserId,
                                CreationTime = DateTime.Now
                            };
                            lstHD.Add(newObj);
                        }
                        else
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Mã khách hàng",
                                GiaTriDuLieu = maKhachHang,
                                DienGiai = "Không tìm thấy khách hàng",
                                LoaiErr = 1
                            });
                        }
                    }

                addChiTietHoaDon:
                    {
                        var itemQD = await _hangHoaAppService.GetDMQuyDoi_byMaHangHoa(maDichVu);
                        if (itemQD != null)
                        {
                            int countCTHD = lstCTHD.Where(x => x.IdHoaDon == idHoaDon).Count();
                            var dongiaNew = string.IsNullOrEmpty(dongia) ? itemQD.GiaBan : Convert.ToDouble(dongia);
                            var thanhTien = soLuongNew * dongiaNew;
                            BH_HoaDon_ChiTiet cthd = new()
                            {
                                Id = Guid.NewGuid(),
                                STT = countCTHD + 1,
                                IdHoaDon = idHoaDon,
                                IdDonViQuyDoi = itemQD.Id,
                                SoLuong = soLuongNew,
                                DonGiaTruocCK = dongiaNew,
                                DonGiaSauCK = dongiaNew,
                                DonGiaSauVAT = dongiaNew,
                                ThanhTienSauCK = thanhTien,
                                ThanhTienTruocCK = thanhTien,
                                ThanhTienSauVAT = thanhTien,
                                TenantId = AbpSession?.TenantId ?? 1,
                                TrangThai = TrangThaiHoaDonConst.HOAN_THANH,
                                CreationTime = DateTime.Now,
                                CreatorUserId = AbpSession?.UserId ?? 1,
                            };
                            lstCTHD.Add(cthd);
                        }
                        else
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Mã dịch vụ",
                                GiaTriDuLieu = maDichVu,
                                DienGiai = "Không tìm thấy dịch vụ",
                                LoaiErr = 1
                            });
                        }

                    }
                }
                await _hoaDonRepository.InsertRangeAsync(lstHD);
                await _hoaDonChiTietRepository.InsertRangeAsync(lstCTHD);
            }
            catch (Exception ex)
            {
                lstErr.Add(new ExcelErrorDto
                {
                    RowNumber = -1,
                    TenTruongDuLieu = "Exception",
                    GiaTriDuLieu = "",
                    DienGiai = ex.Message.ToString(),
                    LoaiErr = -1,
                });
            }
            return lstErr;
        }
        [HttpPost]
        public async Task<List<ExcelErrorDto>> CheckData_FileImportTonDauTGT(FileUpload file)
        {
            List<ExcelErrorDto> lstErr = new();
            try
            {
                using MemoryStream stream = new MemoryStream(file.File);
                using var package = new ExcelPackage();
                package.Load(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                // cột B: mã khách hàng (cột thứ 1), đọc từ dòng số 3 đến dòng rowCount
                var errDuplicate = ObjectHelper.Excel_CheckDuplicateData(worksheet, "A", 1, 3, rowCount);
                if (errDuplicate.Count > 0)
                {
                    foreach (var item in errDuplicate)
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = item.RowNumber,
                            TenTruongDuLieu = "Mã khách hàng",
                            GiaTriDuLieu = item.GiaTriDuLieu,
                            DienGiai = "Mã khách hàng bị trùng lặp",
                            LoaiErr = 1,
                        });
                    }
                }
                for (int i = 3; i <= rowCount; i++)
                {
                    bool rowEmpty = true;
                    string maKhachHang = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string tonDauTGT = worksheet.Cells[i, 2].Value?.ToString().Trim();
                    string dataType_tonDauTGT = worksheet.Cells[i, 2].Value?.GetType()?.ToString();

                    // nếu dòng trống: bỏ qua và nhảy sang dòng tiếp theo
                    if (!string.IsNullOrEmpty(maKhachHang) || !string.IsNullOrEmpty(tonDauTGT))
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }


                    if (string.IsNullOrEmpty(maKhachHang))
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = i,
                            TenTruongDuLieu = "Mã khách hàng",
                            GiaTriDuLieu = maKhachHang,
                            DienGiai = "Mã khách hàng không được để trống",
                            LoaiErr = 1,
                        });
                    }
                    else
                    {
                        var checkExists = await _khachHangService.CheckExistMaKhachHang(maKhachHang);
                        if (!checkExists)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Mã khách hàng",
                                GiaTriDuLieu = maKhachHang,
                                DienGiai = "Mã khách hàng chưa có trên hệ thống",
                                LoaiErr = 1,
                            });
                        }
                    }

                    if (string.IsNullOrEmpty(tonDauTGT))
                    {
                        lstErr.Add(new ExcelErrorDto
                        {
                            RowNumber = i,
                            TenTruongDuLieu = "Tồn đầu thẻ",
                            GiaTriDuLieu = tonDauTGT,
                            DienGiai = "Tồn đầu thẻ không được để trống",
                            LoaiErr = 1,
                        });
                    }
                    else
                    {
                        bool isNumber = ObjectHelper.Excel_CheckNumber(dataType_tonDauTGT);
                        if (!isNumber)
                        {
                            lstErr.Add(new ExcelErrorDto
                            {
                                RowNumber = i,
                                TenTruongDuLieu = "Tồn đầu thẻ",
                                GiaTriDuLieu = tonDauTGT,
                                DienGiai = "Tồn đầu thẻ không phải dạng số",
                                LoaiErr = 1,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstErr.Add(new ExcelErrorDto
                {
                    RowNumber = -1,
                    TenTruongDuLieu = "Exception",
                    GiaTriDuLieu = "",
                    DienGiai = ex.Message.ToString(),
                    LoaiErr = -1,
                });
            }
            return lstErr;
        }
        public async Task<List<ExcelErrorDto>> ImportFileImportTonDauTGT(FileUpload file, Guid idChiNhanh)
        {
            List<ExcelErrorDto> lstErr = new();
            try
            {
                using MemoryStream stream = new MemoryStream(file.File);
                using var package = new ExcelPackage();
                package.Load(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                List<BH_HoaDon> lstHD = new List<BH_HoaDon>();
                double maxMaHoaDon = await _repoHoaDon.GetMaxNumber_ofMaHoaDon(AbpSession.TenantId ?? 1, idChiNhanh, LoaiChungTuConst.DCTGT, DateTime.Now);

                for (int i = 3; i <= rowCount; i++)
                {
                    bool rowEmpty = true;
                    string maKhachHang = worksheet.Cells[i, 1].Value?.ToString().Trim();
                    string tonDauTGT = worksheet.Cells[i, 2].Value?.ToString().Trim();
                    double tonDauTGTNew = !string.IsNullOrEmpty(tonDauTGT) ? double.Parse(tonDauTGT) : 0;

                    if (!string.IsNullOrEmpty(maKhachHang) || !string.IsNullOrEmpty(tonDauTGT))
                    {
                        rowEmpty = false;
                    }
                    if (rowEmpty) { continue; }

                    Guid idKhachHang = await _khachHangService.GetIdKhachHang_byMaKhachHang(maKhachHang);
                    string maHoaDon = await _loaiChungTuService.GetMaChungTuNew_fromMaxMaChungTu(maxMaHoaDon, LoaiChungTuConst.DCTGT);

                    BH_HoaDon newObj = new()
                    {
                        Id = Guid.NewGuid(),
                        IdLoaiChungTu = LoaiChungTuConst.DCTGT,
                        TenantId = AbpSession?.TenantId ?? 1,
                        IdChiNhanh = idChiNhanh,
                        IdKhachHang = idKhachHang,
                        MaHoaDon = maHoaDon,
                        NgayLapHoaDon = DateTime.Now,
                        TongTienHang = tonDauTGTNew,
                        TongTienHDSauVAT = tonDauTGTNew,
                        TongThanhToan = tonDauTGTNew,
                        TongTienHangChuaChietKhau = tonDauTGTNew,
                        GhiChuHD = "Import tồn đầu thẻ giá trị",
                        TrangThai = TrangThaiHoaDonConst.HOAN_THANH,
                        LaHoaDonDauKy = true,
                        CreatorUserId = AbpSession.UserId
                    };
                    lstHD.Add(newObj);

                    maxMaHoaDon += 1;
                }
                await _hoaDonRepository.InsertRangeAsync(lstHD);
            }
            catch (Exception ex)
            {
                lstErr.Add(new ExcelErrorDto
                {
                    RowNumber = -1,
                    TenTruongDuLieu = "Exception",
                    GiaTriDuLieu = "",
                    DienGiai = ex.Message.ToString(),
                    LoaiErr = -1,
                });
            }
            return lstErr;
        }

        [AbpAuthorize(PermissionNames.Pages_HoaDon_Export)]
        public async Task<FileDto> ExportDanhSach(HoaDonRequestDto input)
        {
            input.TextSearch = (input.TextSearch ?? string.Empty).Trim();
            input.CurrentPage = 1;
            input.PageSize = int.MaxValue;
            if (input != null)
            {
                input.IdUserLogin = AbpSession?.UserId ?? 1;
            }
            var data = await _repoHoaDon.GetListHoaDon(input, AbpSession.TenantId ?? 1);
            List<PageHoaDonDto> lstHD = (List<PageHoaDonDto>)data.Items;
            var dtNew = lstHD.Select(x =>
            new
            {
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.TenKhachHang,
                x.TongTienHang,
                x.TongThanhToan,
                x.DaThanhToan,
                x.ConNo,
                x.TxtTrangThaiHD
            }).ToList();

            var tieuDe = "DANH SÁCH HÓA ĐƠN";
            var fileName = @"DanhSachHoaDon_"; ;
            if (input?.IdLoaiChungTus?.Count > 0)
            {
                switch (Convert.ToInt32(input?.IdLoaiChungTus[0]))
                {
                    case LoaiChungTuConst.GDV:
                        {
                            tieuDe = "DANH SÁCH GÓI DỊCH VỤ";
                            fileName = "DanhSachGDV_";
                        }
                        break;
                }
            }

            List<Excel_CellData> lst = new()
            {
                new Excel_CellData{RowIndex = 1, ColumnIndex = 1, CellValue= tieuDe}
            };
            return _excelBase.WriteToExcel(fileName, @"GiaoDichThanhToan_Export_Template.xlsx", dtNew, 4, lst, 10);
        }
        public async Task<FileDto> ExportDanhSach_TheGiaTri(HoaDonRequestDto input)
        {
            input.TextSearch = (input.TextSearch ?? string.Empty).Trim();
            input.CurrentPage = 1;
            input.PageSize = int.MaxValue;
            if (input != null)
            {
                input.IdUserLogin = AbpSession?.UserId ?? 1;
            }
            var data = await _repoHoaDon.GetListHoaDon(input, AbpSession.TenantId ?? 1);
            List<PageHoaDonDto> lstHD = (List<PageHoaDonDto>)data.Items;
            var dtNew = lstHD.Select(x =>
            new
            {
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.TenKhachHang,
                x.SoDienThoai,
                x.TongTienHang,
                x.TongGiamGiaHD,
                x.TongThanhToan,
                x.DaThanhToan,
                x.ConNo,
                x.GhiChuHD,
            }).ToList();

            return _excelBase.WriteToExcel(@"DanhSachTGT_", @"Giaodich\Template_DanhSachTheGiaTri.xlsx", dtNew, 4, null, 10);
        }
        public async Task<FileDto> ExportDanhSach_PhieuDieuChinh(HoaDonRequestDto input)
        {
            input.TextSearch = (input.TextSearch ?? string.Empty).Trim();
            input.CurrentPage = 1;
            input.PageSize = int.MaxValue;
            if (input != null)
            {
                input.IdUserLogin = AbpSession?.UserId ?? 1;
            }
            var data = await _repoHoaDon.GetListHoaDon(input, AbpSession.TenantId ?? 1);
            List<PageHoaDonDto> lstHD = (List<PageHoaDonDto>)data.Items;
            var dtNew = lstHD.Select(x =>
            new
            {
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.TenKhachHang,
                x.SoDienThoai,
                x.TongTienHang,
                x.GhiChuHD
            }).ToList();

            return _excelBase.WriteToExcel(@"DanhSachPhieuDieuChinh_", @"Giaodich\Template_DanhSachPhieuDieuChinh.xlsx", dtNew, 4, null, 10);
        }
        [HttpGet]
        public async Task<FileDto> ExportHoaDon_byId(Guid idHoaDon)
        {
            var hd = await _repoHoaDon.GetInforHoaDon_byId(idHoaDon);
            var itemHD = hd.FirstOrDefault();
            var cthd = await _repoHoaDon.GetChiTietHoaDon_byIdHoaDon(idHoaDon);
            var totalRow = cthd.Count + 6;
            List<Excel_CellData> lst = new()
            {
                new Excel_CellData { RowIndex = 1, ColumnIndex = 1, CellValue = "CHI TIẾT GÓI DỊCH VỤ" },
                new Excel_CellData { RowIndex = 2, ColumnIndex = 2, CellValue = itemHD.MaHoaDon },
                new Excel_CellData { RowIndex = 3, ColumnIndex = 2, CellValue = ConvertHelper.ConverDateTimeToString(itemHD.NgayLapHoaDon,"dd/MM/yyyy HH:mm:ss") },
                new Excel_CellData { RowIndex = 2, ColumnIndex = 5, CellValue = itemHD.TenKhachHang },
                new Excel_CellData { RowIndex = 3, ColumnIndex = 5, CellValue = itemHD.SoDienThoai },
                new Excel_CellData { RowIndex = totalRow, ColumnIndex = 5, CellValue = "Tổng phải trả", IsBold = true  },
                new Excel_CellData { RowIndex = totalRow, ColumnIndex = 6, CellValue = itemHD.TongThanhToan.ToString(), IsNumber = true, IsBold = true },
                new Excel_CellData { RowIndex = totalRow + 1, ColumnIndex = 5, CellValue =  "Khách đã trả", IsBold = true  },
                new Excel_CellData { RowIndex = totalRow + 1, ColumnIndex = 6, CellValue = itemHD.DaThanhToan.ToString(),  IsNumber = true, IsBold = true },
            };
            var dtNew = cthd.Select(x =>
            new
            {
                x.MaHangHoa,
                x.TenHangHoa,
                x.SoLuong,
                x.DonGiaTruocCK,
                x.TienChietKhau,
                x.DonGiaSauVAT,
            }).ToList();

            return _excelBase.WriteToExcel(@"ChiTietHoaDon_", @"GiaoDich\Template_ChiTietHoaDon.xlsx", dtNew, 6, lst, -1);
        }

        #region hoadon used to zalo 
        [HttpPost]
        public async Task<List<Zalo_InforHoaDonSend>> Zalo_GetInforHoaDon(List<Guid> arrIdHoaDon)
        {
            var data = _hoaDonRepository.GetAllList(x => arrIdHoaDon.Contains(x.Id)).Select(x => new Zalo_InforHoaDonSend
            {
                Id = x.Id,
                IdKhachHang = x.IdKhachHang,
                MaHoaDon = x.MaHoaDon,
                NgayLapHoaDon = x.NgayLapHoaDon,
                TongTienHang = x.TongTienHang,
            }).ToList();
            return data;
        }
        #endregion
    }
}
