﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.AppCommon;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Entities;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto.Repository;
using BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BanHangBeautify.Quy.DM_QuyHoaDon
{
    [AbpAuthorize(PermissionNames.Pages_QuyHoaDon)]
    public class QuyHoaDonAppService : SPAAppServiceBase
    {
        private readonly IRepository<QuyHoaDon, Guid> _quyHoaDon;
        private readonly IRepository<QuyHoaDon_ChiTiet, Guid> _quyHoaDonChiTiet;
        private readonly IRepository<DM_LoaiChungTu, int> _loaiChungTuRepository;
        private readonly IQuyHoaDonRepository _repoQuyHD;
        private readonly IExcelBase _excelBase;
        public QuyHoaDonAppService(IRepository<QuyHoaDon, Guid> repository, IRepository<DM_LoaiChungTu, int> loaiChungTuRepository,
            IRepository<QuyHoaDon_ChiTiet, Guid> quyHoaDonChiTiet,
            IExcelBase excelBase,
            IQuyHoaDonRepository repoQuyHD)
        {
            _quyHoaDon = repository;
            _loaiChungTuRepository = loaiChungTuRepository;
            _quyHoaDonChiTiet = quyHoaDonChiTiet;
            _repoQuyHD = repoQuyHD;
            _excelBase = excelBase;
        }
        [AbpAuthorize(PermissionNames.Pages_QuyHoaDon_Create)]
        public async Task<QuyHoaDonDto> Create(CreateOrEditQuyHoaDonDto input)
        {
            List<QuyHoaDon_ChiTiet> lstCT = new();

            if (string.IsNullOrEmpty(input.MaHoaDon))
            {
                var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, input.IdChiNhanh, input.IdLoaiChungTu, input.NgayLapHoaDon);
                input.MaHoaDon = maChungTu;
            }
            input.NgayLapHoaDon = ObjectHelper.AddTimeNow_forDate(input.NgayLapHoaDon);
            QuyHoaDon data = ObjectMapper.Map<QuyHoaDon>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;

            // insert quyct
            if (input.QuyHoaDon_ChiTiet != null && input.QuyHoaDon_ChiTiet.Count > 0)
            {
                foreach (var item in input.QuyHoaDon_ChiTiet)
                {
                    QuyHoaDon_ChiTiet ctNew = ObjectMapper.Map<QuyHoaDon_ChiTiet>(item);
                    ctNew.Id = Guid.NewGuid();
                    ctNew.IdQuyHoaDon = data.Id;
                    ctNew.TenantId = AbpSession.TenantId ?? 1;
                    ctNew.CreatorUserId = AbpSession.UserId;
                    ctNew.CreationTime = DateTime.Now;
                    lstCT.Add(ctNew);
                }
            }
            await _quyHoaDon.InsertAsync(data);
            await _quyHoaDonChiTiet.InsertRangeAsync(lstCT);

            var result = ObjectMapper.Map<QuyHoaDonDto>(data);
            return result;
        }
        /// <summary>
        /// dùng cho update soquy (không liên quan đến hóa đơn)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QuyHoaDonDto> UpdateQuyHoaDon(CreateOrEditQuyHoaDonDto input)
        {
            var oldData = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (string.IsNullOrEmpty(input.MaHoaDon))
            {
                var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, input.IdChiNhanh, input.IdLoaiChungTu, input.NgayLapHoaDon);
                input.MaHoaDon = maChungTu;
            }
            oldData.MaHoaDon = input.MaHoaDon;
            oldData.IdLoaiChungTu = input.IdLoaiChungTu;
            oldData.NgayLapHoaDon = ObjectHelper.AddTimeNow_forDate(input.NgayLapHoaDon); ;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.IdBrandname = input.IdBrandname;
            oldData.TongTienThu = input.TongTienThu;
            oldData.HachToanKinhDoanh = input.HachToanKinhDoanh;
            oldData.NoiDungThu = input.NoiDungThu;
            oldData.TrangThai = input.TrangThai ?? TrangThaiSoQuyConst.DA_THANH_TOAN;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;

            if (input.QuyHoaDon_ChiTiet != null && input.QuyHoaDon_ChiTiet.Count > 0)
            {
                foreach (var item in input.QuyHoaDon_ChiTiet)
                {
                    QuyHoaDon_ChiTiet ctUpdate = await _quyHoaDonChiTiet.FirstOrDefaultAsync(item.Id);
                    ctUpdate.IdHoaDonLienQuan = item.IdHoaDonLienQuan;
                    ctUpdate.IdKhachHang = item.IdKhachHang;
                    ctUpdate.IdNhanVien = item.IdNhanVien;
                    ctUpdate.IdTaiKhoanNganHang = item.IdTaiKhoanNganHang;
                    ctUpdate.IdKhoanThuChi = item.IdKhoanThuChi;
                    ctUpdate.TienThu = item.TienThu;
                    ctUpdate.DiemThanhToan = item.DiemThanhToan;
                    ctUpdate.ChiPhiNganHang = item.ChiPhiNganHang;
                    ctUpdate.LaPTChiPhiNganHang = item.LaPTChiPhiNganHang;
                    ctUpdate.ThuPhiTienGui = item.ThuPhiTienGui;
                    ctUpdate.LastModificationTime = DateTime.Now;
                    ctUpdate.LastModifierUserId = AbpSession.UserId;
                    await _quyHoaDonChiTiet.UpdateAsync(ctUpdate);
                }
            }
            await _quyHoaDon.UpdateAsync(oldData);

            var result = ObjectMapper.Map<QuyHoaDonDto>(oldData);
            return result;
        }
        /// <summary>
        /// dùng cho phiếu thu/chi liên quan đến hóa đơn (xóa hẳn quyCT khỏi DB) và add new ct
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<QuyHoaDonDto> UpdateQuyHD_RemoveCT_andAddAgain(CreateOrEditQuyHoaDonDto input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var oldData = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (string.IsNullOrEmpty(input.MaHoaDon))
                {
                    var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, input.IdChiNhanh, input.IdLoaiChungTu, input.NgayLapHoaDon);
                    input.MaHoaDon = maChungTu;
                }
                oldData.MaHoaDon = input.MaHoaDon;
                oldData.IdLoaiChungTu = input.IdLoaiChungTu;
                oldData.NgayLapHoaDon = input.NgayLapHoaDon;
                oldData.IdChiNhanh = input.IdChiNhanh;
                oldData.IdBrandname = input.IdBrandname;
                oldData.TongTienThu = input.TongTienThu;
                oldData.HachToanKinhDoanh = input.HachToanKinhDoanh;
                oldData.NoiDungThu = input.NoiDungThu;
                oldData.TrangThai = input.TrangThai ?? TrangThaiSoQuyConst.DA_THANH_TOAN;
                oldData.LastModificationTime = DateTime.Now;
                oldData.LastModifierUserId = AbpSession.UserId;

                // delete & add again
                var lstQuyCTOld = await _quyHoaDonChiTiet.GetAllListAsync(x => x.IdQuyHoaDon == input.Id);
                foreach (var item in lstQuyCTOld)
                {
                    await _quyHoaDonChiTiet.HardDeleteAsync(item);
                }

                if (input.QuyHoaDon_ChiTiet != null && input.QuyHoaDon_ChiTiet.Count > 0)
                {
                    foreach (var item in input.QuyHoaDon_ChiTiet)
                    {
                        QuyHoaDon_ChiTiet ctNew = ObjectMapper.Map<QuyHoaDon_ChiTiet>(item);
                        ctNew.Id = Guid.NewGuid();
                        ctNew.IdQuyHoaDon = input.Id;
                        ctNew.TenantId = AbpSession.TenantId ?? 1;
                        ctNew.CreatorUserId = AbpSession.UserId;
                        ctNew.CreationTime = DateTime.Now;
                        await _quyHoaDonChiTiet.InsertAsync(ctNew);
                    }
                }
                await _quyHoaDon.UpdateAsync(oldData);

                var result = ObjectMapper.Map<QuyHoaDonDto>(oldData);
                return result;
            }
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Pages_QuyHoaDon_Delete)]
        public async Task<QuyHoaDonDto> Delete(Guid id)
        {
            var data = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.TrangThai = TrangThaiSoQuyConst.DA_HUY;
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;

                var lstQCT = await _quyHoaDonChiTiet.GetAllListAsync(x => x.IdQuyHoaDon == id);
                if (lstQCT != null && lstQCT.Count > 0)
                {
                    lstQCT.ForEach(x => { x.IsDeleted = true; x.DeleterUserId = AbpSession.UserId; x.DeletionTime = DateTime.Now; });
                }

                await _quyHoaDon.UpdateAsync(data);
                return ObjectMapper.Map<QuyHoaDonDto>(data);
            }
            return new QuyHoaDonDto();
        }
        [HttpGet]
        public async Task<QuyHoaDonDto> KhoiPhucSoQuy(Guid idQuyHoaDon)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var data = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == idQuyHoaDon);
                if (data != null)
                {
                    data.TrangThai = TrangThaiSoQuyConst.DA_THANH_TOAN;
                    data.IsDeleted = false;
                    data.LastModifierUserId = AbpSession.UserId;
                    data.LastModificationTime = DateTime.Now;

                    // nhiều chi tiết quá --> lấy những chi tiết dc xóa lần cuối
                    var lastDeletionTime = ConvertHelper.ConverDateTimeToString(data.DeletionTime, "yyy-MM-dd HH:mm:ss");
                    var ctQuyAll = _quyHoaDonChiTiet.GetAllList(x => x.IdQuyHoaDon == idQuyHoaDon).AsEnumerable();
                    var ctQuy = ctQuyAll.Where(x => ConvertHelper.ConverDateTimeToString(data.DeletionTime, "yyy-MM-dd HH:mm:ss") == lastDeletionTime).ToList();

                    if (ctQuy != null && ctQuy.Count > 0)
                    {
                        ctQuy.ForEach(x => { x.LastModifierUserId = AbpSession.UserId; x.LastModificationTime = DateTime.Now; });
                    }

                    await _quyHoaDon.UpdateAsync(data);
                    return ObjectMapper.Map<QuyHoaDonDto>(data);
                }
                return new QuyHoaDonDto();
            }
        }

        [HttpGet]
        public bool UpdateCustomer_toQuyChiTiet(Guid idHoaDonLienQuan, Guid? idKhachHangNew = null)
        {
            idKhachHangNew = idKhachHangNew == Guid.Empty ? null : idKhachHangNew;
            _quyHoaDonChiTiet.GetAll().Where(x => x.IdHoaDonLienQuan == idHoaDonLienQuan).ToList().ForEach(x =>
            {
                x.IdKhachHang = idKhachHangNew;
                x.LastModifierUserId = AbpSession.UserId;
                x.LastModificationTime = DateTime.Now;
            });
            return true;
        }
        /// <summary>
        /// only update quyHD create first
        /// </summary>
        /// <param name="idHoaDonLienQuan"></param>
        /// <param name="ngayLapHDNew"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> UpdateNgayLapQuyHD_ifChangeNgayLapHD(Guid idHoaDonLienQuan, DateTime ngayLapHDNew)
        {
            var arrIDQuyHD = _quyHoaDonChiTiet.GetAll().Where(x => x.IdHoaDonLienQuan == idHoaDonLienQuan).Select(x => x.IdQuyHoaDon).ToList();
            var firstQuyHD = _quyHoaDon.GetAll().Where(x => arrIDQuyHD.Contains(x.Id)).OrderBy(x => x.NgayLapHoaDon).FirstOrDefault();
            if (firstQuyHD != null)
            {
                firstQuyHD.NgayLapHoaDon = ngayLapHDNew;
                await  _quyHoaDon.UpdateAsync(firstQuyHD);
            }
            return true;
        }

        [HttpPost]
        public async Task DeleteMultiple_QuyHoaDon(List<Guid> lstId)
        {
            _quyHoaDon.GetAllList(x => lstId.Contains(x.Id)).ToList().ForEach(x =>
            {
                x.TrangThai = TrangThaiSoQuyConst.DA_HUY;
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
            });
            _quyHoaDonChiTiet.GetAllList(x => lstId.Contains(x.IdQuyHoaDon)).ToList().ForEach(x =>
            {
                x.IsDeleted = true;
                x.DeleterUserId = AbpSession.UserId;
                x.DeletionTime = DateTime.Now;
            });
        }
        [HttpGet]
        public async Task HuyPhieuThuChi_ofHoaDonLienQuan(Guid idHoaDonLienQuan)
        {
            var lstQCT = await _quyHoaDonChiTiet.GetAllListAsync(x => x.IdHoaDonLienQuan == idHoaDonLienQuan);
            if (lstQCT != null && lstQCT.Count > 0)
            {
                Guid idQuyHD = lstQCT.FirstOrDefault().IdQuyHoaDon;
                var quyHD = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == idQuyHD);
                if (quyHD != null)
                {
                    quyHD.DeleterUserId = AbpSession.UserId;
                    quyHD.DeletionTime = DateTime.Now;
                    quyHD.TrangThai = TrangThaiHoaDonConst.DA_HUY;
                    quyHD.IsDeleted = true;
                    await _quyHoaDon.UpdateAsync(quyHD);
                }
                lstQCT.ForEach(x => { x.DeleterUserId = AbpSession.UserId; x.DeletionTime = DateTime.Now; x.IsDeleted = true; });
            }
        }

        [HttpGet]
        public async Task<CreateOrEditQuyHoaDonDto> GetForEdit(Guid id)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var data = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == id);
                if (data != null)
                {
                    if (data.IsDeleted)
                    {
                        // đã xóa: get chi tiết bị xóa cuối cùng --> sử dụng khi muốn xem lại phiếu hủy
                        // .AsEnumerable() hoặc .ToList thì mới ConverDateTimeToString
                        var lastDeletionTime = ConvertHelper.ConverDateTimeToString(data.DeletionTime, "yyy-MM-dd HH:mm:ss");
                        var ctQuyAll = _quyHoaDonChiTiet.GetAllList(x => x.IdQuyHoaDon == id).AsEnumerable();
                        var ctQuy = ctQuyAll.Where(x => ConvertHelper.ConverDateTimeToString(x.DeletionTime, "yyy-MM-dd HH:mm:ss") == lastDeletionTime);
                        var result = ObjectMapper.Map<CreateOrEditQuyHoaDonDto>(data);
                        if (ctQuy != null)
                        {
                            result.QuyHoaDon_ChiTiet = ObjectMapper.Map<List<QuyHoaDonChiTietDto>>(ctQuy.OrderBy(x => x.HinhThucThanhToan));
                        }
                        return result;
                    }
                    else
                    {
                        // nếu chưa xóa: chỉ get những chi tiết chưa bị xóa
                        var ctQuy = await _quyHoaDonChiTiet.GetAllListAsync(x => x.IdQuyHoaDon == id && x.IsDeleted == false);
                        var result = ObjectMapper.Map<CreateOrEditQuyHoaDonDto>(data);
                        if (ctQuy != null)
                        {
                            // nếu thanh toán kết hợp: order hinhThucThanhToan theo thứ tu: mạt, ck, pos
                            result.QuyHoaDon_ChiTiet = ObjectMapper.Map<List<QuyHoaDonChiTietDto>>(ctQuy.OrderBy(x => x.HinhThucThanhToan));
                        }
                        return result;
                    }
                }
                return new CreateOrEditQuyHoaDonDto();
            }
        }
        [HttpGet]
        public async Task<List<QuyHoaDonChiTietDto>> GetQuyChiTiet_byIQuyHoaDon(Guid idQuyHoaDon)
        {
            var ctQuy = await _repoQuyHD.GetQuyChiTiet_byIQuyHoaDon(idQuyHoaDon);
            return ctQuy;
        }
        public async Task<List<QuyHoaDonViewItemDto>> GetNhatKyThanhToan_ofHoaDon(Guid idHoadonLienQuan)
        {
            var data = await _repoQuyHD.GetNhatKyThanhToan_ofHoaDon(idHoadonLienQuan);
            return data;
        }
        public async Task<PagedResultDto<GetAllQuyHoaDonItemDto>> GetAll(PagedQuyHoaDonRequestDto input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            return await _repoQuyHD.Search(input);
        }
        public async Task<ThuChi_DauKyCuoiKyDto> GetThuChi_DauKyCuoiKy(PagedQuyHoaDonRequestDto input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            return await _repoQuyHD.GetThuChi_DauKyCuoiKy(input);
        }
        [AbpAuthorize(PermissionNames.Pages_QuyHoaDon_Export)]
        public async Task<FileDto> ExportExcelQuyHoaDon(PagedQuyHoaDonRequestDto input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            var data = await _repoQuyHD.Search(input);
            List<GetAllQuyHoaDonItemDto> lstQuy = (List<GetAllQuyHoaDonItemDto>)data.Items;
            var dataNew = lstQuy.Select(x => new
            {
                x.LoaiPhieu,
                x.MaHoaDon,
                x.NgayLapHoaDon,
                x.TenNguoiNop,
                x.MaHoaDonLienQuans,
                x.TienMat,
                x.TienChuyenKhoan,
                x.TienQuyetThe,
                x.TongTienThu,
                x.NoiDungThu,
                x.TxtTrangThai
            }).ToList();
            FileDto ff = _excelBase.WriteToExcel("DanhSachThuChi_", "SoQuy_Export_Template.xlsx", dataNew, 4, null, 20);
            return ff;
        }
        [HttpGet]
        public async Task<bool> CheckExistsMaPhieuThuChi(string maphieu, Guid? id = null)
        {
            if (id != null && id != Guid.Empty)
            {
                var lst = await _quyHoaDon.GetAllListAsync(x => x.Id != id && x.MaHoaDon.ToUpper() == maphieu.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lst = await _quyHoaDon.GetAllListAsync(x => x.MaHoaDon.ToUpper() == maphieu.Trim().ToUpper());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
