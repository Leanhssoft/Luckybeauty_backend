using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto.Repository;
using BanHangBeautify.Quy.DM_QuyHoaDon.Exporting;
using BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanHangBeautify.Quy.DM_QuyHoaDon
{
    [AbpAuthorize(PermissionNames.Pages_QuyHoaDon)]
    public class QuyHoaDonAppService : SPAAppServiceBase
    {
        private readonly IRepository<QuyHoaDon, Guid> _quyHoaDon;
        private readonly IRepository<QuyHoaDon_ChiTiet, Guid> _quyHoaDonChiTiet;
        private readonly IRepository<DM_LoaiChungTu, int> _loaiChungTuRepository;
        private readonly IQuyHoaDonRepository _repoQuyHD;
        private readonly IQuyHoaDonExcelExporter _quyHoaDonExcelExport;
        public QuyHoaDonAppService(IRepository<QuyHoaDon, Guid> repository, IRepository<DM_LoaiChungTu, int> loaiChungTuRepository,
            IRepository<QuyHoaDon_ChiTiet, Guid> quyHoaDonChiTiet,
            IQuyHoaDonExcelExporter quyHoaDonExcelExporter,
            IQuyHoaDonRepository repoQuyHD)
        {
            _quyHoaDon = repository;
            _loaiChungTuRepository = loaiChungTuRepository;
            _quyHoaDonChiTiet = quyHoaDonChiTiet;
            _repoQuyHD = repoQuyHD;
            _quyHoaDonExcelExport = quyHoaDonExcelExporter;
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
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_QuyHoaDon_Edit)]
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
            oldData.NgayLapHoaDon = input.NgayLapHoaDon;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.TongTienThu = input.TongTienThu;
            oldData.HachToanKinhDoanh = input.HachToanKinhDoanh;
            oldData.NoiDungThu = input.NoiDungThu;
            oldData.TrangThai = input.TrangThai ?? 1;
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

        // not use
        public async Task<CreateOrEditQuyHoaDonDto> CreateQuyHoaDon([FromBody] JObject data)
        {
            List<QuyHoaDon_ChiTiet> lstCTQuy = new();
            QuyHoaDon objHD = ObjectMapper.Map<QuyHoaDon>(data["soquy"].ToObject<QuyHoaDon>());
            List<QuyHoaDon_ChiTiet> dataChiTietHD = ObjectMapper.Map<List<QuyHoaDon_ChiTiet>>(data["soquyChiTiet"].ToObject<List<QuyHoaDon_ChiTiet>>());

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;

            if (string.IsNullOrEmpty(objHD.MaHoaDon))
            {
                var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, objHD.IdChiNhanh ?? null,
                    objHD.IdLoaiChungTu, objHD.NgayLapHoaDon);
                objHD.MaHoaDon = maChungTu;
            }
            foreach (var item in dataChiTietHD)
            {
                QuyHoaDon_ChiTiet ctNew = ObjectMapper.Map<QuyHoaDon_ChiTiet>(item);
                ctNew.Id = Guid.NewGuid();
                ctNew.IdQuyHoaDon = objHD.Id;
                ctNew.TenantId = AbpSession.TenantId ?? 1;
                ctNew.CreatorUserId = AbpSession.UserId;
                ctNew.CreationTime = DateTime.Now;
                lstCTQuy.Add(ctNew);
            }
            await _quyHoaDon.InsertAsync(objHD);
            await _quyHoaDonChiTiet.InsertRangeAsync(lstCTQuy);

            var result = ObjectMapper.Map<CreateOrEditQuyHoaDonDto>(objHD);
            return result;
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Pages_QuyHoaDon_Delete)]
        public async Task<QuyHoaDonDto> Delete(Guid id)
        {
            var data = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.TrangThai = 0;
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;

                var lstQCT = await _quyHoaDonChiTiet.GetAllListAsync(x => x.IdQuyHoaDon == id);
                if (lstQCT != null && lstQCT.Count > 0)
                {
                    lstQCT.ForEach(x => { x.DeleterUserId = AbpSession.UserId; x.DeletionTime = DateTime.Now; });
                }

                await _quyHoaDon.UpdateAsync(data);
                return ObjectMapper.Map<QuyHoaDonDto>(data);
            }
            return new QuyHoaDonDto();
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
                    quyHD.TrangThai = 0;
                    await _quyHoaDon.UpdateAsync(quyHD);
                }
                lstQCT.ForEach(x => { x.DeleterUserId = AbpSession.UserId; x.DeletionTime = DateTime.Now; });
            }
        }
        public async Task<CreateOrEditQuyHoaDonDto> GetForEdit(Guid id)
        {
            var data = await _quyHoaDon.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                // get ctquy
                var ctQuy = await _quyHoaDonChiTiet.GetAllListAsync(x => x.IdQuyHoaDon == id);
                var result = ObjectMapper.Map<CreateOrEditQuyHoaDonDto>(data);
                if (ctQuy != null)
                {
                    result.QuyHoaDon_ChiTiet = ObjectMapper.Map<List<QuyHoaDonChiTietDto>>(ctQuy);
                }
                return result;
            }
            return new CreateOrEditQuyHoaDonDto();
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
        [AbpAuthorize(PermissionNames.Pages_QuyHoaDon_Export)]
        public async Task<FileDto> ExportToExcel(PagedQuyHoaDonRequestDto input)
        {
            input.TenantId = AbpSession.TenantId ?? 1;
            input.CurrentPage = 0;
            input.PageSize = int.MaxValue;
            var data = await _repoQuyHD.Search(input);
            List<GetAllQuyHoaDonItemDto> model = new List<GetAllQuyHoaDonItemDto>();
            model = (List<GetAllQuyHoaDonItemDto>)data.Items;
            return _quyHoaDonExcelExport.ExportDanhSachQuyHoaDon(model);
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
